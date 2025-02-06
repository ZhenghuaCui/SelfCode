using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp3.Common;
using WpfApp3.Data;

namespace WpfApp3.ViewModels
{
    /// <summary>
    /// 增伤易伤公用VM
    /// </summary>
    public class DamageBonusViewModel : BindableBase
	{
		#region list
		// 全增伤区list
		public ObservableCollection<ShowIncreInfo> AllHurtList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		// 攻击类型list
		public ObservableCollection<ShowIncreInfo> AtkTypeList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		// 伤害类型list
		public ObservableCollection<ShowIncreInfo> DamageTypeList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		// 目标类型list
		public ObservableCollection<ShowIncreInfo> TargetTypeList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		#endregion
		public double GetAllBonus()
		{
			double bonus = (1 + GetSum(AllHurtList) / 100) * (1 + GetSum(AtkTypeList) / 100) * (1 + GetSum(DamageTypeList) / 100) * (1 + GetSum(TargetTypeList) / 100);
			return bonus;
		}
		public int GetSum(ObservableCollection<ShowIncreInfo> ShowIncreInfos)
		{
			int bonus = 0;
			foreach(ShowIncreInfo ShowIncreInfo in ShowIncreInfos)
			{
				bonus += int.Parse(ShowIncreInfo.IncreNum);
			}
			return bonus;
		}
		public void ClearAllList()
		{
			AllHurtList.Clear();
			AtkTypeList.Clear();
			DamageTypeList.Clear();
			TargetTypeList.Clear();
		}
		public void LoadMsg(List<ShowIncreInfo> fullList, IncreClass increArea)
		{
			AllHurtList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)increArea && i.SelectedIncre.IncreType == (int)IncreType.Percent));
			AtkTypeList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)increArea && i.SelectedIncre.IncreType == (int)IncreType.Number));
			DamageTypeList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)increArea && i.SelectedIncre.IncreType == (int)IncreType.DamageType));
			DamageTypeList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)increArea && i.SelectedIncre.IncreType == (int)IncreType.SpetialType));
			TargetTypeList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)increArea && i.SelectedIncre.IncreType == (int)IncreType.TargetType));
		}
	}
}
