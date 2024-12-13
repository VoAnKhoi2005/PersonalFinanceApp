using System.Globalization;
using System.Windows.Data;


namespace PersonalFinanceApp.ViewModel.Converter;
public class DateTimeToDayNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
        //switch (value)
        //{
        //    case "M": return "Monday"; break;
        //    case "T": return "Monday"; break;
        //    case "W": return "Monday"; break;
        //    case "M": return "Monday"; break;
        //    case "M": return "Monday"; break;
        //    default: return "";
        //}
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
