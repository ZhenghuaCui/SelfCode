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
    public class Panda2Guard : WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            var incre1 = IncreDic["全增伤"];
            IncreInfo incre1Copy = CommonStaticSource.DeepCopy<IncreInfo>(incre1);
            incre1Copy.IncreNum = 15;
            IncreInfos.Add(new ShowIncreInfo()
            {
                Title = "武器",
                IncreNum = "30",
                SelectedIncre = incre1Copy 
            });

            return IncreInfos;
        }
    }
}
