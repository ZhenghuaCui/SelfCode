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
	public class IncreInfoService : BaseService<IncreInfo>, IIncreInfoService
	{
		private readonly IIncreInfoRepository _iIncreInfoRepository;
		public IncreInfoService(IIncreInfoRepository repository)
		{
			base._iBaseRepository = repository;
			_iIncreInfoRepository = repository;
		}
	}
}
