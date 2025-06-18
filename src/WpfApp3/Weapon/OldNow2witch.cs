using System.Collections.Generic;
using WpfApp3.Common;
using WpfApp3.Data;

namespace Wuhua.Main.Weapon
{
    public class OldNow2witch:WeaponBase
    { 
        // 星霞
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            for (int i = 0; i < flag; i++)
            {
                CountIncreInfo(IncreDic["绝技增伤"], 25);
            }
            if (skillItem.AtkType.Equals(AtkType.Frequent) && flag < 2)
                flag++;

            return IncreInfos;
        }
    }
}
