using System.Collections.Generic;
using WpfApp3.Common;
using WpfApp3.Data;

namespace Wuhua.Main.Weapon
{
    // 古典松木弩
    public class OldWuden2Shooter :WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            for (int i = 0; i < flag; i++)
            {
                CountIncreInfo(IncreDic["全增伤"], 20);
            }
            if (skillItem.AtkType.Equals(AtkType.Extra) && flag < 2)
                flag++;

            return IncreInfos;
        }
    }
}
