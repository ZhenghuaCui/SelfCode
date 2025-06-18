using System.Collections.Generic;
using System.Linq;
using WpfApp3.Data;

namespace Wuhua.Main.Common.Extensions
{
    public static class ShowIncreInfoExtensions
    {
        public static int SumNumericalValues(this IEnumerable<ShowIncreInfo> list)
            => list.Sum(i => int.TryParse(i.IncreNum, out var num) ? num : 0);
    }
}
