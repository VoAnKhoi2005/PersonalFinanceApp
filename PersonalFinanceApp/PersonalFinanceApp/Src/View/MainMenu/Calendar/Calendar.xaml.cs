using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.MainMenu;

namespace PersonalFinanceApp.Src.View;

public partial class Calendar : UserControl
{
    private DispatcherTimer hoverTimer;
    public Calendar()
    {
        InitializeComponent();
        hoverTimer = new DispatcherTimer();
        hoverTimer.Interval = TimeSpan.FromSeconds(0.5); 
        hoverTimer.Tick += HoverTimer_Tick;
    }
    private void UpdateDataContextCalenderButton(object sender, RoutedEventArgs e) {
        if (sender is System.Windows.Controls.Calendar calendar && calendar.DataContext is CalendarViewModel viewModel) {
            viewModel.LoadDataContextCalenderButton(calendar);
        }
    }

    private void CalendarDayButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        hoverTimer.Start();
    }

    private void CalendarDayButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        hoverTimer.Stop();      // Dừng timer
        DayPopup.IsOpen = false; // Đóng Popup
    }
    private void HoverTimer_Tick(object sender, EventArgs e)
    {
        hoverTimer.Stop();      // Dừng timer
        DayPopup.IsOpen = true; // Mở Popup
    }
}