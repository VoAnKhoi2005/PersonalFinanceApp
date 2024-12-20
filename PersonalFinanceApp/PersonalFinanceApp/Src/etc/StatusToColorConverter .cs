using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PersonalFinanceApp.Src.etc;
public class StatusToColorConverter : IValueConverter {
    public SolidColorBrush color1 = new SolidColorBrush(Color.FromRgb(21, 249, 17));
    public SolidColorBrush color2 = new SolidColorBrush(Color.FromRgb(223, 58, 32));
    public SolidColorBrush color3 = new SolidColorBrush(Color.FromRgb(182, 54, 220));
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is string status) {
            return status switch {
                "Completed" => color1,
                "Canceled" => color2,
                "Active" => color3,
                _ => Brushes.Black
            };
        }
        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
