using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wuhua.IRepository;
using Wuhua.Model;

namespace Wuhua.Repository
{
	public class CustomWeaponRepository : BaseRepository<CustomWeaponInfo>, ICustomWeaponRepository
	{
	}
}
