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
	public class DeepInfoRepository:BaseRepository<DeepInfo>, IDeepInfoRepository
	{
		//public override Task<List<DeepInfo>> QueryAsync()
		//{
		//	return base.Context.Queryable<DeepInfo>()
		//		.Mapper(c => c.DeepName, c => c.DeepEf)
		//		.ToListAsync();
		//}
		//public async override Task<List<DeepInfo>> QueryAsync(Expression<Func<DeepInfo, bool>> func)
		//{
		//	return await base.Context.Queryable<DeepInfo>()
		//		.Where(func)
		//		.Mapper(c => c.DeepName, c => c.DeepEf)
		//		.ToListAsync();
		//}
	}
}
