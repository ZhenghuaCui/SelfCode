using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuhua.Model
{
	public  class RoleInfo:BaseId
	{
		public string RoleName { get; set; }
		public int Deep { get; set; }
		public int Life { get; set; }
		public int Weapon { get; set; }
		// 输出/辅助
		public int RoleType { get; set; }
		public int BaseAtk { get; set; }
		public string IncreEf { get; set; }
		public string SkillEf { get; set; }
		// 职业
		public int Post { get; set; }

	}
}
