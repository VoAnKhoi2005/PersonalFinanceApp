using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using PersonalFinanceApp.Src.View;
using Calendar = System.Windows.Controls.Calendar;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class CalendarViewModel : BaseViewModel
{
    public List<CalendarDayViewModel> DayDataContexts { get; set; } = new List<CalendarDayViewModel>();

    public CalendarViewModel(IServiceProvider serviceProvider) { }

    public void LoadDataContextCalenderButton(Calendar calendar)
    {
        List<CalendarDayButton> dayButtons = GetVisualChildren<CalendarDayButton>(calendar).ToList();

        foreach (CalendarDayButton dayButton in dayButtons)
        {
            DateTime date = (DateTime)dayButton.DataContext;
            CalendarDayViewModel calendarDayViewModel = new CalendarDayViewModel
            {
                Date = date
            };
            DayDataContexts.Add(calendarDayViewModel);
            dayButton.DataContext = calendarDayViewModel;
        }
    }

    private IEnumerable<T> GetVisualChildren<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (var descendant in GetVisualChildren<T>(child))
            {
                yield return descendant;
            }
        }
    }
}