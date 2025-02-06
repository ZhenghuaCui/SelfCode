using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using WpfApp3.Common;
using Wuhua.Main.Data;
using Wuhua.Main.Events;

namespace Wuhua.Main.ViewModels
{

    public class CountWeaponControlViewModel:BindableBase
    {
        private ObservableCollection<ShowIncreInfoLight> increInfos;
        public ObservableCollection<ShowIncreInfoLight> IncreInfos{
            get
            {
                return increInfos;
            }
            set
            {
                SetProperty(ref increInfos, value);
            }
        }
        private CommonSource _commonSource;
        IEventAggregator _aggregator;

        public CountWeaponControlViewModel(IContainerProvider containerProvider,IEventAggregator aggregator) {
            _commonSource = containerProvider.Resolve<CommonSource>("CommonSource");
            _aggregator = aggregator;
            InitList();
        }

        private void InitList()
        {
            var baseList = _commonSource.IncreDic.Values.ToList();
            IncreInfos = new ObservableCollection<ShowIncreInfoLight>();
            var increList = baseList.Where(i => i.IncreNum != 0);
            foreach (var incre in increList)
            {
                var item = new ShowIncreInfoLight() { IncreInfo = incre };
                IncreInfos.Add(item);
            }
            
        }
    }
}
