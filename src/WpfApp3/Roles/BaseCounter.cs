using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SqlSugar;
using WpfApp3.Common;
using WpfApp3.Data;
using Wuhua.Main.Common;
using Wuhua.Main.Common.Extensions;
using Wuhua.Main.Data;
using Wuhua.Main.Internfaces;
using Wuhua.Main.Roles.Strategy;
using Wuhua.Main.Weapon;
using Wuhua.Model;
using Wuhua.NLog;
using static Unity.Storage.RegistrationSet;

namespace WpfApp3.Roles
{

    public class BaseCounter : RolesBase
    {
        public const int BaseCriticalDamage = 150;
        public const decimal BaseCriticalBonus = 1;

        private IDamageStrategy _damageStrategy;
        public void SetDamageStrategy(IDamageStrategy strategy) => _damageStrategy = strategy;
        private readonly WeaponBuffGroups _weaponBuffGroups = new();
        List<List<Tuple<List<string>, int>>> compairResults = new List<List<Tuple<List<string>, int>>>();
        private WeaponIncreManager weaponIncreManager;
        public override IEnumerable<string> GetOutPut()
        {

            try
            {
                if (_skillList == null || _skillList.Count == 0)
                {
                    MessageBox.Show("技能数值异常");
                    return base.GetOutPut();
                }
                compairResults.Clear();
                ResultList.Clear();
                ResultList.Add($"【{_rolesVm.CurrentRole.SelectedRole.RoleName}】武器:{_rolesVm.CurrentRole.SelectedWeapon.Weapon}");
                if (WeaponEntryList.Count > 0)
                {
                    // 计算组合最优解
                    var entryList = PermutationAndCombination<IncreInfo>.GetCombination(WeaponEntryList.ToArray(), 3);
                    foreach (var entry in entryList)
                    {
                        var retsult = ComposeDamage(entry.ToList());
                        compairResults.Add(retsult);
                    }
                    var tempList = compairResults.OrderByDescending(i => i.FirstOrDefault().Item2);
                    foreach (var item in tempList)
                    {
                        var item2 = item.Select(i => i.Item1);
                        foreach (var itemD in item2)
                        {
                            ResultList.AddRange(itemD);
                        }
                        ResultList.Add("");
                    }
                }
                else
                {
                    var result = ComposeDamage();
                    foreach (var str in result)
                    {
                        ResultList.AddRange(str.Item1);
                    }
                }
                return ResultList;
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
                return ResultList;
            }
        }
        public List<Tuple<List<string>, int>> ComposeDamage(List<IncreInfo> entryList = null)
        {
            _damageStrategy = _rolesVm.CurrentRole.SelectedRole.Post switch
            {
                _ => new DefaultDamageStrategy(_rolesVm, _weaponBuffGroups),
            };
            weaponIncreManager = new WeaponIncreManager();
            LoggerHelper.Logger.Info($"计算开始");
            try
            {
                // 局内攻击力
                var Atk = GetGameAtk(entryList);
                List<Tuple<List<string>, int>> tuples = new List<Tuple<List<string>, int>>();


                //// exist bug, make list need lock
                //Parallel.ForEach(_monsters, (monster, ParallelLoopState) =>
                //{
                //    if ((!int.TryParse(monster.Def, out int monsterDef) || monsterDef <= 0))
                //    {
                //        ParallelLoopState.Break();
                //        return;
                //    }
                foreach (var monster in _monsters)
                {
                    if ((!int.TryParse(monster.Def, out int monsterDef) || monsterDef <= 0))
                    {
                        //ParallelLoopState.Break();
                        continue;
                    }
                    List<string> resultStrList = new List<string>();
                    resultStrList.Add($"怪物等级：{monster.Level}防御：{monster.Def}血量：{monster.Blood}");
                    resultStrList.Add($"基础攻击力{Atk.baseAtk}");
                    resultStrList.Add($"局内攻击力{Atk.gameAtk}");
                    int allDamage = 0;
                    resultStrList.Add(PrintWeaponInfo(entryList));
                    LoggerHelper.Logger.Info($"按Monster--开始");
                    weaponIncreManager.SetWeaponInstance(_rolesVm.CurrentRole.SelectedWeapon);
                    foreach (var item in _skillList)
                    {
                        LoggerHelper.Logger.Info($"按Skill--开始");
                        // 处理武器加成
                        var weaponIncres = weaponIncreManager.GetWeapoIncre(item);
                        SplitWeaponIncre(weaponIncres);
                        // 计算组合最优解
                        if (entryList != null)
                        {
                            SplitWeaponIncre(TranseferIncreInfo(entryList));
                        }
                        // 真实伤害
                        if (item.DamageType == DamageType.Authentic)
                        {
                            CountAuthentic(resultStrList, Atk.gameAtk, monster, item, ref allDamage);
                            continue;
                        }
                        var defDown = _damageStrategy.CalculateDefenceReduction(monster);
                        var criticalBonus = _damageStrategy.CalculateCriticalBonus(item);
                        var skillInjury = _damageStrategy.CalculateSkill(item);
                        var injury = _damageStrategy.CalculateInjuryMultiplier(item);
                        Debug.WriteLine($"defDown:{defDown},criticalBonus:{criticalBonus},skillInjury{skillInjury},injury:{injury}");
                        // 最终计算
                        CountSingleDamage(resultStrList, item, Atk.gameAtk, defDown, monster, criticalBonus * skillInjury * injury, ref allDamage);
                    }
                    var finalResult = $"总计:{allDamage}";
                    LoggerHelper.Logger.Info(finalResult);
                    resultStrList.Add(finalResult);
                    tuples.Add(Tuple.Create(resultStrList, allDamage));
                };
                return tuples;
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message);
                return null;
            }
        }

