namespace WpfApp3.Common
{
    public enum DamageType
	{
		// 物理伤害
		Physical,
		// 构术伤害
		Magic,
		// 真实伤害
		Authentic,
		// 流失
		LostBlood,
		// 熔伤
		Melt,
		// 灼烧
		Burn
	} 
	public enum SpecialDamage
	{
		// 暴击伤害
		Critical,
		// 技能伤害
		SkillInjury,

	}
	public enum LearnType
	{
		// 博物研学
		Learn,
		// 领域共研
		CommmonLearn,

	}
	public enum AtkType
	{
		// 警戒
		Alert,
		// 常击
		Frequent,
		// 连击
		Batter,
		// 绝技
		Stunk,
		// 回击
		Counterattack,
		// 额外
		Extra,
		// 职业技
		Profession,
		// 持续伤害
		Contential,
	}
	public enum Occupation
	{
		// 轻锐
		Solder=0,
		// 构术
		Witch=1,
		// 远击
		Shooter=2,
		// 宿卫
		Tank=3,
		// 战略
		Guard=4
	}
	public enum RoleProEnm
	{
		// 深造
		Deep,
		// 致知
		Life,
		// 武器
		Weapon,
		// 角色职业
		Occupation,
		// 角色定位
		RoleType,
		// 加成效果
		IncreEf,
		// 考核
		CoreLearn,
		// 共研
		CommonLearn,
		//博物研学
		Learn
	}
	public enum DefArea
	{
		// 防御穿透
		CrossDef,
		// 贯穿率
		ThroughOut,
		// 贯穿强度
		ThroughOutNum,

		DefDown
	}
	public enum RoleTypeEnum
	{
		AD,
		Worker
	}
	// 加成分区
	public enum IncreClass
	{
		// 基础区
		BaseIncre=0,
		// 攻击加成区
		AtkIncre = 1,
		// 倍率区
		Skill = 2,
		// 减防区
		DeDef = 3,
		// 暴击区
		Critical = 4,
		// 伤害类型区
		DamageIncre = 5,
        // 易伤区
        WeakIncre = 6,

	}
	// 加成属性
	public enum IncreType		
	{
		//百分比属性，含暴击率，攻击加成，穿防率，全增伤
		Percent = 0,
		//数值属性，含暴击伤害，攻击力，攻击类型增伤
		Number = 1,
		//伤害类型增伤，易伤
		DamageType = 2,
		//目标类型增伤，易伤
		TargetType = 3,
		//特殊伤害类型，如暴击伤害，持续伤害
		SpetialType = 4,
	}

}
