using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using SqlSugar;
using WpfApp3.Common;
using WpfApp3.Data;
using Wuhua.Main.Common;
using Wuhua.Main.Internfaces;
using Wuhua.Main.Weapon;
using Wuhua.Model;
using Wuhua.NLog;
using static Unity.Storage.RegistrationSet;

namespace WpfApp3.Roles
{
    public class MogaoGrottoes : RolesBase
    {
        List<ShowIncreInfo> wAtkIncres = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wDefIncres = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wCraIncres = new List<ShowIncreInfo>();

        List<ShowIncreInfo> wAllHurtList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wAtkTypeList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wDamageTypeList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wTargetTypeList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wAllVulList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wAtkTypeVulList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wDamageTypeVulList = new List<ShowIncreInfo>();
        List<ShowIncreInfo> wTargetTypeVulList = new List<ShowIncreInfo>();
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
                        var retsult = ComposeDamage(entry);
                        compairResults.Add(retsult);
                    }
                    var tempList = compairResults.OrderByDescending(i => i.FirstOrDefault().Item2);
                    foreach (var item in tempList)
                    {
                        var item2 = item.Select(i => i.Item1).ToList();
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
                return ResultList.ToList();
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
                return ResultList.ToList();
            }
        }

        public List<Tuple<List<string>, int>> ComposeDamage(IncreInfo[] entryList = null)
        {
            weaponIncreManager = new WeaponIncreManager();
            LoggerHelper.Logger.Info($"计算开始");
            try
            {
                // 局内攻击力
                var Atk = GetGameAtk(entryList);


                //分离特殊伤害列表,如暴击伤害增伤
                var spDamageTypeInjuryList = _rolesVm.InjuryVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType == (int)IncreType.SpetialType).ToList();
                var damageTypeInjuryList = _rolesVm.InjuryVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType != (int)IncreType.SpetialType).ToList();
                var spDamageTypeVoluneraList = _rolesVm.VulnerabVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType == (int)IncreType.SpetialType).ToList();
                var damageTypeVoluneraList = _rolesVm.VulnerabVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType != (int)IncreType.SpetialType).ToList();


                List<Tuple<List<string>, int>> tuples = new List<Tuple<List<string>, int>>();
                foreach (var monster in _monsters)
                {
                    List<string> resultStrList = new List<string>();
                    resultStrList.Add($"怪物等级：{monster.Level}防御：{monster.Def}血量：{monster.Blood}");
                    resultStrList.Add($"基础攻击力{Atk.baseAtk}");
                    resultStrList.Add($"局内攻击力{Atk.gameAtk}");
                    int allDamage = 0;
                    resultStrList.Add(PrintWeaponInfo(entryList));
                    if (int.Parse(monster.Def) <= 0) continue;
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
                        // 减防区
                        decimal defDown = CountDeDef(monster);

                        // 暴击区
                        decimal criticalBonus = CountCratical(item, spDamageTypeInjuryList, spDamageTypeVoluneraList);

                        // 技能加成倍率
                        if (item.AtkType == AtkType.Profession || item.AtkType == AtkType.Stunk)
                        {
                            var increBonus = (1 + GetCompairedBonus(spDamageTypeInjuryList, (int)SpecialDamage.SkillInjury) / (decimal)100) * (1 + GetCompairedBonus(spDamageTypeVoluneraList, (int)SpecialDamage.SkillInjury) / (decimal)100);
                            LoggerHelper.Logger.Info($"技能加成倍率系数：{increBonus}");
                            criticalBonus *= increBonus;
                        }
                        var incre = CountInjureVulIncre(item, damageTypeInjuryList, damageTypeVoluneraList);
                        criticalBonus *= incre;
                        // 最终计算
                        CountSingleDamage(resultStrList, item, Atk.gameAtk, defDown, monster, criticalBonus, ref allDamage);
                    }
                    var finalResult = $"总计:{allDamage}";
                    LoggerHelper.Logger.Info(finalResult);
                    resultStrList.Add(finalResult);
                    tuples.Add(Tuple.Create(resultStrList, allDamage));
                }
                return tuples;
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message);
                return null;
            }
        }

        private void ClearAllWeaponList()
        {
            wAtkIncres.Clear();
            wDefIncres.Clear();
            wCraIncres.Clear();
            wAllHurtList.Clear();
            wAtkTypeList.Clear();
            wDamageTypeList.Clear();
            wTargetTypeList.Clear();
            wAllVulList.Clear();
            wAtkTypeVulList.Clear();
            wDamageTypeVulList.Clear();
            wTargetTypeVulList.Clear();

        }
        private List<ShowIncreInfo> TranseferIncreInfo(IncreInfo[] increInfos)
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
                        wAtkIncres.Add(showIncreInfo); break;
                    case IncreClass.DeDef:
                        wDefIncres.Add(showIncreInfo); break;
                    case IncreClass.Critical:
                        wCraIncres.Add(showIncreInfo); break;
                    default: break;
                }
            }
            var increList = weaponIncres.Where(i => i.SelectedIncre.IncreClass.Equals((int)IncreClass.DamageIncre)).ToList();
            foreach (var incre in increList)
            {
                switch ((IncreType)incre.SelectedIncre.IncreType)
                {
                    case IncreType.Percent:
                        wAllHurtList.Add(incre); break;
                    case IncreType.Number:
                        wAtkTypeList.Add(incre); break;
                    case IncreType.DamageType:
                        wDamageTypeList.Add(incre); break;
                    case IncreType.TargetType:
                        wTargetTypeList.Add(incre); break;
                    default: break;
                }
            }
            var vlunList = weaponIncres.Where(i => i.SelectedIncre.IncreClass.Equals((int)IncreClass.WeakIncre)).ToList();
            foreach (var vlun in vlunList)
            {
                switch ((IncreType)vlun.SelectedIncre.IncreType)
                {
                    case IncreType.Percent:
                        wAllVulList.Add(vlun); break;
                    case IncreType.Number:
                        wAtkTypeVulList.Add(vlun); break;
                    case IncreType.DamageType:
                        wDamageTypeVulList.Add(vlun); break;
                    case IncreType.TargetType:
                        wTargetTypeVulList.Add(vlun); break;
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
        // 不比较，直接累计列表所有加成数值
        public int GetListBonus(List<ShowIncreInfo> AtkBonusPerList)
        {
            var value = 0;
            foreach (ShowIncreInfo info in AtkBonusPerList)
            {
                if (string.IsNullOrEmpty(info.IncreNum)) continue;
                value += int.Parse(info.IncreNum);
            }
            return value;
        }
        // 比较类型后获取加成数值
        public int GetCompairedBonus(List<ShowIncreInfo> AtkBonusList, int type)
        {
            var list = AtkBonusList.Where(i => i.SelectedIncre.IncreDetail.Equals(type));
            var value = 0;
            foreach (ShowIncreInfo info in list)
            {
                if (string.IsNullOrEmpty(info.IncreNum)) continue;
                value += int.Parse(info.IncreNum);
            }
            return value;
        }
        #endregion
        #region count func
        private (decimal baseAtk, decimal gameAtk) GetGameAtk(IncreInfo[] entryList)
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
            decimal gameAtk = baseAtk * (1 + GetListBonus(_rolesVm.AtkBonusPerList.ToList()) / (decimal)100) + GetListBonus(_rolesVm.AtkBonusList.ToList());
            string atkLog = $"局内攻击力:{gameAtk}";
            LoggerHelper.Logger.Info(atkLog);
            return (baseAtk, gameAtk);
        }
        private string PrintWeaponInfo(IncreInfo[] entryList = null)
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
            ClearAllWeaponList();
            var singleResult = ComposeResult(monster, item, (int)result);
            LoggerHelper.Logger.Info($"本次结果：{singleResult}");
            resultStrList.Add(singleResult);
        }
        private decimal CountDeDef(Monster monster)
        {
            decimal defDown = 0;
            if (_rolesVm.CurrentRole.SelectedRole.Post.Equals((int)Occupation.Solder) || _rolesVm.CurrentRole.SelectedRole.Post.Equals((int)Occupation.Tank))
            {

                // 贯穿概率
                var throughout = GetListBonus(_rolesVm.DefPeneList.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOut)).ToList());
                var weaponThout = GetListBonus(wDefIncres.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOut)).ToList());
                throughout += weaponThout;
                // 贯穿强度
                var throughoutNum = 70 + GetListBonus(_rolesVm.DefPeneList.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOutNum)).ToList());
                var weaponThoutNum = GetListBonus(wDefIncres.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOutNum)).ToList());
                throughoutNum += weaponThoutNum;
                if (throughout > 100) throughout = 100;
                var defbonu = 1 - throughoutNum / (decimal)100 * throughout / (decimal)100;
                var defConstant = int.Parse(monster.Def) * defbonu / (decimal)(int.Parse(monster.Def) * defbonu + 400 + 3 * int.Parse(monster.Level));
                defDown = 1 - defConstant;
            }
            else
            {
                var defConstant = int.Parse(monster.Def) / (decimal)(int.Parse(monster.Def) + 400 + 3 * int.Parse(monster.Level));
                var countDown = GetListBonus(_rolesVm.DefPeneList.ToList());
                var weaponCountDown = GetListBonus(wDefIncres);

                var fullDown = 1 - (countDown + weaponCountDown) / (decimal)100;

                defDown = 1 - fullDown * defConstant;
            }
            LoggerHelper.Logger.Info($"减防区系数：{defDown}");
            return defDown;
        }

        private decimal CountCratical(SkillItem item, List<ShowIncreInfo> spDamageTypeInjuryList, List<ShowIncreInfo> spDamageTypeVoluneraList)
        {
            decimal criticalBonus = 1;
            if (item.DamageType != DamageType.Authentic && item.AtkType != AtkType.Extra && item.AtkType != AtkType.Contential)
            {
                // 暴击率
                var cBonus = GetListBonus(_rolesVm.CraticalBonusList.ToList()) / (decimal)100;
                var wpBonus = GetListBonus(wCraIncres.Where(i => i.SelectedIncre.IncreType.Equals((int)IncreType.Percent)).ToList()) / (decimal)100;
                cBonus += wpBonus;
                LoggerHelper.Logger.Info($"暴击率加成：{cBonus}");
                if (cBonus > 1) cBonus = 1;
                var weaponCHurt = GetListBonus(wCraIncres.Where(i => i.SelectedIncre.IncreType.Equals((int)IncreType.Number)).ToList());
                weaponCHurt += GetListBonus(_rolesVm.CraticalHurtList.ToList());
                LoggerHelper.Logger.Info($"暴击伤害加成：{weaponCHurt}");
                criticalBonus = (150 + weaponCHurt) / (decimal)100 * cBonus;
                // 暴击增伤/易伤区
                var increBonus = (1 + GetCompairedBonus(spDamageTypeInjuryList, (int)SpecialDamage.Critical) / (decimal)100) * (1 + GetCompairedBonus(spDamageTypeVoluneraList, (int)SpecialDamage.Critical) / (decimal)100);
                LoggerHelper.Logger.Info($"暴击增伤易伤系数：{increBonus}");
                criticalBonus *= increBonus;
            }
            return criticalBonus;
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
            allDamage += (int)result;
            ClearAllWeaponList();
            var singleResult = ComposeResult(monster, item, (int)result);
            LoggerHelper.Logger.Info($"本次结果：{singleResult}");
            if (WeaponEntryList.Count() > 0) return;
            resultStrList.Add(singleResult);

        }
        // 计算增伤*易伤乘区
        private decimal CountInjureVulIncre(SkillItem item, List<ShowIncreInfo> damageTypeInjuryList, List<ShowIncreInfo> damageTypeVoluneraList)
        {
            DamageType dameType = item.AtkType != AtkType.Contential ? item.DamageType : DamageType.Magic;
            // 增伤区：全增伤*攻击类型增伤*伤害类型增伤*目标类型增伤
            decimal injury = ((100 + GetListBonus(_rolesVm.InjuryVm.AllHurtList.ToList()) + GetListBonus(wAllHurtList)) / (decimal)100)
                * ((100 + GetCompairedBonus(_rolesVm.InjuryVm.AtkTypeList.ToList(), (int)item.AtkType) + GetCompairedBonus(wAtkTypeList, (int)item.AtkType)) / (decimal)100)
                * ((100 + GetCompairedBonus(damageTypeInjuryList, (int)item.DamageType) + GetCompairedBonus(wDamageTypeList, (int)item.DamageType)) / (decimal)100);
            LoggerHelper.Logger.Info($"增伤区系数：{injury}");

            // 易伤区：全易伤*攻击类型易伤*伤害类型易伤*目标类型易伤
            decimal vulnerab = (100 + GetListBonus(_rolesVm.VulnerabVm.AllHurtList.ToList()) + GetListBonus(wAllVulList)) / (decimal)100
                * (100 + GetCompairedBonus(_rolesVm.VulnerabVm.AtkTypeList.ToList(), (int)item.AtkType) + GetCompairedBonus(wAtkTypeVulList, (int)item.AtkType)) / (decimal)100
                * (100 + GetCompairedBonus(damageTypeVoluneraList, (int)item.DamageType) + GetCompairedBonus(wDamageTypeVulList, (int)item.DamageType)) / (decimal)100;
            LoggerHelper.Logger.Info($"易伤区系数：{vulnerab}");

            return injury * vulnerab;
        }
        #endregion
    }
}
