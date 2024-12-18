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
using Syncfusion.Windows.Controls.Input;
using XAct;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedService _sharedService;  
    private readonly AccountStore _accountStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    public BaseViewModel? CurrentModalViewModel => _modalNavigationStore.CurrentModalViewModel;
    //notify
    private ObservableCollection<object> _notifyCardViewModels = new ObservableCollection<object>();
    public ObservableCollection<object> NotifyCardViewModels {
        get => _notifyCardViewModels;
        set {
            if (_notifyCardViewModels != value) {
                _notifyCardViewModels.CollectionChanged -= OnGoalNotifyViewModelsChanged;
                _notifyCardViewModels = value;
                _notifyCardViewModels.CollectionChanged += OnGoalNotifyViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoNotify));
            }
        }
    }
    private void OnGoalNotifyViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        OnPropertyChanged(nameof(HasNoNotify));
    }
    public bool HasNoNotify => !NotifyCardViewModels.Any();
    public bool IsModalOpen => _modalNavigationStore.IsOpen;

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


    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;

        _sharedService.TriggerActionNotification += LoadNotify;

        NotifyCardViewModels = new ObservableCollection<object>();

        DashBoardNavigateCommand = new NavigateCommand<DashboardViewModel>(serviceProvider);
        ExpenseBookNavigateCommand = new NavigateCommand<ExpenseViewModel>(serviceProvider);
        GoalplanNavigateCommand = new NavigateCommand<GoalplanViewModel>(serviceProvider);
        SettingNavigateCommand = new NavigateCommand<SettingViewModel>(serviceProvider);
        RecurringNavigateCommand = new NavigateCommand<RecurringViewModel>(serviceProvider);

        CloseCommand = new RelayCommand<Window>(CloseWindow);
        WindowMaximum = new RelayCommand<Window>(w => 
                    w.WindowState = w.WindowState == WindowState.Maximized
                                    ? WindowState.Normal
                                    : WindowState.Maximized);
        WindowMinimum = new RelayCommand<Window>(w => w.WindowState = WindowState.Minimized);
        MoveCommand = new RelayCommand<Window>(w => w?.DragMove());
        ExitAccountCommand = new RelayCommand<object>(ExitMain);//logout
        CloseWindowCommand = new RelayCommand<Window>(CloseWindow);//exit
    }
    public void LoadNotify() {
        LoadNotifyGoal();
        LoadNotifyRecurring();
    }
    public void LoadNotifyGoal() {
        try {
            NotifyCardViewModels.Clear();
            var items = DBManager.GetCondition<Goal>(g => g.UserID == int.Parse(_accountStore.UsersID) && g.Deadline >= DateTime.Today);
            foreach (var item in items) {
                DateTime dt = (DateTime)item.StartDay;
                if (item.Reminder.CompareTo("Daily") == 0) {
                    NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                }
                else if (item.Reminder.CompareTo("Weekly") == 0) {
                    if (dt.AddDays(7) == DateTime.Today) {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                }
                else if (item.Reminder.CompareTo("Monthly") == 0) {
                    if (dt.AddMonths(1) == DateTime.Today) {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                }
                else {//yearly
                    if (dt.AddYears(1) == DateTime.Today) {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                }

            }
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void LoadNotifyRecurring() {
        try {
            var recurring = DBManager.GetCondition<RecurringExpense>(re => re.LastTime <= DateOnly.FromDateTime(DateTime.Now) && re.UserID == _accountStore.Users.UserID);
            foreach(var rec in recurring) {

                while (rec.LastTime <= DateOnly.FromDateTime(DateTime.Today)) {

                    if (rec.Frequency.CompareTo("Daily") == 0) {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Weekly") == 0) {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval * 7);
                    }
                    else if (rec.Frequency.CompareTo("Monthly") == 0) {
                        rec.LastTime = rec.LastTime.AddMonths(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Yearly") == 0) {
                        rec.LastTime = rec.LastTime.AddYears(rec.Interval);
                    }

                    if(rec.LastTime <= DateOnly.FromDateTime(DateTime.Today)){
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, rec));
                    }
                }
            }
        }
        catch(Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
        //CurrentModalViewModel.StartUp();
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
}