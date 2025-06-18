using System.Collections.Generic;
using WpfApp3.Data;

namespace Wuhua.Main.Weapon
{
    // 熊猫竹剑
    public class Panda2Solder:WeaponBase
    {
        public override List<ShowIncreInfo> GetIncre(SkillItem skillItem)
        {
            IncreInfos.Clear();
            for (int i = 0; i < 1; i++)
            {
                CountIncreInfo(IncreDic["贯穿率"], 15);
                CountIncreInfo(IncreDic["暴击率"], 10);
                CountIncreInfo(IncreDic["暴击伤害"], 15);
            }

            return IncreInfos;
        }
    }
}
