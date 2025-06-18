using WpfApp3.Data;

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
