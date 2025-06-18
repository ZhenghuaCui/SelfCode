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
    public class AddWeaponViewModel:BindableBase
	{
		// 武器列表
		public ObservableCollection<WeaponInfo> CheckedWeaponList { get; private set; } = new ObservableCollection<WeaponInfo>();
		// 副词条加成List
		public ObservableCollection<IncreInfo> IncreProList { get; private set; } = new ObservableCollection<IncreInfo>();

		public ObservableCollection<ShowIncreInfo> ShowWeaponsList { get; set; } = new ObservableCollection<ShowIncreInfo>();
		//角色职业Enum list
		public Dictionary<Occupation, string> OccupationList { get; set; }

		#region properties

		//角色职业
		private Occupation _selectedOccupation;
		public Occupation SelectedOccupation
		{
			get => _selectedOccupation;
			set
			{
				SetProperty(ref _selectedOccupation, value);
				CheckedWeaponList.Clear();
				CheckedWeaponList.AddRange(_commonSource.WeaponInfos.FindAll(i=>i.Post==(int)value));
				SelectedWeapon = CheckedWeaponList.FirstOrDefault();
			}
		}
		

		//角色武器选择

		private WeaponInfo _selectedWeapon = null;
		public WeaponInfo SelectedWeapon
		{
			get => _selectedWeapon;
			set
			{
				SetProperty(ref _selectedWeapon, value);
				if(_selectedWeapon is null)
				{
					BaseAtk = "0";
				}
				else
				{
					BaseAtk = _selectedWeapon.BaseAtk.ToString();
				}
			}
		}
		// 武器主词条攻击

		private string _baseAtk = "0";
		public string BaseAtk
		{
			get => _baseAtk;
			set
			{
				int number;
				if (int.TryParse(value, out number))
				{
					SetProperty(ref _baseAtk, number.ToString());
				}
				else
				{
					SetProperty(ref _baseAtk, "0");
				}
				
			}
		}

		//角色武器副属性A
		private IncreInfo _selectedWeaponPA = new IncreInfo();
		public IncreInfo SelectedWeaponPA
		{
			get => _selectedWeaponPA;
			set
			{
				SetProperty(ref _selectedWeaponPA, value);
			}
		}

		//角色武器副属性B
		private IncreInfo _selectedWeaponPB = new IncreInfo();
		public IncreInfo SelectedWeaponPB
		{
			get => _selectedWeaponPB;
			set
			{
				SetProperty(ref _selectedWeaponPB, value);
			}
		}

		//角色武器副属性C
		private IncreInfo _selectedWeaponPC = new IncreInfo();
		public IncreInfo SelectedWeaponPC
		{
			get => _selectedWeaponPC;
			set
			{
				SetProperty(ref _selectedWeaponPC, value);
			}
		}
		#endregion
		#region commmand
		public DelegateCommand<Window> SaveWeaponCommmand { get; set; }
		public DelegateCommand<Window> CancelCommand { get; set; }

		#endregion
		ICustomWeaponService _customWeaponService;
		IWeaponInfoService _weaponService;
		IEventAggregator _aggregator;
		CommonSource _commonSource;
		public AddWeaponViewModel(IContainerProvider containerProvider, IEventAggregator aggregator,ICustomWeaponService customWeaponService, IWeaponInfoService weaponInfoservice)
		{
			_commonSource = containerProvider.Resolve<CommonSource>("CommonSource");

			_weaponService = weaponInfoservice;
			_customWeaponService = customWeaponService;
			_aggregator = aggregator;

			SaveWeaponCommmand = new DelegateCommand<Window>(SaveWeaponCommmandExecute);
			CancelCommand = new DelegateCommand<Window>(CancelCommandExecute);

			Init();
		}

		private void CancelCommandExecute(Window window)
		{
			if (window != null)
			{
				window.Close();
			}
		}

		private void Init()
		{
			//排除攻击加成区及易伤区选项
			IncreProList.AddRange(_commonSource.IncreInfos.Where(i => i.IncreClass != (int)IncreClass.AtkIncre
																	&& i.IncreClass != (int)IncreClass.WeakIncre));
			CheckedWeaponList.AddRange(_commonSource.WeaponInfos.FindAll(i=>i.Post == (int)Occupation.Solder));
			OccupationList = CommonStaticSource.OccupationDic;

			for (int i=0; i < 3; i++)
			{
				ShowWeaponsList.Add(new ShowIncreInfo() {Title = "武器", IncreInfoList = IncreProList });
			}
		}

		private async void SaveWeaponCommmandExecute(Window window)
		{
			try
			{
				var value = DateTime.Now.ToString("yyyyMMddHHmmssms");
				CustomWeaponInfo customWeaponInfo = new CustomWeaponInfo()
				{

					Weapon = SelectedWeapon.Weapon,
					WeaponUId = value,
					BaseAtk = int.Parse(BaseAtk),
					Post = SelectedWeapon.Post
				};
				customWeaponInfo.WeaponEf = CommonStaticSource.SerializeIncreInfo(ShowWeaponsList.ToList());

				await _customWeaponService.CreatAsync(customWeaponInfo);
				if (window != null)
				{
					window.Close();
				}

				_aggregator.GetEvent<WeaponAddedEvent>()?.Publish(customWeaponInfo);
				var info = "武器存储成功";
				_aggregator.GetEvent<SendInfoMessage>()?.Publish(info);
                LoggerHelper.Logger.Info(info);
            }
			catch (Exception ex)
			{
				LoggerHelper.Logger.Error(ex.Message, ex);
			}
			
		}
	}
}
