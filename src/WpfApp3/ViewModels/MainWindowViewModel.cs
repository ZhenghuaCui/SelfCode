
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml;
using WpfApp3.Common;
using WpfApp3.Events;
using Wuhua.IService;
using Wuhua.NLog;
using Wuhua.Service;

namespace WpfApp3.ViewModels
{

    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<string> ResultList { get; private set; } 
            = new ObservableCollection<string>();
        private string infoMsg = "";
        public string InfoMsg
        {
            get { return infoMsg; }
            set
            {
                SetProperty(ref infoMsg, value);
            }
        }
        private string version = "";
        public string Version
        {
            get { return version; }
            set
            {
                SetProperty(ref version, value);
            }
        }
        private IContainerProvider _containerProvider;
        private IRoleInfoService _roleService;
        private IWeaponInfoService _weaponService;
        private IIncreInfoService _increInfoService;
        private IEventAggregator _aggregator;
        public MainWindowViewModel(IContainerProvider containerProvider, IEventAggregator aggregator)
        {
            _containerProvider = containerProvider;
            _aggregator = aggregator;
            _roleService = _containerProvider.Resolve<RoleInfoService>();
            _weaponService = _containerProvider.Resolve<WeaponInfoService>();  
            _increInfoService = _containerProvider.Resolve<IncreInfoService>();
            _aggregator.GetEvent<DeliverResultEvent>()?.Subscribe(ReceivedResult);
            _aggregator.GetEvent<SendInfoMessage>()?.Subscribe(ReceivedMsg);
            Init();
        }

        private void ReceivedMsg(string info)
        {
            InfoMsg = info;
        }

        private void ReceivedResult(List<string> results)
		{
            ResultList.AddRange(results);
        }

		private async void Init()
		{
            CommonSource commonSource = new CommonSource(_increInfoService);
            var list = await _weaponService.QueryAsync();
            commonSource.WeaponInfos.AddRange(list);
            commonSource.IncreInfos.AddRange(await _increInfoService.QueryAsync());
            var a = Application.Current as App;
            var _registry = (IContainerRegistry)a.Container;
            _registry.RegisterInstance(commonSource,"CommonSource");

            ResultList.Add("计算信息显示：");
            InitVersion();

        }

        private void InitVersion()
        {
            //XmlTextWriter writer = new XmlTextWriter("Version.xml", System.Text.Encoding.UTF8);
            //writer.Formatting = Formatting.Indented;
            //writer.WriteElementString("version","1.0.0");
            //writer.Close();
            XmlDocument doc = new XmlDocument();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resource/Version.xml");
            try
            {
                doc.Load(path);
                XmlNodeList lis = doc.GetElementsByTagName("version");
                Version = lis[0].InnerText;
            }
            catch (Exception ex)
            {
                var info = $"配置版本文件失败:{path}";
                InfoMsg = info;
                LoggerHelper.Logger.Error(info);
                LoggerHelper.Logger.Error(ex.Message, ex);
            }

        }
        public void Dispose()
        {
            _aggregator.GetEvent<DeliverResultEvent>()?.Unsubscribe(ReceivedResult);
            _aggregator.GetEvent<SendInfoMessage>()?.Unsubscribe(ReceivedMsg);
        }

    }
}
