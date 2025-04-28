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
    // 熊猫竹剑
    public class Panda2Solder:WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            var incre1 = IncreDic["贯穿率"];
            IncreInfo incre1Copy = CommonStaticSource.DeepCopy<IncreInfo>(incre1);
            incre1Copy.IncreNum = 15;
            IncreInfos.Add(new ShowIncreInfo()
                {
                    Title = "武器",
                    IncreNum = "15",
                    SelectedIncre = incre1Copy
            });
            var incre2 = IncreDic["暴击率"];
            IncreInfo incre2Copy = CommonStaticSource.DeepCopy<IncreInfo>(incre2);

            incre2Copy.IncreNum = 10;
            IncreInfos.Add(new ShowIncreInfo()
            {
                Title = "武器",
                IncreNum = "10",
                SelectedIncre = incre2
            });
            var incre3 = IncreDic["暴击伤害"];
            IncreInfo incre3Copy = CommonStaticSource.DeepCopy<IncreInfo>(incre3);
            incre3Copy.IncreNum  =15;
            IncreInfos.Add(new ShowIncreInfo()
            {
                Title = "武器",
                IncreNum = "15",
                SelectedIncre = incre3
            });

            return IncreInfos;
        }
    }
}
