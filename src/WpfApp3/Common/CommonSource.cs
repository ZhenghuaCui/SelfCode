using System;
using System.Collections.Generic;
using System.Linq;
using Wuhua.IService;
using Wuhua.Model;
using Wuhua.NLog;

namespace WpfApp3.Common
{
    public class CommonSource
	{
		public List<WeaponInfo> WeaponInfos = new List<WeaponInfo>();
		public List<IncreInfo> IncreInfos = new List<IncreInfo>();


        public Dictionary<string, IncreInfo> IncreDic;


        IIncreInfoService _infoService = null;
		public CommonSource(IIncreInfoService infoService)
		{
            _infoService = infoService;
			Init();
        }

        private async void Init()
        {
            try
            {
                var result = await _infoService.QueryAsync();
                IncreDic = result.ToDictionary(key => key.IncreName, value => value);

            }
            catch (Exception ex) {
                LoggerHelper.Logger.Error(ex.Message);
            }

        }
    }
}
