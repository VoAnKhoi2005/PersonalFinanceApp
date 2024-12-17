using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Text.RegularExpressions;

namespace PersonalFinanceApp.Src.etc;
// Converter để định dạng số thành chuỗi có khoảng trắng
public class NumberToFormattedStringConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is string input && decimal.TryParse(input, out decimal number)) {
            // Định dạng số thành dạng "1 000 000"
            return number.ToString("N0", CultureInfo.InvariantCulture).Replace(",", " ");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is string input) {
            // Loại bỏ khoảng trắng để chuyển ngược thành số
            input = input.Replace(" ", "");
            if (decimal.TryParse(input, out decimal number)) {
                return number.ToString();
            }
        }
        return value;
    }
}

// TextBox với khả năng tự động định dạng và chỉ cho phép nhập số
public class FormattedTextBox : TextBox {
    public static readonly DependencyProperty FormattedTextProperty = DependencyProperty.Register(
        "FormattedText",
        typeof(string),
        typeof(FormattedTextBox),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFormattedTextChanged));

    private static void OnFormattedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is FormattedTextBox textBox) {
            textBox.Text = e.NewValue as string;
        }
    }

    protected override void OnTextChanged(TextChangedEventArgs e) {
        base.OnTextChanged(e);

        // Chỉ cho phép nhập số
        string newText = Regex.Replace(Text, "[^0-9]", "");
        if (newText != Text) {
            int caretIndex = CaretIndex - (Text.Length - newText.Length);
            Text = newText;
            CaretIndex = Math.Max(0, caretIndex);
        }

        // Cập nhật giá trị gốc khi TextBox thay đổi
        FormattedText = Text;
    }

    public string FormattedText {
        get => (string)GetValue(FormattedTextProperty);
        set => SetValue(FormattedTextProperty, value);
    }
}
