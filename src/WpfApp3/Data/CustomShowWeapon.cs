using System.Collections.Generic;
using Prism.Mvvm;

namespace WpfApp3.Data
{
    public class CustomShowWeapon : BindableBase
    {
        public int Id { get; set; }
        public List<ShowIncreInfo> WeaponEfList { get; set; }
        public string Weapon { get; set; }
        public int BaseAtk { get; set; }
        public int Post { get; set; } = 0;
    }
}