        private List<ShowIncreInfo> TranseferIncreInfo(List<IncreInfo> increInfos)
        {
            List<ShowIncreInfo> showIncreInfos = new List<ShowIncreInfo>();
            foreach (var increInfo in increInfos)
            {
                showIncreInfos.Add(new ShowIncreInfo()
                {
                    Title = "武器",
                    IncreNum = increInfo.IncreNum.ToString(),
                    SelectedIncre = increInfo
                });
            }
            return showIncreInfos;
        }
        #region Tool func
        private void SplitWeaponIncre(List<ShowIncreInfo> weaponIncres)
        {
            foreach (var showIncreInfo in weaponIncres)
            {
                switch ((IncreClass)showIncreInfo.SelectedIncre.IncreClass)
                {
                    case IncreClass.AtkIncre:
                        _weaponBuffGroups.AtkIncres.Add(showIncreInfo); break;
                    case IncreClass.DeDef:
                        _weaponBuffGroups.DefIncres.Add(showIncreInfo); break;
                    case IncreClass.Critical:
                        _weaponBuffGroups.CraIncres.Add(showIncreInfo); break;
                    default: break;
                }
            }
            var increList = weaponIncres.Where(i => i.SelectedIncre.IncreClass.Equals((int)IncreClass.DamageIncre));
            foreach (var incre in increList)
            {
                switch ((IncreType)incre.SelectedIncre.IncreType)
                {
                    case IncreType.Percent:
                        _weaponBuffGroups.AllHurtList.Add(incre); break;
                    case IncreType.Number:
                        _weaponBuffGroups.AtkTypeList.Add(incre); break;
                    case IncreType.DamageType:
                        _weaponBuffGroups.DamageTypeList.Add(incre); break;
                    case IncreType.TargetType:
                        _weaponBuffGroups.TargetTypeList.Add(incre); break;
                    default: break;
                }
            }
            var vlunList = weaponIncres.Where(i => i.SelectedIncre.IncreClass.Equals((int)IncreClass.WeakIncre));
            foreach (var vlun in vlunList)
            {
                switch ((IncreType)vlun.SelectedIncre.IncreType)
                {
                    case IncreType.Percent:
                        _weaponBuffGroups.AllVulList.Add(vlun); break;
                    case IncreType.Number:
                        _weaponBuffGroups.AtkTypeVulList.Add(vlun); break;
                    case IncreType.DamageType:
                        _weaponBuffGroups.DamageTypeVulList.Add(vlun); break;
                    case IncreType.TargetType:
                        _weaponBuffGroups.TargetTypeVulList.Add(vlun); break;
                    default: break;
                }
            }

        }
        private string ComposeResult(Monster monster, SkillItem skill, int damage)
        {
            string str = $"对{monster.Level}级-{monster.Def}防敌人造成[{CommonStaticSource.AtkTypeDic[skill.AtkType]}]" +
                $"[{CommonStaticSource.DamageTypeDic[skill.DamageType]}] {skill.DamageTimes}次共：{damage}点";
            return str;

        }
        private static readonly ConcurrentDictionary<string, int> BonusCache = new ConcurrentDictionary<string, int>();

