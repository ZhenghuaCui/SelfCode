using SqlSugar;
using System;

namespace Wuhua.Model
{
	public class BaseId
	{
		[SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
		public int Id { get; set; }
	}
}
