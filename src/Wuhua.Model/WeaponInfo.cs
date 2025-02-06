using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuhua.Model
{
	public class WeaponInfo:BaseId
	{
		[SugarColumn(ColumnDataType = "nvarchar(30)")]
		public string Weapon { get; set; }
		public int BaseAtk { get; set; }
		[SugarColumn(ColumnDataType = "nvarchar(30)")]
		public string WeaponEf { get; set; }
		public int Post { get; set; }
	}
}
