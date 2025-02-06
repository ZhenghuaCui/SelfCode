using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using WpfApp3;
using WpfApp3.Common;
using WpfApp3.Data;
using Wuhua.Main.Internfaces;
using Wuhua.Model;

namespace Wuhua.Main.Weapon
{
    public class WeaponBase : IWeaponIncre
    {
        public bool IsGetAll = false;
        public List<SkillItem> SkillList = new List<SkillItem>();
        public List<ShowIncreInfo> IncreInfos = new List<ShowIncreInfo>();
        public Dictionary<string, IncreInfo> IncreDic;
        public WeaponBase(IContainerProvider containerProvider)
        {
            var _commonSource = containerProvider.Resolve<CommonSource>("CommonSource");
            IncreDic = _commonSource.IncreDic;
        }
        public WeaponBase()
        {
            var a = Application.Current as App;
            var containerProvider = (IContainerProvider)a.Container;
            var _commonSource = containerProvider.Resolve<CommonSource>("CommonSource");
            IncreDic = _commonSource.IncreDic;
        
        }

        public void ClearAll()
        {
            SkillList.Clear();
        }

        public virtual List<ShowIncreInfo> GetIncre(SkillItem skillItem) {
            return new List<ShowIncreInfo>();
        }
    }
}
