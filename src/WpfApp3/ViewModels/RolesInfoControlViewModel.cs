using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using WpfApp3.Common;
using WpfApp3.Data;
using WpfApp3.Events;
using Wuhua.IService;
using Wuhua.NLog;

namespace WpfApp3.ViewModels
{
    public class RolesInfoControlViewModel:BindableBase
	{
		#region list

		// 攻击数值list
		public ObservableCollection<ShowIncreInfo> AtkBonusList { get; private set; } = new ObservableCollection<ShowIncreInfo>();
		// 攻击加成list
		public ObservableCollection<ShowIncreInfo> AtkBonusPerList { get; private set; } = new ObservableCollection<ShowIncreInfo>();
		// 技能倍率list
		public ObservableCollection<SkillItem> SkillMultiList { get; private set; } = new ObservableCollection<SkillItem>();
		// 穿防加成list
		public ObservableCollection<ShowIncreInfo> DefPeneList { get; private set; } = new ObservableCollection<ShowIncreInfo>();
		// 暴击率list
		public ObservableCollection<ShowIncreInfo> CraticalBonusList { get; private set; } = new ObservableCollection<ShowIncreInfo>();
		// 暴击伤害list
		public ObservableCollection<ShowIncreInfo> CraticalHurtList { get; private set; } = new ObservableCollection<ShowIncreInfo>();

		#endregion
		#region properties
		private Role role;
		public Role Role
		{
			get { return role; }
			set
			{
				SetProperty(ref role, value);
			}
		}
		private int roleAtk;
		public int RoleAtk
		{
			get { return roleAtk; }
			set
			{
				SetProperty(ref roleAtk, value);
			}
		}
		public TeamDisplay CurrentRole { get; set; }

		IEventAggregator _aggregator;
		IContainerProvider _containerProvider;
		CommonSource _commonSource;
		Dictionary<RoleProEnm, string> roleProDic = CommonStaticSource.RoleProDic;
		DamageBonusViewModel injuryVm;
		public DamageBonusViewModel InjuryVm => injuryVm;
		DamageBonusViewModel vulnerabVm;
		public DamageBonusViewModel VulnerabVm => vulnerabVm;
		private IDeepInfoService _deepInfoService;
        private ILearnInfoService _learnInfoService;
        #endregion
        public RolesInfoControlViewModel(IContainerProvider containerProvider,IEventAggregator aggregator,
										IDeepInfoService deepInfoService,ILearnInfoService learnInfoService)
		{
			_containerProvider = containerProvider;
            _deepInfoService = deepInfoService;
			_learnInfoService = learnInfoService;	
            Role = new Role()
			{
				BaseAtk = 2274,
				CoreAtkPer = 30,
				DeepAtkPer = 10,
				WeaponAtkPer = 30,
				LifeAtkPer = 14,
				LearnAtkPer = 10,
				DeepAtk = 273,
				LifeAtk = 136,
				CommonLearnAtk = 540,
				WeaponAtk = 1469
			};
			_aggregator = aggregator;
			_aggregator.GetEvent<RoleSelectedEvent>()?.Subscribe(ReceivedRolesExecute);
			_commonSource = containerProvider.Resolve<CommonSource>("CommonSource");

			RoleAtk = CountRoleAtk(Role);
		}

