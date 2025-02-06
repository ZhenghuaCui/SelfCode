using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WpfApp3.Common;
using WpfApp3.Data;
using WpfApp3.Events;
using Wuhua.IService;
using Wuhua.Model;
using Wuhua.NLog;

namespace WpfApp3.ViewModels
{
    public class AddRolesViewModel:BindableBase
	{
		#region list
		//角色列表
		public ObservableCollection<RoleInfo> RolesList { get; set; } = new ObservableCollection<RoleInfo>();

		//技能倍率列表
		public ObservableCollection<SkillItem> SkillList { get; set; } = new ObservableCollection<SkillItem>();

		//面板属性加成列表
		public ObservableCollection<ShowIncreInfo> RoleIncreList { get; private set; } = new ObservableCollection<ShowIncreInfo>();

		// 副词条加成List
		public ObservableCollection<IncreInfo> IncreProList { get; private set; } = new ObservableCollection<IncreInfo>();

		//游戏内加成列表
		public ObservableCollection<ShowIncreInfo> GameIncreList { get; set; } = new ObservableCollection<ShowIncreInfo>();

		//深造列表
		
		public ObservableCollection<ShowDeepInfo> ShowDeepList { get; set; } = new ObservableCollection<ShowDeepInfo>();

		//深造加成列表
		public ObservableCollection<ShowIncreInfo> CurrentDeepIncreList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		// 研学 共研加成列表
		public ObservableCollection<ShowIncreInfo> CurrentLearnIncreList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		private ObservableCollection<IncreInfo> baseIncreList;

		private ObservableCollection<IncreInfo> gameIncreList;
		public Dictionary<Occupation,string> OccupationList { get; set; }
		public Dictionary<RoleTypeEnum, string> RoleTypeList { get; set; }

		public Dictionary<AtkType, string> AtkTypeList { get; set; }
		public Dictionary<DamageType, string> DamageTypeList { get; set; }
		#endregion
		#region commmand
		public DelegateCommand<Window> SaveRoleCommmand { get; set; }
		public DelegateCommand<Window> CancelCommand { get; set; }
		public DelegateCommand<Window> AddIncreItemCommand { get; set; }
		public DelegateCommand<ShowDeepInfo> DeleDeepCommand { get; set; }
        public DelegateCommand<Window> AddSkillItemCommand { get; set; }
        

        #endregion
        #region properties
        private RoleInfo _newRole = new RoleInfo();
		public RoleInfo NewRole
		{
			get => _newRole;
			set
			{
				SetProperty(ref _newRole, value);
			}
		}

		// 角色类型
		private RoleTypeEnum _selectedRoleType;
		public RoleTypeEnum SelectedRoleType
		{
			get => _selectedRoleType;
			set
			{
				SetProperty(ref _selectedRoleType, value);
			}
		}

		//角色职业
		private Occupation _selectedOccupation;
		public Occupation SelectedOccupation
		{
			get => _selectedOccupation;
			set
			{
				SetProperty(ref _selectedOccupation, value);
				InitDeepList();
				InitLearnInfo();
			}
		}
		//角色深造
		private ShowDeepInfo _selectedDeep;
		public ShowDeepInfo SelectedDeep
		{
			get => _selectedDeep;
			set
			{
				SetProperty(ref _selectedDeep, value);
				ResetCurrentList(CurrentDeepIncreList);
				InitDeep();
			}
		}



		//编辑的角色
		private RoleInfo editRole;
		public RoleInfo EditRole
		{
			get => editRole;
			set
			{
				SetProperty(ref editRole, value);
				LoadRole();
			}
		}
		private Visibility isEditShow;
		public Visibility IsEditShow
		{
			get { return isEditShow; }
			set
			{
				SetProperty(ref isEditShow, value);
			}
		}

		//角色职业Enum list
		Dictionary<RoleProEnm, string> roleProDic = CommonStaticSource.RoleProDic;
		CommonSource _commonSource;
		IRoleInfoService _roleInfoService;
		IDeepInfoService _deepInfoService;
		ILearnInfoService _learnInfoService;
		#endregion

