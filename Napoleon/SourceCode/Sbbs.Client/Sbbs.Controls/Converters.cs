/************************************************
 * FileName: BoolToVisibleConverter.cs
 * Document-related:
 * Module: Sbbs.Controls
 * Application: Sbbs Client
 * Author: Peter Chen
 * Created on: 02-09-2013
 *************************************************/

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Sbbs.Controls
{
    /// <summary>
    /// Convert boolean value to visible
    /// </summary>
    public class BoolToVisibleConverter : IValueConverter
    {
        #region IValueConverter Members
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? Visibility.Visible : Visibility.Collapsed);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }
        #endregion
    }
    
    /// <summary>
    /// Convert boolean value to !boolean value
    /// </summary>
    public class BoolConverter : IValueConverter
    {
        #region IValueConverter Members
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? false : true);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
        #endregion
    }

    /// <summary>
    /// Convert boolean value to invisible
    /// </summary>
    public class BoolToInvisibleConverter : IValueConverter
    {
        #region IValueConverter Members
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? Visibility.Collapsed : Visibility.Visible);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Collapsed;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class StampDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            date = date.AddSeconds((int)value).ToLocalTime();
            DateTime now = DateTime.Now;

            if (date.Year == now.Year && date.Month == now.Month && date.Day == now.Day)
                return date.ToString("HH:mm", culture);
            else if (date.Year == now.Year)
                return date.ToString("M月d日 HH:mm", culture);
            else
                return date.ToString("yyyy年M月d日", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IndexToFloorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string floor = string.Empty;

            switch ((int)value)
            {
                case 0:
                    {
                        floor = "楼主";
                        break;
                    }
                case 1:
                    {
                        floor = "沙发";
                        break;
                    }
                case 2:
                    {
                        floor = "板凳";
                        break;
                    }
                case 3:
                    {
                        floor = "地板";
                        break;
                    }
                default:
                    {
                        floor = value.ToString() + "楼";
                        break;
                    }
            }

            return floor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
