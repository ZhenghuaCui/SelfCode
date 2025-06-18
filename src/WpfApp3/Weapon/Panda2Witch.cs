using System.Collections.Generic;
using WpfApp3.Common;
using WpfApp3.Data;

namespace Wuhua.Main.Weapon
{
    // 熊猫竹灯
    public class Panda2Witch : WeaponBase
    {
        public Panda2Witch():base(){ }
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            CountIncreInfo(IncreDic["全增伤"], 15);
            for (int i = 0; i < flag; i++)
            {

                CountIncreInfo(IncreDic["防御穿透"], 20);
            }
            if (skillItem.AtkType.Equals(AtkType.Profession) && flag <1)
                flag++;
            return IncreInfos;
        }
    }
}
