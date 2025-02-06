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
    public class OldDraw2witch : WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            IncreInfos.Add(new ShowIncreInfo()
            {
                Title = "武器",
                IncreNum = "20",
                SelectedIncre = new IncreInfo()
                {
                    IncreClass = (int)IncreClass.DamageIncre,
                    IncreType = (int)IncreType.Percent,
                    IncreNum = 20
                }
            });
            return IncreInfos;
        }
    }
}
