using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalEditViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;
    #region Properties
    //name
    public string NameEditGoal {
        get => _nameGoal;
        set {
            if (_nameGoal != value) {
                _nameGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameGoal;
    //target
    public string TargetEditGoal {
        get => _targetGoal;
        set {
            if (_targetGoal != value) {
                _targetGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _targetGoal;
    //current amount
    public string CurrentAmountEditGoal {
        get => _currentAmountGoal;
        set {
            if (_currentAmountGoal != value) {
                _currentAmountGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _currentAmountGoal;
    //resource
    public string ResourceEditGoal {
        get => _resourceGoal;
        set {
            if (_resourceGoal != value) {
                _resourceGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceGoal;
    //reminder
    public string ReminderEditGoal {
        get => _reminderGoal;
        set {
            if (_reminderGoal != value) {
                _reminderGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _reminderGoal;
    //deadline
    public DateTime? DeadlineEditGoal {
        get => _deadlineGoal;
        set {
            if (_deadlineGoal != value) {
                _deadlineGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private DateTime? _deadlineGoal = DateTime.Now;
    //status
    public string StatusEditGoal {
        get => _statusGoal;
        set {
            if (_statusGoal != value) {
                _statusGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _statusGoal;
    //Description
    public string DescriptionEditGoal {
        get => _descriptionEditGoal;
        set {
            if (_descriptionEditGoal != value) {
                _descriptionEditGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionEditGoal;
    //category
    public string CategoryEditGoal {
        get => _categoryGoal;
        set {
            if (_categoryGoal != value) {
                _categoryGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryGoal;
    public ObservableCollection<string> _itemsGoalEdit = new ObservableCollection<string> { };
    public ObservableCollection<string> ItemsGoalEdit {
        get => _itemsGoalEdit;
        set {
            if (_itemsGoalEdit != value) {
                _itemsGoalEdit = value;
                OnPropertyChanged();
            }
        }
    }
    public string SelectedItemEdit {
        get => _selectedItemEdit;
        set {
            if (value != null && value.CompareTo("<New>") == 0) {
                CreateCategoryCommand.Execute(this);
                _selectedItemEdit = value;
                OnPropertyChanged();
            }
            else {
                _selectedItemEdit = value;
                OnPropertyChanged();
            }

        }
    }
    private string _selectedItemEdit;
    #endregion
    public ICommand CancelEditGoalCommand { get; set; }
    public ICommand ConfirmEditGoalCommand { get; set; }
    public ICommand CreateCategoryCommand {  get; set; }
    public ICommand LoadDataAddNewGoalCommand { get; set; }

    public GoalEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();

        CreateCategoryCommand = new NavigateModalCommand<GoalAddNewCategoryViewModel>(serviceProvider);

        LoadDataAddNewGoalCommand = new RelayCommand<object>(LoadData);
        

        CancelEditGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditGoalCommand = new RelayCommand<object>(ConfirmEditGoal);
    }
    public void LoadData(object parameter) {
        LoadItemSourceCategoryGoal();
        loadItem();
        if (_goalStore.NewCategory != null) {
            CategoryEditGoal = _goalStore.NewCategory;
            _goalStore.NewCategory = null;
        }
    }
    public void LoadItemSourceCategoryGoal() {
        ItemsGoalEdit.Clear();
        var item = DBManager.GetAll<GoalCategory>();
        ItemsGoalEdit.Add("<New>");
        foreach (var it in item) {
            ItemsGoalEdit.Add(it.Name);
        }
    }
    public void loadItem() {
        var item = DBManager.GetFirst<Goal>(g => int.Parse(_goalStore.GoalID) == g.GoalID);
        NameEditGoal = item.Name;
        TargetEditGoal = item.Target.ToString();
        CurrentAmountEditGoal = item.CurrentAmount.ToString();
        //goalEdit.Reminder = "Daily";
        DeadlineEditGoal = item.Deadline;
        DescriptionEditGoal = item.Description;
        CategoryEditGoal = item.CategoryName;
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditGoal(object sender) {
        //add data to database
        var goalID = _goalStore.GoalID;
        Goal goalEdit = new();
        if(goalID != null) goalEdit = DBManager.GetFirst<Goal>(g => g.GoalID == int.Parse(goalID));
        if (goalEdit != null) {
            goalEdit.Name = NameEditGoal;
            goalEdit.Target = long.Parse(TargetEditGoal);
            goalEdit.CurrentAmount = long.Parse(CurrentAmountEditGoal);
            goalEdit.Reminder = "Daily";
            goalEdit.Deadline = DeadlineEditGoal;
            goalEdit.Status = (long.Parse(TargetEditGoal) <= long.Parse(CurrentAmountEditGoal)) ? "Completed" : "Active";
            goalEdit.Description = DescriptionEditGoal;
            goalEdit.CategoryName = CategoryEditGoal;
        }
        bool update = DBManager.Update<Goal>(goalEdit);
        _modalNavigationStore.Close();
    }
}