        // 比较类型后获取加成数值
        public int GetCompairedBonus(IEnumerable<ShowIncreInfo> AtkBonusList, int type)
        {
            var cacheKey = $"{AtkBonusList.GetHashCode()}-{type}";
            return BonusCache.GetOrAdd(cacheKey, _ =>
                AtkBonusList
                .Where(i => i.SelectedIncre.IncreDetail == type)
                .Sum(i => string.IsNullOrEmpty(i.IncreNum) ? 0 : int.Parse(i.IncreNum)));
        }
        #endregion
        #region count func
        private (decimal baseAtk, decimal gameAtk) GetGameAtk(List<IncreInfo> entryList)
        {
            var baseAtk = _rolesVm.RoleAtk;
            if (entryList != null)
            {
                var weaponAtk = entryList.Where(i => i.IncreClass == (int)IncreClass.BaseIncre && i.IncreType == (int)IncreType.Number);
                if (weaponAtk.Count() > 0)
                {
                    baseAtk += weaponAtk.First().IncreNum;
                }
                var weaponAtkPer = entryList.Where(i => i.IncreClass == (int)IncreClass.BaseIncre && i.IncreType == (int)IncreType.Percent);
                if (weaponAtkPer.Count() > 0)
                {
                    baseAtk += (int)(_rolesVm.Role.BaseAtk * weaponAtkPer.First().IncreNum / (decimal)100);
                }
            }
            string baseAtkLog = $"初始攻击力:{baseAtk}";
            decimal gameAtk = baseAtk * (1 + _rolesVm.AtkBonusPerList.SumNumericalValues() / (decimal)100) + _rolesVm.AtkBonusList.SumNumericalValues();
            string atkLog = $"局内攻击力:{gameAtk}";
            LoggerHelper.Logger.Info(atkLog);
            return (baseAtk, gameAtk);
        }
        private string PrintWeaponInfo(List<IncreInfo> entryList = null)
        {
            string info = "";
            if (entryList != null)
            {
                foreach (var itme in entryList)
                {
                    info += $"[{itme.IncreName}:{itme.IncreNum} ]";
                }
            }
            else
            {
                foreach (var itme in _rolesVm.CurrentRole.SelectedWeapon.WeaponEfList)
                {
                    info += $"[{itme.SelectedIncre.IncreName}:{itme.SelectedIncre.IncreNum} ]";

                }
            }
            return info;
        }
        private void CountAuthentic(List<string> resultStrList, decimal gameAtk, Monster monster, SkillItem item, ref int allDamage)
        {
            decimal result = (int)(gameAtk * int.Parse(item.SkillNum) / (decimal)100 * int.Parse(item.DamageTimes));
            allDamage += (int)result;
            _weaponBuffGroups.ClearAll();
            var singleResult = ComposeResult(monster, item, (int)result);
            LoggerHelper.Logger.Info($"本次结果：{singleResult}");
            resultStrList.Add(singleResult);
        }
        private void CountSingleDamage(List<string> resultStrList, SkillItem item, decimal gameAtk, decimal defDown, Monster monster, Decimal criticalBonus, ref int allDamage)
        {
            decimal result = 0;
            // 单次伤害
            if (item.AtkType != AtkType.Contential)
            {
                result = (int)(gameAtk * int.Parse(item.SkillNum) / (decimal)100 * int.Parse(item.DamageTimes) * criticalBonus);

            }
            else
            {
                if (item.DamageType == DamageType.Burn)
                {
                    result = (int)(gameAtk * 20 / (decimal)100 * int.Parse(item.DamageTimes) * int.Parse(item.SkillNum) * criticalBonus);
                }
                else if (item.DamageType == DamageType.Melt)
                {
                    var dam = allDamage < int.Parse(monster.Blood) * (decimal)0.1 ? int.Parse(monster.Blood) * (decimal)0.01 : allDamage * (decimal)0.1;
                    result = (int)(dam * int.Parse(item.DamageTimes) * int.Parse(item.SkillNum) * criticalBonus);
                }
                else if (item.DamageType == DamageType.LostBlood)
                {
                    result = (int)(int.Parse(monster.Blood) * (decimal)0.05 * int.Parse(item.DamageTimes) * int.Parse(item.SkillNum) * criticalBonus);
                }
            }
            result *= defDown;
            Interlocked.Add(ref allDamage, (int)result);
            _weaponBuffGroups.ClearAll();
            var singleResult = ComposeResult(monster, item, (int)result);
            LoggerHelper.Logger.Info($"本次结果：{singleResult}");
            if (WeaponEntryList.Count() > 0) return;
            resultStrList.Add(singleResult);

        }
        #endregion

    }
}
