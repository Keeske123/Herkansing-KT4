using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using System.Globalization;
using System.ComponentModel;
using System.Resources;
using System.Collections;

namespace Sewco.Resources.Helper_classes
{
    class Converters
    {
        public static string cvObjToStr(object objValue)    // Convert Object to String
        {
            string sReturn = "";
            try
            {
                sReturn = objValue.ToString();
            }
            catch
            {
                sReturn = "NoValue";
            }
            return sReturn;
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        Visibility isVisible;
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            isVisible = new Visibility();

            if (value is bool)
            {
                if ((bool)value)
                    isVisible = Visibility.Visible;
                else
                    isVisible = Visibility.Hidden;
            }
            return isVisible;
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            isVisible = new Visibility();

            if (value is bool)
            {
                if ((bool)value)
                    isVisible = Visibility.Visible;
                else
                    isVisible = Visibility.Hidden;
            }
            return isVisible;
        }
    }
}
