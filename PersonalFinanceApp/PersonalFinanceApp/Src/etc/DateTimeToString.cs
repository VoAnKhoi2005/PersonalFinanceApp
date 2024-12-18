using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PersonalFinanceApp.Src.etc;
public class DateTimeToString : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is DateTime dateTime) {
            return dateTime.ToString("dd/MM/yy");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        if (DateTime.TryParseExact(value.ToString(), "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)) {
            return dateTime;
        }
        return value;
    }
}
