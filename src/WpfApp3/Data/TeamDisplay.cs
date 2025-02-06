using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Mvvm;
using WpfApp3.Common;
using Wuhua.IService;
using Wuhua.Model;
using Wuhua.NLog;

namespace WpfApp3.Data
{
    public class TeamDisplay : BindableBase
    {
        public DelegateCommand ClearCommand { get; set; }
        private ICustomWeaponService _customWeaponService;
        private IRoleInfoService _roleInfoService;
        public TeamDisplay(ICustomWeaponService weaponService, IRoleInfoService roleInfoService)
        {

            _customWeaponService = weaponService;
            _roleInfoService = roleInfoService;
            InitWeaponList();
            InitRolesList();
        }

        public async void InitRolesList()
        {
            RolesList.Clear();
            var roles = await _roleInfoService.QueryAsync();
            RolesList.AddRange(roles);
        }

        public async void InitWeaponList()
        {
            try
            {
                int post = 0;
                if (selectedRole != null) post = selectedRole.Post;
                CustomWeaponList.Clear();
                var weponlist = await _customWeaponService.QueryAsync(i => i.Post == post);
                foreach (var item in weponlist)
                {
                    List<ShowIncreInfo> efList = CommonStaticSource.DeSerializeIncreInfo(item.WeaponEf);
                    CustomWeaponList.Add(new CustomShowWeapon()
                    {
                        Id = item.Id,
                        BaseAtk = item.BaseAtk,
                        Weapon = item.Weapon,
                        WeaponEfList = efList,
                        Post = item.Post
                    });
                }
                var veryItem = CustomWeaponList.FirstOrDefault();
                if (veryItem != null)
                {
                    SelectedWeapon = veryItem;
                }
                ClearCommand = new DelegateCommand(ClearExecute);
            }
            catch (Exception ex)
            {
                LoggerHelper.Logger.Error(ex.Message, ex);
            }
        }

        private void ClearExecute()
        {
            SelectedRole = null;
            InitWeaponList();
        }

        public ObservableCollection<RoleInfo> RolesList { get; set; } = new ObservableCollection<RoleInfo>();
        public ObservableCollection<CustomShowWeapon> CustomWeaponList { get; set; } = new ObservableCollection<CustomShowWeapon>();

        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                SetProperty(ref isChecked, value);
            }
        }
        private RoleInfo selectedRole;
        public RoleInfo SelectedRole
        {
            get { return selectedRole; }
            set
            {
                SetProperty(ref selectedRole, value);
                if (selectedRole != null && (selectedWeapon == null || selectedRole.Post != selectedWeapon.Post))
                {
                    InitWeaponList();
                }
            }
        }
        private CustomShowWeapon selectedWeapon = new CustomShowWeapon();
        public CustomShowWeapon SelectedWeapon
        {
            get { return selectedWeapon; }
            set
            {
                SetProperty(ref selectedWeapon, value);
            }
        }

    }
}
