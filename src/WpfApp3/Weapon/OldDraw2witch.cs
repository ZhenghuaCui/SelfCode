using System.Collections.Generic;
using WpfApp3.Data;

namespace Wuhua.Main.Weapon
{
    //旧画扇
    public class OldDraw2witch : WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            for (int i = 0; i < 1; i++)
            {
                CountIncreInfo(IncreDic["全增伤"], 20);
            }
            return IncreInfos;
        }
    }
}
