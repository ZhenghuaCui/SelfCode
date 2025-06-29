﻿using System;
using System.Windows;
using System.Windows.Data;

namespace WpfApp3.Common.Converter
{
    public class InverseVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be  Visibility");
			if (value.Equals(Visibility.Visible))
			{
                return Visibility.Collapsed;

            }
			else
			{
                return Visibility.Visible;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

}
