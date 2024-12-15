using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using PersonalFinanceApp.Src.View;
using Calendar = System.Windows.Controls.Calendar;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class CalendarViewModel : BaseViewModel
{
    public List<CalendarButtonViewModel> DayDataContexts { get; set; } = new List<CalendarButtonViewModel>();

    public CalendarViewModel(IServiceProvider serviceProvider) { }

    public void LoadDataContextCalenderButton(Calendar calendar)
    {
        DayDataContexts.Clear();

        ObservableCollection<CalendarDayButton> dayButtons = GetVisualChildren<CalendarDayButton>(calendar);
        foreach (CalendarDayButton dayButton in dayButtons)
        {
            DateTime date = (DateTime)dayButton.DataContext;
            CalendarButtonViewModel calendarDayViewModel = new CalendarButtonViewModel
            {
                Date = date
            };
            DayDataContexts.Add(calendarDayViewModel);
            CalendarHelper.SetViewModel(dayButton, calendarDayViewModel);
        }
    }

    private ObservableCollection<T> GetVisualChildren<T>(DependencyObject parent) where T : DependencyObject
    {
        ObservableCollection<T> children = new ObservableCollection<T>();
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T typedChild)
            {
                children.Add(typedChild);
            }

            foreach (var descendant in GetVisualChildren<T>(child))
            {
                children.Add(descendant);
            }
        }
        return children;
    }
}