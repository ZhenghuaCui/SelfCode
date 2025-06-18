using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WpfApp3.Data;
using WpfApp3.Events;
using Wuhua.IService;
using Wuhua.Model;

namespace WpfApp3.ViewModels
{
    public class TeamControlViewModel:BindableBase
	{
		public DelegateCommand<CustomShowWeapon> DeleWeaponCommand { get; set; }
		public DelegateCommand<RoleInfo> DeleRoleCommand { get; set; }
		
		#region list

		// 攻击数值list
		public ObservableCollection<TeamDisplay> TeamList { get; private set; } = new ObservableCollection<TeamDisplay>();

		#endregion
		#region
		//private RoleInfo _selectedRole = null;
		//public RoleInfo SelectedRole
		//{
		//	get { return _selectedRole; }
		//	set
		//	{
		//		SetProperty(ref _selectedRole, value);
		//	}
		//}

		//private CustomWeaponInfo _selectedWeapon = null;
		//public CustomWeaponInfo SelectedWeapon
		//{
		//	get { return _selectedWeapon; }
		//	set
		//	{
		//		SetProperty(ref _selectedWeapon, value);
		//	}
		//}
		#endregion
		#region
		IEventAggregator _aggregator;
		#endregion

		IRoleInfoService _roleService;
		ICustomWeaponService _weaponService;
		public TeamControlViewModel(IRoleInfoService roleService, IEventAggregator aggregator, ICustomWeaponService weaponService)
		{
			_aggregator = aggregator;
			_aggregator.GetEvent<RoleAddedEvent>().Subscribe(RolesAdded);
			_aggregator.GetEvent<RoleEditedEvent>().Subscribe(RoleEdited);
			_aggregator.GetEvent<WeaponAddedEvent>().Subscribe(WeaponAdded);
			_roleService = roleService;
			_weaponService = weaponService;
			var a = Application.Current as App;
			var registry = (IContainerRegistry)a.Container;
			registry.RegisterInstance(TeamList, "TeamList");
			InitDisplay();
			DeleWeaponCommand = new DelegateCommand<CustomShowWeapon>(DeleteWeaponExecute);
			DeleRoleCommand = new DelegateCommand<RoleInfo>(DeleRoleExecute);
		}



		private async  void DeleRoleExecute(RoleInfo role)
		{
			var result = await _roleService.DeleteAsync(role.Id);
			if (result)
			{
				foreach (var item in TeamList)
				{
					item.InitRolesList();
				}
			}
		}

		private async void DeleteWeaponExecute(CustomShowWeapon delWeapon)
		{
			var result = await _weaponService.DeleteAsync(delWeapon.Id);
			if (result)
			{
				foreach(var item in TeamList)
				{
					if (item.SelectedWeapon.Post.Equals(delWeapon.Post))
					{
						item.InitWeaponList();
					}
				}
			}
		}

		private void WeaponAdded(CustomWeaponInfo obj)
		{
			foreach (var item in TeamList)
			{
				item.InitWeaponList();
			}
		}

		private  void RolesAdded(RoleInfo role)
		{
			foreach(var item in TeamList)
			{
				item.RolesList.Add(role);
			}
		}
		private void RoleEdited(bool obj)
		{
			InitDisplay();
		}

		private  void InitDisplay()
		{
			TeamList.Clear();
			for (int i = 0; i < 6; i++)
			{
				TeamList.Add(new TeamDisplay(_weaponService, _roleService) );
			}
			TeamList.FirstOrDefault().IsChecked = true;
		}
		public void Dispose()
		{
            _aggregator.GetEvent<RoleAddedEvent>().Unsubscribe(RolesAdded);
            _aggregator.GetEvent<RoleEditedEvent>().Unsubscribe(RoleEdited);
            _aggregator.GetEvent<WeaponAddedEvent>().Unsubscribe(WeaponAdded);
        }
	}
}
