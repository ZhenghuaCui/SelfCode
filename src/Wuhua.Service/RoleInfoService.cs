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
	public class RoleInfoService : BaseService<RoleInfo>, IRoleInfoService
	{
		private readonly IRoleInfoRepository _iRoleInfoRepository;
		public RoleInfoService(IRoleInfoRepository repository)
		{
			base._iBaseRepository = repository;
			_iRoleInfoRepository = repository;
		}
	}
}
