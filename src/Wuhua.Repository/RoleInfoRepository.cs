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
	public class RoleInfoRepository:BaseRepository<RoleInfo>, IRoleInfoRepository
	{
		//public override Task<List<RoleInfo>> QueryAsync()
		//{
		//	return base.Context.Queryable<RoleInfo>()
		//		.Mapper(c => c.RoleName, c => c.RoleType, c => c.Weapon)
		//		.Mapper(c => c.Deep, c => c.IncreEf, c => c.Post)
		//		.ToListAsync();
		//}
		//public async override Task<List<RoleInfo>> QueryAsync(Expression<Func<RoleInfo, bool>> func)
		//{
		//	return await base.Context.Queryable<RoleInfo>()
		//		.Where(func)
		//		.Mapper(c => c.RoleName, c => c.RoleType, c => c.Weapon)
		//		.Mapper(c => c.Deep, c => c.IncreEf, c => c.Post)
		//		.ToListAsync();
		//}
	}
}
