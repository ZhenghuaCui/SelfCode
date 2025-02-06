using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuhua.Model
{
	public class DeepInfo:BaseId
	{
		[SugarColumn(ColumnDataType = "nvarchar(30)")]
		public string DeepName { get; set; }
		// 使用‘%’区分基础攻击属性和其他属性
		public string DeepEf { get; set; }
		public int Post { get; set; }
	}
}
