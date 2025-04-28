using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuhua.IRepository;
using Wuhua.IService;
using Wuhua.Model;

namespace Wuhua.Service
{
	public class WeaponInfoService : BaseService<WeaponInfo>, IWeaponInfoService
	{
		private readonly IWeaponInfoRepository _iWeaponRepository;
		public WeaponInfoService(IWeaponInfoRepository repository)
		{
			base._iBaseRepository = _iWeaponRepository = repository;
		}
	}
}
