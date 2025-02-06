using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp3.Data;
using WpfApp3.ViewModels;
using Wuhua.Main.Data;
using Wuhua.Model;

namespace WpfApp3.Internfaces
{
    public interface IRoles
	{
		List<string> GetOutPut();
		void Register(RolesInfoControlViewModel vm,List<Monster>monsters, ObservableCollection<string> ResultList,List<IncreInfo> increInfoLights);
	}
}
