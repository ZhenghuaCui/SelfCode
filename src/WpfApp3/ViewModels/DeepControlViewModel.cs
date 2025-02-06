using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
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
	/// <summary>
	/// 深造页面
	/// </summary>
    public class DeepControlViewModel:BindableBase
	{
		private string deepName = "";
		public string DeepName
		{
			get => deepName;
			set
			{
				SetProperty(ref deepName, value);
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
			}
		}

		#region commmand
		public DelegateCommand<Window> SaveDeepCommmand { get; set; }
		public DelegateCommand<Window> CancelCommand { get; set; }
		public DelegateCommand AddIncreItemCommand { get; set; }

		#endregion
		// 副词条加成List
		public ObservableCollection<IncreInfo> IncreProList { get; private set; } = new ObservableCollection<IncreInfo>();

		//游戏内加成列表
		public ObservableCollection<ShowIncreInfo> RoleIncreList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		//深造核心加成列表
		public ObservableCollection<ShowIncreInfo> CoreIncreList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		// 基础属性加成列表
		private ObservableCollection<IncreInfo> roleList = new ObservableCollection<IncreInfo>();
		public Dictionary<Occupation, string> OccupationList { get; set; }

		private CommonSource _commonSource;
		private IDeepInfoService _deepInfoService;
		private IEventAggregator _aggregator;

        public DeepControlViewModel(IContainerProvider containerProvider,IEventAggregator aggregator, IDeepInfoService deepInfoService)
		{
			_commonSource = containerProvider.Resolve<CommonSource>("CommonSource");
			_deepInfoService = deepInfoService;
			_aggregator = aggregator;


            IncreProList.AddRange(_commonSource.IncreInfos);

			SaveDeepCommmand = new DelegateCommand<Window>(SaveRoleExecute);
			CancelCommand = new DelegateCommand<Window>(CancelExecute);
			AddIncreItemCommand = new DelegateCommand(AddIncreItemExecute);
			initList();
		}

		private void initList()
		{
			try
			{
				OccupationList = CommonStaticSource.OccupationDic;
				var coreList = new ObservableCollection<IncreInfo>();
				coreList.AddRange(IncreProList.Where(i => i.IncreClass != (int)IncreClass.BaseIncre).ToList());
				for (int i = 0; i < 2; i++)
				{
					CoreIncreList.Add(new ShowIncreInfo() { Title = "深造", IncreInfoList = coreList });
				}
				roleList = new ObservableCollection<IncreInfo>();
				roleList.AddRange(IncreProList.Where(i => i.IncreClass != (int)IncreClass.AtkIncre).ToList());
				for (int i = 0; i < 5; i++)
				{
					RoleIncreList.Add(new ShowIncreInfo() { Title = "深造", IncreInfoList = roleList });
				}
			}
			catch (Exception ex)
			{
				_aggregator.GetEvent<SendInfoMessage>().Publish("深造方案初始化失败");

                LoggerHelper.Logger.Error(ex.Message, ex);
			}

		}

		private void AddIncreItemExecute()
		{
			RoleIncreList.Add(new ShowIncreInfo() { Title = "深造", IncreInfoList = roleList });
		}

		private void CancelExecute(Window window)
		{
			if (window != null)
			{
				window.Close();
			}
		}

		private async void SaveRoleExecute(Window window)
		{
			try
			{

                if (string.IsNullOrEmpty(DeepName))
                {
                    deepName = "未命名";
                }
                var findResult = await _deepInfoService.QueryAsync(i => i.DeepName.Equals(DeepName));
                if (findResult != null && findResult.Count() > 0)
                {
                    MessageBox.Show("已存在深造方案");
                    return;
                }
                var baseIncres = RoleIncreList.Where(i => i.SelectedIncre != null && i.SelectedIncre.IncreClass.Equals((int)IncreClass.BaseIncre)).ToList();
                var gameIncres = RoleIncreList.Where(i => i.SelectedIncre != null && i.SelectedIncre.IncreClass != (int)IncreClass.BaseIncre).ToList();
                CoreIncreList.AddRange(gameIncres);
                var increEf = CommonStaticSource.SerializeIncreInfo(baseIncres);
                increEf += "%" + CommonStaticSource.SerializeIncreInfo(CoreIncreList.ToList());
                var deepInfo = new DeepInfo()
                {
                    DeepName = deepName,
                    Post = (int)SelectedOccupation,
                    DeepEf = increEf
                };
                var saveResult = await _deepInfoService.CreatAsync(deepInfo);
                if (!saveResult)
                {
					var info = "深造方案保存失败";
                    _aggregator.GetEvent<SendInfoMessage>().Publish(info);
                    LoggerHelper.Logger.Error(info);
                }
                _aggregator.GetEvent<SendInfoMessage>().Publish("深造方案保存成功");
                if (window != null)
                {
                    window.Close();
                }
            }
			catch (Exception ex)
			{
				_aggregator.GetEvent<SendInfoMessage>().Publish("深造方案初始化失败");

				LoggerHelper.Logger.Error(ex.Message, ex);
			}
		}
	}
}
