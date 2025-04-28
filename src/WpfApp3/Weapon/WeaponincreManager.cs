using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Data;
using Wuhua.Main.Internfaces;
using Wuhua.Model;

namespace Wuhua.Main.Weapon
{
    public  class WeaponIncreManager
    {
        private IWeaponIncre weaponIncre;
        public void SetWeaponInstance(CustomShowWeapon customWeapon)
        {
            if (customWeapon == null || weaponIncre!=null) return;
            switch (customWeapon.Weapon)
            {
                case "熊猫竹灯":
                    weaponIncre = new Panda2Witch();
                    break;
                case "旧画扇":
                    weaponIncre = new OldDraw2witch();
                    break;
                case "星霞":
                    weaponIncre = new OldNow2witch();
                    break;
                case "古典松木弩":
                    weaponIncre = new OldWuden2Shooter();
                    break;
                case "熊猫竹剑":
                    weaponIncre = new Panda2Solder();
                    break;
                case "熊猫竹炮":
                    weaponIncre = new Panda2Guard();
                    break;
                default:
                    weaponIncre = new WeaponBase();
                    break;
            }
        }
        public List<ShowIncreInfo> GetWeapoIncre(SkillItem skill)
        {
            return weaponIncre.GetIncre(skill);
        }
    }
}
