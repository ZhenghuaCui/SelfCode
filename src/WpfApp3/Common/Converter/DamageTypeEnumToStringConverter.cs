using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp3.Common.Converter
{
	/// <summary>
	/// 枚举值转枚举名称 （parameter传枚举类型）
	/// </summary>
	[ValueConversion(typeof(DamageType), typeof(string))]
    public class DamageTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                var key = CommonStaticSource.DamageTypeDic[(DamageType)value];
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
