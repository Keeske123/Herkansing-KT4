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
    public class NotConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                return !(bool)value;
            }
            return value;
        }
    }

    
}
