using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
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
    public bool PopupOpen {
        get => _popupOpen;
        set {
            if (_popupOpen != value) {
                _popupOpen = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _popupOpen;
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
    //hoverTimer
    private DispatcherTimer hoverTimer;
    //popup
    private Popup popupGlobal { get; set; }
    //source popup
    public ObservableCollection<TextBlock> SourceRecurringInfo {
        get => _sourceRecurringInfo;
        set {
            if (value != _sourceRecurringInfo) {
                _sourceRecurringInfo = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<TextBlock> _sourceRecurringInfo ;

    public ICommand UpdateDataContextCalenderButton {  get; set; }
    public ICommand Popup_MouseEnterCommand { get; set; }
    public ICommand Popup_MouseLeaveCommand { get; set; }
    public ICommand LoadedPopupCommand { get; set; }
    public CalendarViewModel(IServiceProvider serviceProvider) {
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        LoadCalendarViewModel();
        
        _recurringStore.TriggerAction += () => LoadDataContextCalenderButton(CalendarGlobal);
        
        UpdateDataContextCalenderButton = new RelayCommand<Calendar>((c) => { CalendarGlobal = c; LoadDataContextCalenderButton(c); });
        Popup_MouseEnterCommand = new RelayCommand<object>(Popup_MouseEnter);
        Popup_MouseLeaveCommand = new RelayCommand<object>(Popup_MouseLeave);
        LoadedPopupCommand = new RelayCommand<Popup>(LoadPopUp);
    }
    public void LoadCalendarViewModel() {
        usr = _accountStore.Users;
        PopupOpen = false;
        SourceRecurringInfo = new();
        hoverTimer = new DispatcherTimer();
        hoverTimer.Interval = TimeSpan.FromSeconds(0.25);
        hoverTimer.Tick += HoverTimer_Tick;
    }
    public void LoadPopUp(Popup? pu = null) {
        popupGlobal = pu ?? new Popup();
    }
    private void Popup_MouseEnter(object? parameter = null) {
        //none
    }
    private void Popup_MouseLeave(object? parameter = null) {
        PopupOpen = false;
    }
    public void HoverTimer_Tick(object sender, EventArgs e) {
        hoverTimer.Stop();
        PopupOpen = true;
    }
    private void DayButton_MouseEnter(object sender, MouseEventArgs e) {
        //load 
        try {
            SourceRecurringInfo.Clear();
            CalendarDayButton cb = sender as CalendarDayButton;
            if (cb != null) {
                DateTime date = (DateTime)cb.DataContext;
                var matchedItems = DayDataContexts
                .Where(item => item.Date.Equals(date) && item.ListInfo.Count != 0)
                .ToList();
                if (matchedItems.Any()) {
                    hoverTimer.Start();
                    foreach (var textBlock in matchedItems
                        .SelectMany(item => item.ListInfo)
                        .Select(info => new TextBlock() { Text = info.Name })) {
                        SourceRecurringInfo.Add(textBlock);
                    }
                }
            }
        }
        catch (Exception ex) { 
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void DayButton_MouseLeave(object sender, MouseEventArgs e) {
        hoverTimer.Stop();
        if (!popupGlobal.IsMouseOver) {
            PopupOpen = false;
        }
    }
    public void LoadDataContextCalenderButton(Calendar calendar)
    {
        try {
            DayDataContexts.Clear();
            ObservableCollection<CalendarDayButton> dayButtons = GetVisualChildren<CalendarDayButton>(calendar);
            var recs = DBManager.GetCondition<RecurringExpense>(re => re.UserID == usr.UserID && re.RecurringExpenseID != null && re.Status.CompareTo("Active") == 0);
            if (recs.Count == 0) {
                foreach (CalendarDayButton dayButton in dayButtons) {
                    dayButton.MouseEnter += DayButton_MouseEnter;
                    dayButton.MouseLeave += DayButton_MouseLeave;
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
                    dayButton.MouseEnter += DayButton_MouseEnter;
                    dayButton.MouseLeave += DayButton_MouseLeave;
                    List<RecurringExpense> info = new();
                    date = (DateTime)dayButton.DataContext;
                    foreach (var rec in recs) {
                        if (rec.Frequency.CompareTo("Daily") == 0) {
                            if (rec.StartDate.AddDays(rec.Interval).Equals(DateOnly.FromDateTime(date)) || rec.StartDate.Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                        else if (rec.Frequency.CompareTo("Weekly") == 0) {
                            if (rec.StartDate.AddDays(rec.Interval * 7).Equals(DateOnly.FromDateTime(date)) || rec.StartDate.Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                        else if (rec.Frequency.CompareTo("Monthly") == 0) {
                            if (rec.StartDate.AddMonths(rec.Interval).Equals(DateOnly.FromDateTime(date)) || rec.StartDate.Equals(DateOnly.FromDateTime(date))) {
                                rec.StartDate = DateOnly.FromDateTime(date);
                                info.Add(rec);
                            }
                        }
                        else if (rec.Frequency.CompareTo("Yearly") == 0) {
                            if (rec.StartDate.AddYears(rec.Interval).Equals(DateOnly.FromDateTime(date)) || rec.StartDate.Equals(DateOnly.FromDateTime(date))) {
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
                        ListInfo = info,
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