		#region funcs
		private async void ReceivedRolesExecute(bool isIncludeWeapon)
		{
			Debug.WriteLine("receive execute");
			if (injuryVm == null || vulnerabVm == null)
			{
				injuryVm = _containerProvider.Resolve<DamageBonusViewModel>("IncreInjury");
				vulnerabVm = _containerProvider.Resolve<DamageBonusViewModel>("Vulnerabe");
			}
			try
			{
				var teamDisplay = _containerProvider.Resolve<ObservableCollection<TeamDisplay>>("TeamList");
				ClearAllList();
				foreach (var teamRole in teamDisplay)
				{
					if (teamRole == null || teamRole.SelectedRole == null) continue;
					if (teamRole.IsChecked && isIncludeWeapon && teamRole.SelectedWeapon == null) 
					{
						MessageBox.Show("请选择武器");
						return;
					}

					var efList = teamRole.SelectedRole.IncreEf.Split("%");
                    List<string> efStrlist = new List<string>();
                    efStrlist.AddRange(efList);
                    if (teamRole.IsChecked)
					{
						// 获取深造 共研加成
                        var deepInfo = await _deepInfoService.FindAsync(teamRole.SelectedRole.Deep);
                        var learnInfo = await _learnInfoService.QueryAsync(i => i.LearnType.Equals((int)LearnType.Learn));
                        var commonlearnInfo = await _learnInfoService.QueryAsync(i => i.LearnType.Equals((int)LearnType.CommmonLearn) && i.Post.Equals(teamRole.SelectedRole.Post));


                        efStrlist.AddRange(deepInfo.DeepEf.Split("%"));
                        efStrlist.AddRange(learnInfo.FirstOrDefault().LearnEf.Split("%"));
                        efStrlist.AddRange(commonlearnInfo.FirstOrDefault().LearnEf.Split("%"));

                    }
                    var fullList = new List<ShowIncreInfo>();
					foreach(var efStr in efStrlist)
					{
						fullList.AddRange(CommonStaticSource.DeSerializeIncreInfo(efStr));
					}
					
					if (teamRole.IsChecked && teamRole.SelectedRole.RoleType == (int)RoleTypeEnum.AD)
					{
						CurrentRole = teamRole;
						Role custom = new Role();
						custom.BaseAtk = teamRole.SelectedRole.BaseAtk;

						var baseList = fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.BaseIncre);

						// 基础百分比属性
						var basePerList = baseList.Where(i => i.SelectedIncre.IncreType == (int)IncreType.Percent);
						custom.CoreAtkPer = GetNum(baseList, RoleProEnm.CoreLearn);
						custom.DeepAtkPer = GetNum(basePerList, RoleProEnm.Deep); 


						custom.LifeAtkPer = GetNum(basePerList, RoleProEnm.Life);
						custom.LearnAtkPer = GetNum(basePerList, RoleProEnm.Learn);
						// 基础数值属性
						var baseNumList = baseList.Where(i => i.SelectedIncre.IncreType == (int)IncreType.Number);
						custom.DeepAtk = GetNum(baseNumList, RoleProEnm.Deep); 
						custom.LifeAtk = GetNum(baseNumList, RoleProEnm.Life); 
						custom.CommonLearnAtk = GetNum(baseNumList, RoleProEnm.CommonLearn);

                        //WeaponAtk = 1469
                        // 装备属性
                        custom.WeaponAtk = teamRole.SelectedWeapon.BaseAtk;
                        if (isIncludeWeapon  && teamRole.SelectedWeapon != null)
						{
							var weaponIncres = teamRole.SelectedWeapon.WeaponEfList;
							var weaponAtk = weaponIncres.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.BaseIncre && i.SelectedIncre.IncreType == (int)IncreType.Number);
							if (weaponAtk.Count() > 0)
							{
								custom.WeaponAtk += weaponAtk.First().SelectedIncre.IncreNum;
							}
							var weaponAtkPer = weaponIncres.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.BaseIncre && i.SelectedIncre.IncreType == (int)IncreType.Percent);
							if (weaponAtkPer.Count() > 0)
							{
								custom.WeaponAtkPer += weaponAtkPer.First().SelectedIncre.IncreNum;
							}

							fullList.AddRange(teamRole.SelectedWeapon.WeaponEfList);
						}
						Role = custom;
						RoleAtk = CountRoleAtk(Role);

						// 技能加成
						var skillEfList = teamRole.SelectedRole.SkillEf.Split("%");
						foreach (var skillEf in skillEfList)
						{
							SkillMultiList.AddRange(CommonStaticSource.DeSerializeSkillInfo(skillEf));
						}
					}
					AtkBonusPerList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.AtkIncre && i.SelectedIncre.IncreType == (int)IncreType.Percent));

					AtkBonusList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.AtkIncre && i.SelectedIncre.IncreType == (int)IncreType.Number));

					//SkillMultiList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreArea.Skill && i.SelectedIncre.IncreType == (int)IncreType.Percent));

					DefPeneList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.DeDef && i.SelectedIncre.IncreType == (int)IncreType.Percent));

					CraticalBonusList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.Critical && i.SelectedIncre.IncreType == (int)IncreType.Percent));

					CraticalHurtList.AddRange(fullList.Where(i => i.SelectedIncre.IncreClass == (int)IncreClass.Critical && i.SelectedIncre.IncreType == (int)IncreType.Number));

					vulnerabVm.LoadMsg(fullList, IncreClass.WeakIncre);
					injuryVm.LoadMsg(fullList, IncreClass.DamageIncre);
					_aggregator.GetEvent<SendInfoMessage>().Publish("器者配队加载成功");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("角色/武器信息异常");
				LoggerHelper.Logger.Error(ex.Message,ex);
			}
		}
		public int GetNum(IEnumerable<ShowIncreInfo> baseList, RoleProEnm roleProEnm)
		{
			var items = baseList.Where(i => i.Title == roleProDic[roleProEnm]);
			if (items == null || items.Count()==0 || items.FirstOrDefault().SelectedIncre == null) return 0;
			else
			{
				return items.FirstOrDefault().SelectedIncre.IncreNum;
			}
		}
		public int CountRoleAtk(Role role)
		{
			var basePro = role.CoreAtkPer + role.DeepAtkPer + role.LearnAtkPer + role.LifeAtkPer + role.WeaponAtkPer;
			var baseNum = role.WeaponAtk + role.LifeAtk + role.DeepAtk + role.CommonLearnAtk;
			var atk = role.BaseAtk * (1 + basePro / 100) + baseNum;
			return (int)atk;
		}

		public void ClearAllList()
		{
			AtkBonusList.Clear();
			AtkBonusPerList.Clear();
			SkillMultiList.Clear();
			DefPeneList.Clear();
			CraticalBonusList.Clear();
			CraticalHurtList.Clear();

			injuryVm.ClearAllList();
			vulnerabVm.ClearAllList();
		}
		public void Dispose()
		{
            _aggregator.GetEvent<RoleSelectedEvent>()?.Unsubscribe(ReceivedRolesExecute);
        }
		#endregion
	}
}