		#region Event
		IEventAggregator _aggregator;
		#endregion
		public AddRolesViewModel(IContainerProvider containerProvider, IEventAggregator aggregator,IRoleInfoService roleInfoService,IDeepInfoService deepInfoService,ILearnInfoService learnInfoService)
		{
			
			_commonSource = containerProvider.Resolve<CommonSource>("CommonSource");
			aggregator.GetEvent<ManageRolesEvent>().Subscribe(ManageRolesExecute);
			IncreProList.AddRange(_commonSource.IncreInfos);
			_roleInfoService = roleInfoService;
			_deepInfoService = deepInfoService;
			_learnInfoService = learnInfoService;
			_aggregator = aggregator;

			SaveRoleCommmand =  new DelegateCommand<Window>(SaveRoleExecute);
			CancelCommand = new DelegateCommand<Window>(CancelExecute);
			AddIncreItemCommand = new DelegateCommand<Window>(AddIncreItemExecute);
			DeleDeepCommand = new DelegateCommand<ShowDeepInfo>(DeleteDeepExecute);
			AddSkillItemCommand = new DelegateCommand<Window>(AddSkillItemExcute);

            Init();
		}
        #region command excute
        private void AddSkillItemExcute(Window window)
        {
            SkillList.Add(new SkillItem());
        }

        private async void DeleteDeepExecute(ShowDeepInfo showDeepInfo)
		{
			ShowDeepList.Remove(showDeepInfo);
			var result = _deepInfoService.DeleteAsync(showDeepInfo.DeepInfo.Id);
			
		}

		private void AddIncreItemExecute(Window obj)
		{
			GameIncreList.Add(new ShowIncreInfo() { IncreInfoList = gameIncreList });
		}

