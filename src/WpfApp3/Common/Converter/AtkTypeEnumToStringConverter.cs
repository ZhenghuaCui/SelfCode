using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp3.Common.Converter
{
	/// <summary>
	/// 枚举值转枚举名称 （parameter传枚举类型）
	/// </summary>
	[ValueConversion(typeof(AtkType), typeof(string))]
    public class AtkTypeEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                var key = CommonStaticSource.AtkTypeDic[(AtkType)value];
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
