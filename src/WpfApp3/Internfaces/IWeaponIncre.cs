using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Data;
using WpfApp3.ViewModels;
using Wuhua.Model;

namespace Wuhua.Main.Internfaces
{
    public interface IWeaponIncre
    {
        void ClearAll();
        List<ShowIncreInfo> GetIncre(SkillItem skillItem);
    }
}
