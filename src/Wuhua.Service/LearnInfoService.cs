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
	public class LearnInfoService : BaseService<LearnInfo>, ILearnInfoService
	{
		private readonly ILearnInfoRepository _iLearnInfoRepository;
		public LearnInfoService(ILearnInfoRepository repository)
		{
			base._iBaseRepository = _iLearnInfoRepository = repository;

		}
	}
}
