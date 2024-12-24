using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedService _sharedService;  
    private readonly AccountStore _accountStore;
    private readonly ThemeStore _themeStore;
    private readonly RecurringStore _recurringStore;

    #region Properties
    public string UserNameAdmin {
        get => _userNameAdmin;
        set {
            _userNameAdmin = value;
            OnPropertyChanged();
        }
    }
    private string _userNameAdmin;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    public BaseViewModel? CurrentModalViewModel => _modalNavigationStore.CurrentModalViewModel;
    public bool IsModalOpen => _modalNavigationStore.IsOpen;

    public bool HasNoNotify => !NotifyCardViewModels.Any();
    public bool RedPoint => NotifyCardViewModels.Any();

    private ObservableCollection<object> _notifyCardViewModels = new ObservableCollection<object>();
    public ObservableCollection<object> NotifyCardViewModels
    {
        get => _notifyCardViewModels;
        set
        {
            if (_notifyCardViewModels != value)
            {
                _notifyCardViewModels.CollectionChanged -= OnNotifyViewModelsChanged;
                _notifyCardViewModels = value;
                _notifyCardViewModels.CollectionChanged += OnNotifyViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoNotify));
                OnPropertyChanged(nameof(RedPoint));
            }
        }
    }
    private void OnNotifyViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNoNotify));
        OnPropertyChanged(nameof(RedPoint));
    }
    #endregion Properties

    #region Commands
    public ICommand DashBoardNavigateCommand { get; set; }
    public ICommand ExpenseBookNavigateCommand { get; set; }
    public ICommand GoalplanNavigateCommand { get; set; }
    public ICommand SettingNavigateCommand { get; set; }
    public ICommand RecurringNavigateCommand { get; set; }
    public ICommand CloseCommand { get; set; }
    public ICommand WindowMinimum { get; set; }
    public ICommand WindowMaximum { get; set; }
    public ICommand MoveCommand { get; set; }
    public ICommand ExitAccountCommand { get; set; }
    public ICommand CloseWindowCommand { get; set; }
    #endregion Commands


    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _themeStore = serviceProvider.GetRequiredService<ThemeStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;

        //Notification
        LoadNotify();
        _recurringStore.TriggerUpload += Upload;
        //User
        UserNameAdmin = _accountStore.Users.Username;

        //load theme
        _themeStore.Notify();

        DashBoardNavigateCommand = new NavigateCommand<DashboardViewModel>(serviceProvider);
        ExpenseBookNavigateCommand = new NavigateCommand<ExpenseViewModel>(serviceProvider);
        GoalplanNavigateCommand = new NavigateCommand<GoalplanViewModel>(serviceProvider);
        SettingNavigateCommand = new NavigateCommand<SettingViewModel>(serviceProvider);
        RecurringNavigateCommand = new NavigateCommand<RecurringViewModel>(serviceProvider);
       
        CloseCommand = new Command.RelayCommand<Window>(CloseWindow);
        WindowMaximum = new Command.RelayCommand<Window>(w => 
                                                             w.WindowState = w.WindowState == WindowState.Maximized
                                                                 ? WindowState.Normal
                                                                 : WindowState.Maximized);
        WindowMinimum = new Command.RelayCommand<Window>(w => w.WindowState = WindowState.Minimized);
        MoveCommand = new Command.RelayCommand<Window>(w => w?.DragMove());
        ExitAccountCommand = new Command.RelayCommand<object>(ExitMain);//logout
        CloseWindowCommand = new Command.RelayCommand<Window>(CloseWindow);//exit
    }

    public void CloseWindow(Window window) {
        window?.Close();
        _sharedService.w?.Close();
    }
    public void ExitMain(object? parameter = null) {
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginNewAccountViewModel>();

        if (_sharedService.w != null) {
            _sharedService.m = (View.MainWindow?)Application.Current.MainWindow;
            Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            Application.Current.MainWindow = _sharedService.w;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            Application.Current.MainWindow.Activate();
        }
    }

    private void OnCurrentModalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsModalOpen));
        
    }
    public override void StartUp() {
        base.StartUp();
    }
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
        base.Dispose();
        _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentModalViewModelChanged -= OnCurrentModalViewModelChanged;
    }

    #region Notification
    public void Upload() {
        NotifyCardViewModels.Clear();
        foreach(var item in _recurringStore.ShareRecurring) {
            NotifyCardViewModels.Add(item);
        }
    }
    public void LoadNotify()
    {
        NotifyCardViewModels.Clear();
        //NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, new Goal()));
        //NotifyCardViewModels.Add(new NotificationRecurringCard(_serviceProvider, new RecurringExpense()));
        LoadNotifyGoal();
        LoadNotifyRecurring();
        _recurringStore.ShareRecurring = new(NotifyCardViewModels);
    }

    public void LoadNotifyGoal()
    {
        try
        {
            var items = DBManager.GetCondition<Goal>(g => g.UserID == _accountStore.Users.UserID && g.Deadline >= DateTime.Today);
            foreach (var item in items)
            {
                DateTime dt = (DateTime)item.StartDay;
                while (dt <= DateTime.Today) {
                    if (item.Reminder.CompareTo("Daily") == 0) {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                    else if (item.Reminder.CompareTo("Weekly") == 0) {
                        if (dt.AddDays(7) == DateTime.Today) {
                            NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                        }
                        else {
                            dt = dt.AddDays(7);
                        }
                    }
                    else if (item.Reminder.CompareTo("Monthly") == 0) {
                        if (dt.AddMonths(1) == DateTime.Today) {
                            NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                        }
                        else {
                            dt = dt.AddMonths(1);
                        }
                    }
                    else {
                        //yearly
                        if (dt.AddYears(1) == DateTime.Today) {
                            NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                        }
                        else {
                            dt = dt.AddYears(1);
                        }
                    }
                }
                
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void LoadNotifyRecurring()
    {
        try
        {
            var recurring = DBManager.GetCondition<RecurringExpense>(re => re.LastTime <= DateOnly.FromDateTime(DateTime.Now) && re.UserID == _accountStore.Users.UserID);
            foreach (var rec in recurring)
            {
                rec.Expenses = DBManager.GetCondition<Expense>(e => e.UserID == rec.UserID && e.RecurringExpenseID == rec.RecurringExpenseID);
                if (rec.Expenses.Count == 0) continue;
                while (rec.LastTime <= DateOnly.FromDateTime(DateTime.Today))
                {
                    if (rec.Frequency.CompareTo("Daily") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Weekly") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval * 7);
                    }
                    else if (rec.Frequency.CompareTo("Monthly") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddMonths(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Yearly") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddYears(rec.Interval);
                    }

                    if (rec.LastTime <= DateOnly.FromDateTime(DateTime.Today))
                    {
                        NotifyCardViewModels.Add(new NotificationRecurringCard(_serviceProvider, rec));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    #endregion Notification
}