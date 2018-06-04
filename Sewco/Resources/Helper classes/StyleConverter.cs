using System;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sewco.Resources.Helper_classes
{
    class StyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string dataValue = values[0] as string;
            Style firstStyle = values[1] as Style;
            Style secondStyle = values[2] as Style;
            Style defaultStyle = values[3] as Style;

            if (dataValue == "style1")
            {
                return firstStyle;
            }
            else if (dataValue == "style2")
            {
                return secondStyle;
            }
            else
            {
                return defaultStyle;
            }
 
            //return dataValue.Equals("cust2") ? firstStyle : secondStyle;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
