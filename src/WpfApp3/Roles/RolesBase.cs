using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp3.Common;
using WpfApp3.Data;
using WpfApp3.Internfaces;
using WpfApp3.ViewModels;
using Wuhua.Main.Data;
using Wuhua.Model;

namespace WpfApp3.Roles
{
    public class RolesBase : IRoles
	{
		public RolesInfoControlViewModel _rolesVm { get; set; }
		public List<SkillItem> _skillList { get; set; }=new List<SkillItem>();
		public List<Monster> _monsters { get; set; }
		// 结果string 容器
		public ObservableCollection<string> ResultList;
		// 词条容器
		public List<IncreInfo> WeaponEntryList;

        public virtual IEnumerable<string> GetOutPut()
		{
			return ResultList.ToList();
		}

		public void Register(RolesInfoControlViewModel vm, List<Monster> monsters, ObservableCollection<string> resultList,List<IncreInfo> weaponEntryList)
		{
			_rolesVm = vm;
			_monsters = monsters;
			ResultList = resultList;
            WeaponEntryList = weaponEntryList;

            List<SkillItem> meltList = new List<SkillItem>();
            List<SkillItem> lostBloodList = new List<SkillItem>();
            List<SkillItem> burnList = new List<SkillItem>();

            foreach (var item in _rolesVm.SkillMultiList)
			{
				if (item.DamageType == DamageType.Melt)
				{
                    meltList.Add(item);
                }
				else if (item.DamageType == DamageType.LostBlood)
				{
                    lostBloodList.Add(item);
                }
                else if (item.DamageType == DamageType.Burn)
                {
                    burnList.Add(item);
                }
                else
				{
                    _skillList.Add(item);
                }
			}
            if (meltList.Count > 0)

                _skillList.Add(GetSkillPlused(meltList));
            if (burnList.Count>0)
				_skillList.Add(GetSkillPlused(burnList));
            if (lostBloodList.Count > 0)
                _skillList.Add(GetSkillPlused(lostBloodList));
        }
		
		// 将持续伤害进行累计
		private SkillItem GetSkillPlused(List<SkillItem> meltList)
		{
			SkillItem skillItem = null;
			foreach (var item in meltList) {
				if (skillItem == null)
				{
					skillItem = item;
				}
				else
				{
                    int result = int.Parse(skillItem.SkillNum) + int.Parse(item.SkillNum);
					skillItem.SkillNum = result.ToString();

                    result = int.Parse(skillItem.DamageTimes) + int.Parse(item.DamageTimes);
                    skillItem.DamageTimes = result.ToString();
                }
			}
			return skillItem;

        }
	}
}
