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
	public class CustomWeaponService: BaseService<CustomWeaponInfo>, ICustomWeaponService
	{
		private readonly ICustomWeaponRepository _iCustomWeaponRepository;
		public CustomWeaponService(ICustomWeaponRepository repository)
		{
			base._iBaseRepository = repository;
			_iCustomWeaponRepository = repository;
		}
	}
}
