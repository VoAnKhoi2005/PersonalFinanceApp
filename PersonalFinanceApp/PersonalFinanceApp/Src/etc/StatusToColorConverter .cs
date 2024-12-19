using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PersonalFinanceApp.Src.etc;
public class StatusToColorConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is string status) {
            return status switch {
                "Completed" => Brushes.Green,
                "Canceled" => Brushes.Red,
                "Active" => Brushes.Orange,
                _ => Brushes.Black
            };
        }
        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
