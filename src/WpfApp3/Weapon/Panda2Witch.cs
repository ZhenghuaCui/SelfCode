using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Common;
using WpfApp3.Data;
using Wuhua.Main.Internfaces;
using Wuhua.Model;

namespace Wuhua.Main.Weapon
{
    // 熊猫竹灯
    public class Panda2Witch : WeaponBase
    {
        public Panda2Witch():base(){ }
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
                    IncreNum = "20",
                    SelectedIncre = new IncreInfo()
                    {
                        IncreClass = (int)IncreClass.DeDef,
                        IncreType = (int)IncreType.Percent,
                        IncreDetail = (int)DefArea.CountDownDef,
                        IncreNum = 20
                    }
                });
                IncreInfos.Add(new ShowIncreInfo()
                {
                    Title = "武器",
                    IncreNum = "15",
                    SelectedIncre = new IncreInfo()
                    {
                        IncreClass = (int)IncreClass.DamageIncre,
                        IncreType = (int)IncreType.Percent,
                        IncreNum = 15
                    }
                });
                IsGetAll = true;
            }
            return IncreInfos;
        }
    }
}
