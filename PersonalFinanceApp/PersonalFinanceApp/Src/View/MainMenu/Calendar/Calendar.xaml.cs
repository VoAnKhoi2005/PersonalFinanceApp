using System.Windows;
using System.Windows.Controls;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.MainMenu;

namespace PersonalFinanceApp.Src.View;

public partial class Calendar : UserControl
{

    public Calendar()
    {
        InitializeComponent();
    }

    private void UpdateDataContextCalenderButton(object sender, RoutedEventArgs e) {
        if (sender is System.Windows.Controls.Calendar calendar && calendar.DataContext is CalendarViewModel viewModel) {
            viewModel.LoadDataContextCalenderButton(calendar);
        }
    }
}