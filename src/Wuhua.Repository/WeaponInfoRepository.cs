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
	public class WeaponInfoRepository:BaseRepository<WeaponInfo>, IWeaponInfoRepository
	{
		//public override Task<List<WeaponInfo>> QueryAsync()
		//{
		//	return base.Context.Queryable<WeaponInfo>()
		//		.Mapper(c => c.Weapon, c => c.WeaponEf,c=>c.Post)
		//		.Mapper(c => c.PropertyA, c => c.PropertyB,c=>c.PropertyC)
		//		.ToListAsync();
		//}
		//public async override Task<List<WeaponInfo>> QueryAsync(Expression<Func<WeaponInfo, bool>> func)
		//{
		//	return await base.Context.Queryable<WeaponInfo>()
		//		.Where(func)
		//		.Mapper(c => c.Weapon, c => c.WeaponEf,c => c.Post)
		//		.Mapper(c => c.PropertyA, c => c.PropertyB, c => c.PropertyC)
		//		.ToListAsync();
		//}
	}
}
