using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Common;
using WpfApp3.Data;
using Wuhua.Model;

namespace Wuhua.Main.Weapon
{
    // 古典松木弩
    public class OldWuden2Shooter :WeaponBase
    {
        private bool isSecond = false;
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            if (skillItem.AtkType.Equals(AtkType.Extra))
            {
                int value = !isSecond ? 20 : 40;
                if (!isSecond)
                {
                    isSecond=true;
                }
                IncreInfos.Add(new ShowIncreInfo()
                {
                    Title = "武器",
                    IncreNum = value.ToString(),
                    SelectedIncre = new IncreInfo()
                    {
                        IncreClass = (int)IncreClass.DamageIncre,
                        IncreType = (int)IncreType.Percent,
                        IncreNum = value
                    }
                });
            }
            
            return IncreInfos;
        }
    }
}
