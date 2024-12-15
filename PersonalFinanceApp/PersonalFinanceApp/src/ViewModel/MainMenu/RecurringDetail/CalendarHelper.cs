using PersonalFinanceApp.Src.View;
using System.Windows;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public static class CalendarHelper
{
    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.RegisterAttached(
            "ViewModel",
            typeof(CalendarButtonViewModel),
            typeof(CalendarHelper),
            new PropertyMetadata(null));

    public static void SetViewModel(DependencyObject element, CalendarButtonViewModel value)
    {
        element.SetValue(ViewModelProperty, value);
    }

    public static CalendarButtonViewModel GetViewModel(DependencyObject element)
    {
        return (CalendarButtonViewModel)element.GetValue(ViewModelProperty);
    }
}