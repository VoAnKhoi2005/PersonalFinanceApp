using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.View;
using PersonalFinanceApp.Src.ViewModel.Stores;
using Calendar = System.Windows.Controls.Calendar;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class CalendarViewModel : BaseViewModel
{
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;

    public User usr {  get; set; }
    public List<CalendarButtonViewModel> DayDataContexts { get; set; } = new List<CalendarButtonViewModel>();
    public Calendar? CalendarGlobal { 
        get => _calendar;
        set {
            if (value != _calendar) {
                _calendar = value; OnPropertyChanged();
            }
        }
    }
    private Calendar? _calendar;

    public ICommand UpdateDataContextCalenderButton {  get; set; }
    public CalendarViewModel(IServiceProvider serviceProvider) {
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        usr = _accountStore.Users;
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        _recurringStore.TriggerAction += () => LoadDataContextCalenderButton(CalendarGlobal);

        UpdateDataContextCalenderButton = new RelayCommand<Calendar>((c) => { CalendarGlobal = c; LoadDataContextCalenderButton(c); });
    }
    public void LoadDataContextCalenderButton(Calendar calendar)
    {
        try {

            DayDataContexts.Clear();

            ObservableCollection<CalendarDayButton> dayButtons = GetVisualChildren<CalendarDayButton>(calendar);
            var recs = DBManager.GetCondition<RecurringExpense>(re => re.UserID == usr.UserID && re.RecurringExpenseID != null);
            if (recs.Count == 0) {
                foreach (CalendarDayButton dayButton in dayButtons) {
                    DateTime date = (DateTime)dayButton.DataContext;
                    CalendarButtonViewModel calendarDayViewModel = new CalendarButtonViewModel {
                        Date = date,
                    };
                    DayDataContexts.Add(calendarDayViewModel);
                    CalendarHelper.SetViewModel(dayButton, calendarDayViewModel);
                }
            }
            else {
                DateTime date = (DateTime)dayButtons[0].DataContext;
                foreach (var rec in recs) {
                    rec.StartDate = CatchNewLoadCalendar(rec, date);
                }
                foreach (CalendarDayButton dayButton in dayButtons) {
                    List<RecurringExpense> info = new();
                    date = (DateTime)dayButton.DataContext;
                    foreach (var rec in recs) {
                        if (rec.Frequency.CompareTo("Daily") == 0) {
                            if (rec.StartDate.AddDays(rec.Interval).Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                        else if (rec.Frequency.CompareTo("Weekly") == 0) {
                            if (rec.StartDate.AddDays(rec.Interval * 7).Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                        else if (rec.Frequency.CompareTo("Monthly") == 0) {
                            if (rec.StartDate.AddMonths(rec.Interval).Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                        else if (rec.Frequency.CompareTo("Yearly") == 0) {
                            if (rec.StartDate.AddYears(rec.Interval).Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                    }
                    int count = info.Count;
                    CalendarButtonViewModel calendarDayViewModel = new CalendarButtonViewModel {
                        Date = date,
                        Info1 = (count >= 1) ? info[0].Name : string.Empty,
                        Info2 = (count >= 2) ? info[1].Name : string.Empty,
                        AdditionalInfo = (count >= 3) ? $"+{count - 2} More" : string.Empty,
                    };
                    DayDataContexts.Add(calendarDayViewModel);
                    CalendarHelper.SetViewModel(dayButton, calendarDayViewModel);
                }
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
    }
    public DateOnly CatchNewLoadCalendar(RecurringExpense rec, DateTime date) {
        while(DateOnly.FromDateTime(date) > rec.StartDate) {
            if (rec.Frequency.CompareTo("Daily") == 0) {
                rec.StartDate = rec.StartDate.AddDays(rec.Interval);
            }
            else if (rec.Frequency.CompareTo("Weekly") == 0) {
                rec.StartDate = rec.StartDate.AddDays(rec.Interval * 7);
            }
            else if (rec.Frequency.CompareTo("Monthly") == 0) {
                rec.StartDate = rec.StartDate.AddMonths(rec.Interval);
            }
            else if (rec.Frequency.CompareTo("Yearly") == 0) {
                rec.StartDate = rec.StartDate.AddYears(rec.Interval);
            }
        }
        return rec.StartDate;
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