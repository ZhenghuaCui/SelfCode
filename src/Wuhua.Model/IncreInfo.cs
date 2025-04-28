using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wuhua.Model
{
	public class IncreInfo:BaseId
	{
		[SugarColumn(ColumnDataType = "nvarchar(30)")]
		public string IncreName { get; set; }
		// 分区，如基础攻击区，攻击区，减防区
		public int IncreClass { get; set; }
		// 加成类型，如攻击力，攻击加成，暴击率，暴击伤害的乘区区分
		public int IncreType { get; set; }
		// 乘区详细类型，细分为物理、构术伤害等
		public int IncreDetail { get; set; }
		public int IncreNum { get; set; }
	}
}
