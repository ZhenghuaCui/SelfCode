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
    public class OldNow2witch:WeaponBase
    { 
        // 星霞
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            SkillList.Add(skillItem);
            if (IsGetAll)
            {
                return IncreInfos;
            }
            IncreInfos.Clear();
            if (SkillList.Any(i => i.AtkType == AtkType.Profession))
            {
                IncreInfos.Add(new ShowIncreInfo()
                {
                    Title = "武器",
                    IncreNum = "50",
                    SelectedIncre = new IncreInfo()
                    {
                        IncreClass = (int)IncreClass.DamageIncre,
                        IncreType = (int)IncreType.Percent,
                        IncreDetail = (int)AtkType.Stunk,
                        IncreNum = 50
                    }
                });
                IsGetAll = true;
            }
            return IncreInfos;
        }
    }
}
