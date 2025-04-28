using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Data;

namespace Wuhua.Main.Data
{
     public class WeaponBuffGroups
    {
        public List<ShowIncreInfo> AtkIncres { get; } = new();
        public List<ShowIncreInfo> DefIncres { get; } = new();
        public List<ShowIncreInfo> CraIncres { get; } = new();
        public List<ShowIncreInfo> AllHurtList { get; } = new();
        public List<ShowIncreInfo> AtkTypeList { get; } = new();
        public List<ShowIncreInfo> DamageTypeList { get; } = new();
        public List<ShowIncreInfo> TargetTypeList { get; } = new();
        public List<ShowIncreInfo> AllVulList { get; } = new();
        public List<ShowIncreInfo> AtkTypeVulList { get; } = new();
        public List<ShowIncreInfo> DamageTypeVulList { get; } = new();
        public List<ShowIncreInfo> TargetTypeVulList { get; } = new();
        public void ClearAll()
        {
            AtkIncres.Clear();
            DefIncres.Clear();
            CraIncres.Clear();
            AllHurtList.Clear();
            AtkTypeList.Clear();
            DamageTypeVulList.Clear();
            DamageTypeList.Clear();
            TargetTypeList.Clear();
            AllVulList.Clear();
            AtkTypeVulList.Clear();
            TargetTypeVulList.Clear();
        }

    }
}
