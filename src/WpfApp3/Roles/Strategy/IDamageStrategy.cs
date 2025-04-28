using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Data;
using WpfApp3.ViewModels;
using Wuhua.Main.Data;

namespace Wuhua.Main.Roles.Strategy
{
    public interface IDamageStrategy
    {
        decimal CalculateDefenceReduction(Monster monster);
        decimal CalculateCriticalBonus(SkillItem skillItem);
        decimal CalculateInjuryMultiplier(SkillItem skillItem);
        decimal CalculateSkill(SkillItem item);
    }
}
