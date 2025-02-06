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
    /// 领域共研、博物研学页面
    /// </summary>
    public class LearnControlViewModel:BindableBase
	{
		#region Command
		public DelegateCommand<Window> SaveCommmand { get; set; }
		public DelegateCommand<Window> CancelCommand { get; set; }
		public DelegateCommand AddLearnCommand { get; set; }
		public DelegateCommand AddCommonCommand { get; set; }
		#endregion
		private Dictionary<Occupation, string> occupationList;
		public Dictionary<Occupation, string> OccupationList
		{
			get { return occupationList; }
			set
			{
				SetProperty(ref occupationList, value);
			}
		}

		//面板属性加成列表
		public ObservableCollection<ShowIncreInfo> LearnList { get; set; } = new ObservableCollection<ShowIncreInfo>();

		// 副词条加成List
		public ObservableCollection<IncreInfo> IncreProList { get; set; } = new ObservableCollection<IncreInfo>();

		//游戏内加成列表
		public ObservableCollection<ShowIncreInfo> CommonList { get; set; } = new ObservableCollection<ShowIncreInfo>();

		//角色职业Enum list
		Dictionary<RoleProEnm, string> roleProDic = CommonStaticSource.RoleProDic;
		private ObservableCollection<IncreInfo> baseIncreList;
		CommonSource _commonSource;
		IEventAggregator _aggregator;

        ILearnInfoService _learnInfoService;
		private LearnInfo currentLearnInfo;
		public LearnControlViewModel(IContainerProvider containerProvider, IEventAggregator aggregator, ILearnInfoService learnInfoService)
		{
			_commonSource = containerProvider.Resolve<CommonSource>("CommonSource");
			IncreProList.AddRange(_commonSource.IncreInfos);
			_learnInfoService = learnInfoService;
			_aggregator = aggregator;

            SaveCommmand = new DelegateCommand<Window>(SaveCommmandExecute);
			CancelCommand = new DelegateCommand<Window>(CancelCommandExecute);
			AddLearnCommand = new DelegateCommand(AddLearnCommandExecute);
			AddCommonCommand = new DelegateCommand(AddCommonCommandExecute);
			Init();
		}

		private void AddCommonCommandExecute()
		{
			CommonList.Add(new ShowIncreInfo() { Title = roleProDic[RoleProEnm.CommonLearn], IncreInfoList = baseIncreList });
		}

		private void AddLearnCommandExecute()
		{
			LearnList.Add(new ShowIncreInfo() { Title = roleProDic[RoleProEnm.Learn], IncreInfoList = baseIncreList });
		}

		private void CancelCommandExecute(Window window)
		{
			if (window != null)
			{
				window.Close();
			}
		}

		private async void SaveCommmandExecute(Window window)
		{
			try
			{
                var learnBaseList = LearnList.Where(i => i.SelectedIncre != null && i.SelectedIncre.IncreClass.Equals((int)IncreClass.BaseIncre)).ToList();
                var learnGameList = LearnList.Where(i => i.SelectedIncre != null && i.SelectedIncre.IncreClass != (int)IncreClass.BaseIncre).ToList();
                var learnEf = CommonStaticSource.SerializeIncreInfo(learnBaseList);
                learnEf += "%" + CommonStaticSource.SerializeIncreInfo(learnGameList);
                if (currentLearnInfo == null)
                {
                    currentLearnInfo = new LearnInfo()
                    {
                        LearnType = (int)LearnType.Learn,
                        LearnEf = learnEf
                    };
                    await _learnInfoService.CreatAsync(currentLearnInfo);
                }
                else
                {
                    currentLearnInfo.LearnEf = learnEf;
                    await _learnInfoService.EditAsync(currentLearnInfo);
                }
                await _learnInfoService.DeleteAsync(i => i.LearnType.Equals((int)LearnType.CommmonLearn));
                foreach (var post in OccupationList)
                {
                    var commonBaseList = CommonList.Where(i => i.SelectedIncre != null && i.SelectedIncre.IncreClass.Equals((int)IncreClass.BaseIncre) && i.SelectedOccupation.Equals(post.Key)).ToList();
                    var commonGameList = CommonList.Where(i => i.SelectedIncre != null && i.SelectedIncre.IncreClass != (int)IncreClass.BaseIncre && i.SelectedOccupation.Equals(post.Key)).ToList();
                    var commonEf = CommonStaticSource.SerializeIncreInfo(commonBaseList);
                    commonEf += "%" + CommonStaticSource.SerializeIncreInfo(commonGameList);
                    var common = new LearnInfo()
                    {
                        LearnType = (int)LearnType.CommmonLearn,
                        LearnEf = commonEf,
                        Post = (int)post.Key
                    };
                    await _learnInfoService.CreatAsync(common);
                }
				var info = "深造/共研方案存储成功";
                _aggregator.GetEvent<SendInfoMessage>().Publish(info);
                LoggerHelper.Logger.Info(info);
                if (window != null)
                {
                    window.Close();
                }
            }
			catch (Exception ex)
			{
				_aggregator.GetEvent<SendInfoMessage>().Publish("深造/共研方案存储失败");
				LoggerHelper.Logger.Error(ex.Message, ex);
			}
			
		}

		private async void Init()
		{
            try
            {
                OccupationList = CommonStaticSource.OccupationDic;
                baseIncreList = new ObservableCollection<IncreInfo>();
                baseIncreList.AddRange(IncreProList.Where(i => i.IncreClass != (int)IncreClass.AtkIncre));
                var learn = await _learnInfoService.FindAsync(i => i.LearnType.Equals((int)LearnType.Learn));
                if (learn != null)
                {
                    currentLearnInfo = learn;
                    var result = learn.LearnEf.Split("%");
                    foreach (var str in result)
                    {
                        LearnList.AddRange(CommonStaticSource.DeSerializeIncreInfo(str, baseIncreList));
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        LearnList.Add(new ShowIncreInfo() { Title = roleProDic[RoleProEnm.Learn], IncreInfoList = baseIncreList });
                    }
                }
                var commons = await _learnInfoService.QueryAsync(i => i.LearnType.Equals((int)LearnType.CommmonLearn));
                if (commons != null && commons.Count() > 0)
                {
                    foreach (var common in commons)
                    {
                        var result = common.LearnEf.Split("%");
                        foreach (var str in result)
                        {
                            CommonList.AddRange(CommonStaticSource.DeSerializeIncreInfo(str, baseIncreList,(Occupation)common.Post));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        CommonList.Add(new ShowIncreInfo() { Title = roleProDic[RoleProEnm.CommonLearn], IncreInfoList = baseIncreList });
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
            }
			

		}
	}
}
