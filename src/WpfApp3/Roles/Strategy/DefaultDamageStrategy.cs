using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using WpfApp3.Common;
using WpfApp3.Data;
using WpfApp3.ViewModels;
using Wuhua.Main.Common.Extensions;
using Wuhua.Main.Data;
using Wuhua.NLog;

namespace Wuhua.Main.Roles.Strategy
{
    public class DefaultDamageStrategy : IDamageStrategy
    {
        public const int BaseCriticalDamage = 150;
        public const decimal BaseBonus = 1;
        private static readonly ConcurrentDictionary<string, int> BonusCache = new ConcurrentDictionary<string, int>();
        RolesInfoControlViewModel _rolesVm;
        WeaponBuffGroups _weaponBuffGroups;
        IEnumerable<ShowIncreInfo> spDamageTypeInjuryList;
        IEnumerable<ShowIncreInfo> damageTypeInjuryList;
        IEnumerable<ShowIncreInfo> spDamageTypeVoluneraList;
        IEnumerable<ShowIncreInfo> damageTypeVoluneraList;

        public DefaultDamageStrategy(RolesInfoControlViewModel rolesVm,WeaponBuffGroups weaponBuffGroups)
        {
            _rolesVm = rolesVm;
            _weaponBuffGroups = weaponBuffGroups;
            //分离特殊伤害列表,如暴击伤害增伤
            spDamageTypeInjuryList = _rolesVm.InjuryVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType == (int)IncreType.SpetialType);
            damageTypeInjuryList = _rolesVm.InjuryVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType != (int)IncreType.SpetialType);
            spDamageTypeVoluneraList = _rolesVm.VulnerabVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType == (int)IncreType.SpetialType);
            damageTypeVoluneraList = _rolesVm.VulnerabVm.DamageTypeList.Where(i => i.SelectedIncre.IncreType != (int)IncreType.SpetialType);
        }
        // 计算暴击
        public decimal CalculateCriticalBonus(SkillItem item)
        {
            decimal criticalBonus = 1;
            if (item.DamageType != DamageType.Authentic && item.AtkType != AtkType.Extra && item.AtkType != AtkType.Contential)
            {
                // 暴击率
                var cBonus = _rolesVm.CraticalBonusList.SumNumericalValues() / (decimal)100;
                var wpBonus = _weaponBuffGroups.CraIncres.Where(i => i.SelectedIncre.IncreType.Equals((int)IncreType.Percent)).SumNumericalValues() / (decimal)100;
                cBonus += wpBonus;
                LoggerHelper.Logger.Info($"暴击率加成：{cBonus}");
                if (cBonus > BaseBonus) cBonus = BaseBonus;
                var weaponCHurt = _weaponBuffGroups.CraIncres.Where(i => i.SelectedIncre.IncreType.Equals((int)IncreType.Number)).SumNumericalValues();
                weaponCHurt += _rolesVm.CraticalHurtList.SumNumericalValues();
                LoggerHelper.Logger.Info($"暴击伤害加成：{weaponCHurt}");
                criticalBonus = (BaseCriticalDamage + weaponCHurt) / (decimal)100 ;
                // 暴击增伤/易伤区
                var increBonus = (1 + GetCompairedBonus(spDamageTypeInjuryList, (int)SpecialDamage.Critical, true) / (decimal)100) * (1 + GetCompairedBonus(spDamageTypeVoluneraList, (int)SpecialDamage.Critical, true) / (decimal)100);
                LoggerHelper.Logger.Info($"暴击增伤易伤系数：{increBonus}");
                criticalBonus *= increBonus;

                //期望计算
                criticalBonus = (decimal)((1-cBonus)+cBonus*criticalBonus);
            }
            return criticalBonus;
        }
        // 计算减防
        public decimal CalculateDefenceReduction(Monster monster)
        {
            decimal defDown = 0;
            // 防御降低效果
            var rolesDeDef = _rolesVm.DefPeneList.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.DefDown)).SumNumericalValues();
            var weaponDeDef = _weaponBuffGroups.DefIncres.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.DefDown)).SumNumericalValues();
            var deDef = rolesDeDef + weaponDeDef;
            int monsterDef =int.Parse(monster.Def) - (int)(int.Parse(monster.Def) * deDef/(decimal)100);
            if(monsterDef < 0) monsterDef = 0;
            if (_rolesVm.CurrentRole.SelectedRole.Post.Equals((int)Occupation.Solder) || _rolesVm.CurrentRole.SelectedRole.Post.Equals((int)Occupation.Tank))
            {
               
                // 贯穿概率
                var throughout = _rolesVm.DefPeneList.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOut)).SumNumericalValues();
                var weaponThout = _weaponBuffGroups.DefIncres.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOut)).SumNumericalValues();
                throughout += weaponThout;
                // 贯穿强度
                var throughoutNum = 70 + _rolesVm.DefPeneList.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOutNum)).SumNumericalValues();
                var weaponThoutNum = _weaponBuffGroups.DefIncres.Where(i => i.SelectedIncre.IncreDetail.Equals((int)DefArea.ThroughOutNum)).SumNumericalValues();
                throughoutNum += weaponThoutNum;
                if (throughout > 100) throughout = 100;
                if (throughoutNum > 100) throughoutNum = 100;
                //贯穿率 贯穿强度收益
                var defbonu =  (1- throughout / (decimal)100) +(1- throughoutNum / (decimal)100)* throughout / (decimal)100;
                var defConstant = monsterDef * defbonu / ((decimal)monsterDef * defbonu + 400 + 3 * int.Parse(monster.Level));
                defDown = 1 - defConstant;
            }
            else
            {
                var defConstant = monsterDef / (decimal)(monsterDef + 400 + 3 * int.Parse(monster.Level));
                var countDown = _rolesVm.DefPeneList.SumNumericalValues();
                var weaponCountDown = _weaponBuffGroups.DefIncres.SumNumericalValues();

                var fullDown = 1 - (countDown + weaponCountDown) / (decimal)100;

                defDown = 1 - fullDown * defConstant;
            }
            LoggerHelper.Logger.Info($"减防区系数：{defDown}");
            return defDown;
        }
        // 计算增伤*易伤乘区
        public decimal CalculateInjuryMultiplier(SkillItem item)
        {
            DamageType dameType = item.AtkType != AtkType.Contential ? item.DamageType : DamageType.Magic;
            // 增伤区：全增伤*攻击类型增伤*伤害类型增伤*目标类型增伤
            decimal fullIncre = (100 + _rolesVm.InjuryVm.AllHurtList.SumNumericalValues() + _weaponBuffGroups.AllHurtList.SumNumericalValues()) / (decimal)100;
            decimal atkTypeIncre = (100 + GetCompairedBonus(_rolesVm.InjuryVm.AtkTypeList.ToList(), (int)item.AtkType) + GetCompairedBonus(_weaponBuffGroups.AtkTypeList, (int)item.AtkType, true)) / (decimal)100;
            //对持续伤害进行增伤判定
            DamageType damageType = TransContentialDamageType(item);

            decimal damageTypeIncre = (100 + GetCompairedBonus(damageTypeInjuryList, (int)damageType) + GetCompairedBonus(_weaponBuffGroups.DamageTypeList, (int)damageType, true)) / (decimal)100;
            decimal injury = fullIncre * atkTypeIncre * damageTypeIncre;
            //Debug.WriteLine($"full:{fullIncre},atkType:{atkTypeIncre},damageType{damageTypeIncre}");

            LoggerHelper.Logger.Info($"增伤区系数：{injury}");

            // 易伤区：全易伤*攻击类型易伤*伤害类型易伤*目标类型易伤
            decimal vulnerab = (100 + _rolesVm.VulnerabVm.AllHurtList.SumNumericalValues() + _weaponBuffGroups.AllVulList.SumNumericalValues()) / (decimal)100
                * (100 + GetCompairedBonus(_rolesVm.VulnerabVm.AtkTypeList.ToList(), (int)item.AtkType) + GetCompairedBonus(_weaponBuffGroups.AtkTypeVulList, (int)item.AtkType, true)) / (decimal)100
                * (100 + GetCompairedBonus(damageTypeVoluneraList, (int)damageType) + GetCompairedBonus(_weaponBuffGroups.DamageTypeVulList, (int)damageType, true)) / (decimal)100;
            LoggerHelper.Logger.Info($"易伤区系数：{vulnerab}");

            return injury * vulnerab;
        }

        private static DamageType TransContentialDamageType(SkillItem item)
        {
            var damageType = item.DamageType;
            if (item.DamageType == DamageType.Burn || item.DamageType == DamageType.Melt)
                damageType = DamageType.Magic;
            else if (item.DamageType == DamageType.LostBlood)
                damageType = DamageType.Physical;
            return damageType;
        }

        public int GetCompairedBonus(IEnumerable<ShowIncreInfo> AtkBonusList, int type,bool isWeapon= false)
        {
            if (isWeapon)
            {
                return AtkBonusList
                .Where(i => i.SelectedIncre.IncreDetail == type)
                .Sum(i => string.IsNullOrEmpty(i.IncreNum) ? 0 : int.Parse(i.IncreNum));
            }
            else
            {
                var cacheKey = $"{AtkBonusList.GetHashCode()}-{type}";
                return BonusCache.GetOrAdd(cacheKey, _ =>
                    AtkBonusList
                    .Where(i => i.SelectedIncre.IncreDetail == type)
                    .Sum(i => string.IsNullOrEmpty(i.IncreNum) ? 0 : int.Parse(i.IncreNum)));



            }
        }
        public void ClearCache()
        {

        }
        // 计算技能增伤
        public decimal CalculateSkill(SkillItem item) {
            // 技能加成倍率
            if (item.AtkType == AtkType.Profession || item.AtkType == AtkType.Stunk)
            {
                var increBonus = (1 + GetCompairedBonus(spDamageTypeInjuryList, (int)SpecialDamage.SkillInjury) / (decimal)100) * (1 + GetCompairedBonus(spDamageTypeVoluneraList, (int)SpecialDamage.SkillInjury) / (decimal)100);
                LoggerHelper.Logger.Info($"技能加成倍率系数：{increBonus}");
                return increBonus;
            }
            return BaseBonus;
        }
    }
}
