using System.Collections.Generic;
using WpfApp3.Data;

namespace Wuhua.Main.Weapon
{
    //熊猫竹炮
    public class Panda2Guard : WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            for (int i = 0; i < 1; i++)
            {
                CountIncreInfo(IncreDic["全增伤"], 30);
            }

            return IncreInfos;
        }
    }
}
