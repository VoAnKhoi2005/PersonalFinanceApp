using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PersonalFinanceApp.ViewModel.Modal;
public class Modal : ContentControl
{
    public static readonly DependencyProperty IsOpenProperty = 
        DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(Modal), new PropertyMetadata((object)false));
    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Modal), new PropertyMetadata(new CornerRadius()));
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    static Modal()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(typeof(Modal)));
        BackgroundProperty.OverrideMetadata(typeof(Modal), new FrameworkPropertyMetadata(CreateDefaultBackground()));
    }

    private static object CreateDefaultBackground()
    {
        SolidColorBrush defaultBackground = new SolidColorBrush(Colors.Black)
        {
            Opacity = 0.5
        };
        return defaultBackground;
    }
}