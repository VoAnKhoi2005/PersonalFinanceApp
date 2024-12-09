using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PersonalFinanceApp.Src.etc
{
    public class ProgressBarWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;
            double maximum = 100; // Default maximum value
            return (progress / maximum) * 100; // Scale the width based on the value and maximum
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // Not needed for this example
        }

    }
}
