using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp3.Common
{
    /// <summary>
    /// 枚举值转枚举名称 （parameter传枚举类型）
    /// </summary>
    [ValueConversion(typeof(Occupation), typeof(string))]
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                var key = CommonStaticSource.OccupationDic[(Occupation)value];
                return key;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
