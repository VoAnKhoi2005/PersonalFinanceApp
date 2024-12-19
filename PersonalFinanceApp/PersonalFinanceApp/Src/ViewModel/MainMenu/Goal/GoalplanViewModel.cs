using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class GoalplanViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;
    private readonly AccountStore _accountStore;
    #region Properties
    //source status 
    public ObservableCollection<CheckBoxStatus> SourceStatus {
        get => _sourceStatus;
        set {
            if (_sourceStatus != value) {
                _sourceStatus = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<CheckBoxStatus> _sourceStatus = new();
    //source status true
    public ObservableCollection<CheckBoxStatus> StatusFilter {
        get => _statusFilter;
        set {
            if (_statusFilter != value) {
                _statusFilter = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<CheckBoxStatus> _statusFilter = new();

    //data filter
    public string DataFilterGoal {
        get => _dataFilterGoal;
        set {
            if (_dataFilterGoal != value) {
                _dataFilterGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _dataFilterGoal;
    #endregion
    #region Command
    public ICommand AddNewGoalCommand { get; set; }
    public ICommand RefreshGoalCommand { get; set; }
    public ICommand TextChangedFilterCommand { get; set; }
    public ICommand FilterStatusCommand { get; set; }
    #endregion

    private ObservableCollection<GoalplanCardViewModel> _goalplanCardViewModels = new ObservableCollection<GoalplanCardViewModel>();
    public ObservableCollection<GoalplanCardViewModel> GoalplanCardViewModels
    {
        get => _goalplanCardViewModels;
        set
        {
            if (_goalplanCardViewModels != value)
            {
                _goalplanCardViewModels.CollectionChanged -= OnGoalplanCardViewModelsChanged;
                _goalplanCardViewModels = value;
                _goalplanCardViewModels.CollectionChanged += OnGoalplanCardViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoGoal));
            }
        }
    }
    private void OnGoalplanCardViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNoGoal));
    }

    public bool HasNoGoal => !GoalplanCardViewModels.Any();

    


    public GoalplanViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        GoalplanCardViewModels = new ObservableCollection<GoalplanCardViewModel>();

        AddNewGoalCommand = new NavigateModalCommand<GoalplanAddNewViewModel>(serviceProvider);

        RefreshGoalCommand = new RelayCommand<object>(LoadedGoal);

        LoadedGoal();

        _goalStore.TriggerAction += Reload;

        SourceStatus.Add(new CheckBoxStatus(new CheckBox() { Content = "Active", IsChecked = false }));
        SourceStatus.Add(new CheckBoxStatus(new CheckBox() { Content = "Completed", IsChecked = false }));
        SourceStatus.Add(new CheckBoxStatus(new CheckBox() { Content = "Canceled", IsChecked = false }));

        //auto
        TextChangedFilterCommand = new RelayCommand<object>(FilterTextChanged);
        FilterStatusCommand = new RelayCommand<object>(FilterStatus);
    }
    public void MatchCase(object? parameter = null) {
        ReviewStatus();
        if (DataFilterGoal == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = $@"{Regex.Escape(DataFilterGoal)}";
        Filter(pattern);
    }
    public void Filter(string pattern) {
        var userID = int.Parse(_accountStore.UsersID);

        GoalplanCardViewModels.Clear();

        List<Goal> items = new List<Goal>();

        if (StatusFilter.Count == 0) {
            items = DBManager.GetCondition<Goal>(exp => exp.UserID == userID);
        }
        else {
            foreach (var item in StatusFilter) {
                var i = DBManager.GetCondition<Goal>(e => e.UserID == userID && item.Name.CompareTo(e.Status) == 0);
                if (i != null) {
                    foreach (var j in i) {
                        items.Add(j);
                    }
                }
            }
        }

        if (items == null) return;


        foreach (var item in items) {
            if (Regex.IsMatch(item.Name, pattern)) {
                GoalplanCardViewModels.Add(new GoalplanCardViewModel(_serviceProvider, item) { ID = item.GoalID.ToString() });
            }
        }
        
    }
    //status
    public void FilterStatus(object? parameter = null) {
        ReviewStatus();
        FilterTextChanged();
    }
    //filter when textbox changed
    public void FilterTextChanged(object? parameter = null) {
        try {
            if ((DataFilterGoal == null || DataFilterGoal.Trim() == "") && (StatusFilter.Count == 0)) {
                //filter when none data andd none status
                LoadedGoal();
                return;
            }
            else if ((DataFilterGoal == null || DataFilterGoal.Trim() == "") && (StatusFilter.Count != 0)) {
                //filter when none data and have status
                LoadFilterHaveStatus();
                return;
            }
            if (DataFilterGoal != "") {
                //filter
                MatchCase();
            }
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void LoadFilterHaveStatus() {
        GoalplanCardViewModels.Clear();

        List<Goal> goals = DBManager.GetCondition<Goal>(g => g.User.UserID == int.Parse(_accountStore.UsersID));

        HashSet<string> status = new HashSet<string>(StatusFilter.Select(c => c.Name));

        List<Goal> filteredgoal = goals.Where(e => status.Contains(e.Status)).ToList();

        foreach (var goal in filteredgoal) {
            GoalplanCardViewModels.Add(new GoalplanCardViewModel(_serviceProvider, goal) { ID = goal.GoalID.ToString() });
        }
        
    }
    public void ReviewStatus() {
        if (SourceStatus.Count == 0) return;
        StatusFilter.Clear();
        foreach (var item in SourceStatus) {
            if (item.IsChecked == true) {
                StatusFilter.Add(item);
            }
        }
    }
    public void Reload() {
        LoadedGoal();
    }
    private void LoadedGoal(object? parameter = null) {
        //reload data to itemcontrol
        GoalplanCardViewModels.Clear();
        List<Goal> goals = DBManager.GetCondition<Goal>(g => g.User.UserID == int.Parse(_accountStore.UsersID));
        foreach (var goal in goals) {
            GoalplanCardViewModels.Add(new GoalplanCardViewModel(_serviceProvider, goal) { ID = goal.GoalID.ToString()});
        }
    }
    public class CheckBoxStatus {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public CheckBox ckb { get; set; }
        public CheckBoxStatus(CheckBox cb) {
            ckb = cb;
            Name = cb.Content.ToString();
            IsChecked = false;
        }
    }
}