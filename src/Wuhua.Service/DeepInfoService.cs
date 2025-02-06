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
	public class DeepInfoService : BaseService<DeepInfo>, IDeepInfoService
	{
		private readonly IDeepInfoRepository _iDeepInfoRepository;
		public DeepInfoService(IDeepInfoRepository repository)
		{
			base._iBaseRepository = repository;
			_iDeepInfoRepository = repository;
		}
	}
}
