using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfApp3.Data;
using WpfApp3.Events;
using WpfApp3.Internfaces;
using WpfApp3.Roles;
using Wuhua.Main.Data;
using Wuhua.Main.Events;
using Wuhua.Model;

namespace WpfApp3.ViewModels
{

    /// <summary>
    /// 加载怪物信息及最终计算结果页面
    /// </summary>
    public class DamageOutControlViewModel:BindableBase
	{
		private IEventAggregator _aggregator;
		public RolesInfoControlViewModel rolesVm;

		private string damageResult = "";
		public string DamageResult
		{
			get { return damageResult; }
			set
			{
				SetProperty(ref damageResult, value);
			}
		}
		public ObservableCollection<Monster> MonsterDefList { get; set; } = new ObservableCollection<Monster>();
		public ObservableCollection<string> ResultList { get; set; } = new ObservableCollection<string>();
		public List<IncreInfo> WeaponEntryList = new List<IncreInfo>();
        public DamageOutControlViewModel(IContainerProvider containerProvider, IEventAggregator aggregator)
		{
			_aggregator = aggregator;
			rolesVm = containerProvider.Resolve<RolesInfoControlViewModel>("RolesInfo");
			_aggregator.GetEvent<CountDamageStartEvent>().Subscribe(CountDamageExecute);
            _aggregator.GetEvent<CountWeaponEvent>().Subscribe(CountWeaponExecute);

            //GetResult();
            for (int i = 0; i < 3; i++)
			{
				MonsterDefList.Add(new Monster());
			}
			MonsterDefList.FirstOrDefault().Def = "1500";
		}

        private void CountWeaponExecute(List<IncreInfo> list)
        {
			WeaponEntryList.AddRange(list);
        }

        private void CountDamageExecute(bool obj)
		{
			GetResult();
		}

		private void GetResult()
		{
			IRoles roleItem;
			switch (rolesVm.CurrentRole.SelectedRole.RoleName)
			{
				case "莫高窟":
				default:
					roleItem = new MogaoGrottoes(); break;
			}
			roleItem.Register(rolesVm,MonsterDefList.ToList(), ResultList, WeaponEntryList);
			var resultList = roleItem.GetOutPut();

			_aggregator.GetEvent<DeliverResultEvent>()?.Publish(resultList);
		}
        public void Dispose()
        {
			WeaponEntryList.Clear();
            _aggregator.GetEvent<CountWeaponEvent>().Unsubscribe(CountWeaponExecute);
            _aggregator.GetEvent<CountDamageStartEvent>().Unsubscribe(CountDamageExecute);
        }
    }
}
