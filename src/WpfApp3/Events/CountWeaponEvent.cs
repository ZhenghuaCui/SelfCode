using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using Wuhua.Main.Data;
using Wuhua.Model;

namespace Wuhua.Main.Events
{
    public class CountWeaponEvent : PubSubEvent<List<IncreInfo>>
    {
    }
}
