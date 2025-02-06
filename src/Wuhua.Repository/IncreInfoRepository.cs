using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Wuhua.IRepository;
using Wuhua.Model;

namespace Wuhua.Repository
{
	public class IncreInfoRepository:BaseRepository<IncreInfo>, IIncreInfoRepository
	{
		//public override Task<List<IncreInfo>> QueryAsync()
		//{
		//	return base.Context.Queryable<IncreInfo>()
		//		.Mapper(c => c.IncreClass, c => c.IncreType)
		//		.Mapper(c => c.IncreName, c => c.IncreNum)
		//		.ToListAsync();
		//}
		//public async override Task<List<IncreInfo>> QueryAsync(Expression<Func<IncreInfo, bool>> func)
		//{
		//	return await base.Context.Queryable<IncreInfo>()
		//		.Where(func)
		//		.Mapper(c => c.IncreClass, c => c.IncreType)
		//		.Mapper(c => c.IncreName, c => c.IncreNum)
		//		.ToListAsync();
		//}
	}
}