		private void ManageRolesExecute(bool isEdit)
		{
			if (isEdit)
			{
				InitEditRole();
				IsEditShow = Visibility.Visible;
			}
			else
			{
				IsEditShow = Visibility.Collapsed;
			}
            SelectedDeep = ShowDeepList.FirstOrDefault();
        }
        private void CancelExecute(Window window)
        {
            Dispose();
            if (window != null)
            {
                window.Close();
            }
        }
        private async void SaveRoleExecute(Window window)
        {
            try
            {
                if (string.IsNullOrEmpty(NewRole.RoleName))
                {
                    NewRole.RoleName = "未命名";
                }
                var findResult = await _roleInfoService.QueryAsync(i => i.RoleName.Equals(NewRole.RoleName));
                if (findResult != null && findResult.Count() > 0 && isEditShow != Visibility.Visible)
                {
                    var info = "已存在同名器者";
                    MessageBox.Show(info);
                    LoggerHelper.Logger.Info(info);
                    return;
                }
                // 清除深造内的内容
                ResetCurrentList(CurrentDeepIncreList);
                ResetCurrentList(CurrentLearnIncreList);
                NewRole.RoleType = (int)SelectedRoleType;
                NewRole.Post = (int)SelectedOccupation;
                NewRole.IncreEf = CommonStaticSource.SerializeIncreInfo(RoleIncreList.ToList());
                NewRole.IncreEf += "%" + CommonStaticSource.SerializeIncreInfo(GameIncreList.ToList());
                NewRole.SkillEf = CommonStaticSource.SerializeSkillInfo(SkillList.ToList());
                if (SelectedDeep != null)
                {
                    NewRole.Deep = SelectedDeep.DeepInfo.Id;
                }

                if (isEditShow == Visibility.Visible)
                {
                    await _roleInfoService.EditAsync(NewRole);
                    _aggregator.GetEvent<RoleEditedEvent>()?.Publish(true);
                }
                else
                {
                    await _roleInfoService.CreatAsync(NewRole);
                    _aggregator.GetEvent<RoleAddedEvent>()?.Publish(NewRole);
                }
                var SucInfo = "器者信息保存成功！";

                LoggerHelper.Logger.Info(SucInfo);
                _aggregator.GetEvent<SendInfoMessage>()?.Publish(SucInfo);
                Dispose();
                if (window != null)
                {
                    window.Close();
                }


            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
            }


        }
        #endregion
        #region
        public async void InitLearnInfo()
        {
            try
            {
                ResetCurrentList(CurrentLearnIncreList);
                var learnList = await _learnInfoService.QueryAsync(i => i.LearnType.Equals((int)LearnType.Learn) ||
                    (i.LearnType.Equals((int)LearnType.CommmonLearn) && i.Post.Equals((int)SelectedOccupation)));
                foreach (var item in learnList)
                {
                    var learnEf = item.LearnEf.Split("%");
                    if (!string.IsNullOrEmpty(learnEf[0]))
                    {
                        var learnInfos = CommonStaticSource.DeSerializeIncreInfo(learnEf[0], baseIncreList);
                        RoleIncreList.AddRange(learnInfos);
                        CurrentLearnIncreList.AddRange(learnInfos);
                    }
                    if (!string.IsNullOrEmpty(learnEf[1]))
                    {
                        var learnInfos = CommonStaticSource.DeSerializeIncreInfo(learnEf[1], gameIncreList);
                        GameIncreList.AddRange(learnInfos);
                        CurrentLearnIncreList.AddRange(learnInfos);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
            }
        }
        private async void InitDeepList()
        {
            try
            {
                ShowDeepList.Clear();
                var result = await _deepInfoService.QueryAsync(i => i.Post == (int)_selectedOccupation);
                foreach (var item in result)
                {
                    var deepEf = item.DeepEf.Split("%");
                    var newDeep = new ShowDeepInfo();
                    newDeep.DeepInfo = item;
                    if (!string.IsNullOrEmpty(deepEf[0]))
                    {
                        var deepInfos = CommonStaticSource.DeSerializeIncreInfo(deepEf[0], baseIncreList);
                        newDeep.DeepIncres.AddRange(deepInfos);
                    }
                    if (!string.IsNullOrEmpty(deepEf[1]))
                    {
                        var deepInfos = CommonStaticSource.DeSerializeIncreInfo(deepEf[1], gameIncreList);
                        newDeep.DeepIncres.AddRange(deepInfos);
                    }
                    ShowDeepList.Add(newDeep);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
            }

        }
        private async void Init()
        {
            OccupationList = CommonStaticSource.OccupationDic;
            RoleTypeList = CommonStaticSource.RoleTypeDic;
            AtkTypeList = CommonStaticSource.AtkTypeDic;
            DamageTypeList = CommonStaticSource.DamageTypeDic;
            InitNewRole();
            InitDeepList();
            InitLearnInfo();
            for (int i = 0; i < 2; i++)
            {
                SkillList.Add(new SkillItem());
            }
        }
        private void InitDeep()
        {
            try
            {
                if (SelectedDeep == null) return;
                var deepEf = SelectedDeep.DeepIncres.Where(i => i.SelectedIncre.IncreClass.Equals((int)IncreClass.BaseIncre));
                foreach (var item in deepEf)
                {
                    item.IncreInfoList = baseIncreList;
                    CurrentDeepIncreList.Add(item);
                    RoleIncreList.Add(item);
                }
                var deepGameEf = SelectedDeep.DeepIncres.Where(i => i.SelectedIncre.IncreClass != (int)IncreClass.BaseIncre);
                foreach (var item in deepGameEf)
                {
                    item.IncreInfoList = gameIncreList;
                    CurrentDeepIncreList.Add(item);
                    GameIncreList.Add(item);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
            }

        }
        private async void InitNewRole()
        {
            try
            {
                baseIncreList = new ObservableCollection<IncreInfo>();
                baseIncreList.AddRange(IncreProList.Where(i => i.IncreClass == (int)IncreClass.BaseIncre));


                RoleIncreList.Add(new ShowIncreInfo() { Title = roleProDic[RoleProEnm.Life], IncreInfoList = baseIncreList });
                RoleIncreList.Add(new ShowIncreInfo() { Title = roleProDic[RoleProEnm.Life], IncreInfoList = baseIncreList });
                var atkIncre = baseIncreList.FirstOrDefault(i => i.IncreType.Equals((int)IncreType.Percent));
                RoleIncreList.Add(new ShowIncreInfo()
                {
                    Title = roleProDic[RoleProEnm.CoreLearn],
                    SelectedIncre = atkIncre,
                    IncreInfoList = baseIncreList,
                    IncreNum = "30"
                });

                gameIncreList = new ObservableCollection<IncreInfo>();
                gameIncreList.AddRange(IncreProList.Where(i => i.IncreClass != (int)IncreClass.BaseIncre).ToList());
                for (int i = 0; i < 2; i++)
                {
                    GameIncreList.Add(new ShowIncreInfo() { IncreInfoList = gameIncreList });
                }
            }
            catch (Exception ex)
            {
                _aggregator.GetEvent<SendInfoMessage>()?.Publish("角色信息加载失败");
                LoggerHelper.Logger.Error(ex.Message, ex);
            }


        }

        private async void InitEditRole()
        {
            ObservableCollection<RoleInfo> tempList = new ObservableCollection<RoleInfo>();
            var roleQueryList = await _roleInfoService.QueryAsync();
            RolesList.AddRange(roleQueryList);
        }
        #endregion
        #region func
        private void ResetCurrentList(ObservableCollection<ShowIncreInfo> increList)
		{
			foreach(var item in increList)
			{
				var roleList = RoleIncreList.Where(i => i.Title.Equals(item.Title) && i.IncreNum.Equals(item.IncreNum)
														&& i.SelectedIncre.GetHashCode().Equals(item.SelectedIncre.GetHashCode()));
				if (roleList.Count()>0)
				{
					RoleIncreList.Remove(roleList.FirstOrDefault());
				}
                var gameList = GameIncreList.Where(i => i.Title.Equals(item.Title) && i.IncreNum.Equals(item.IncreNum)
                                        && i.SelectedIncre.GetHashCode().Equals(item.SelectedIncre.GetHashCode()));
                if (gameList.Count() > 0)
                {
                    GameIncreList.Remove(gameList.FirstOrDefault());
                }
            }
            increList.Clear();
		}
		public void Dispose()
		{
            _aggregator.GetEvent<ManageRolesEvent>().Unsubscribe(ManageRolesExecute);
        }
		private async void LoadRole()
		{
			try
			{
                var role = await _roleInfoService.FindAsync(i => i.Id.Equals(editRole.Id));
                if (role == null)
                {
                    MessageBox.Show("未查询到器者信息");
					return;
                }
                NewRole = role;
                RoleIncreList.Clear();
                GameIncreList.Clear();
                SkillList.Clear();
                SelectedRoleType = (RoleTypeEnum)role.RoleType;
                SelectedOccupation = (Occupation)role.Post;
                var increList = role.IncreEf.Split("%");
                var baseIncreList = new ObservableCollection<IncreInfo>();
                baseIncreList.AddRange(IncreProList.Where(i => i.IncreClass == (int)IncreClass.BaseIncre));
                RoleIncreList.AddRange(CommonStaticSource.DeSerializeIncreInfo(increList[0], baseIncreList));

                var gameIncreList = new ObservableCollection<IncreInfo>();
                gameIncreList.AddRange(IncreProList.Where(i => i.IncreClass != (int)IncreClass.BaseIncre).ToList());
                GameIncreList.AddRange(CommonStaticSource.DeSerializeIncreInfo(increList[1], gameIncreList));
                SkillList.AddRange(CommonStaticSource.DeSerializeSkillInfo(role.SkillEf));
				SelectedDeep =  ShowDeepList.FirstOrDefault(i => i.DeepInfo.Id.Equals(role.Deep));
            }
            catch (Exception ex)
            {
                _aggregator.GetEvent<SendInfoMessage>()?.Publish("角色信息加载失败");
                LoggerHelper.Logger.Error(ex.Message, ex);
            }

		}
        #endregion





    }
}
