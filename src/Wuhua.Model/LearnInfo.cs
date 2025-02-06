using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuhua.Model
{
	public class LearnInfo :BaseId
	{
		public int LearnType { get; set; } 
		public string LearnEf { get; set; }
		public int Post { get; set; }
	}
}
