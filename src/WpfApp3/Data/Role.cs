using Prism.Mvvm;

namespace WpfApp3.Data
{
    public class Role:BindableBase
	{
		#region 基础攻击
		public int BaseAtk { get; set; } = 0;
		#endregion
		#region 百分比加成
		// 考核
		public double CoreAtkPer { get; set; } = 0;
		// 深造
		public double DeepAtkPer { get; set; } = 0;
		// 致知
		public double LifeAtkPer { get; set; } = 0;
		// 武器
		public double WeaponAtkPer { get; set; } = 0;
		// 研学
		public double LearnAtkPer { get; set; } = 0;
		#endregion
		#region 数值加成
		// 深造
		public int DeepAtk { get; set; } = 0;
		// 致知
		public int LifeAtk { get; set; } = 0;
		// 武器
		public int WeaponAtk { get; set; } = 0;
		// 领域共研
		public int CommonLearnAtk { get; set; } = 0;
		#endregion
	}
}
