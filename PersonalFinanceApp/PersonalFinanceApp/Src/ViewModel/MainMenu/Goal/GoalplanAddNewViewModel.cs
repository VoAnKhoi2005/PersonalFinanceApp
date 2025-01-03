﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanAddNewViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly GoalStore _goalStore;

    #region Properties
    //name
    public string NameGoal {
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
    public string TargetGoal {
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
    public string CurrentAmountGoal {
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
    public string ResourceGoal {
        get => _resourceGoal;
        set {
            if (_resourceGoal != value) {
                _resourceGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceGoal;
    //deadline
    public DateTime DeadlineGoal {
        get => _deadlineGoal;
        set {
            if (_deadlineGoal != value) {
                _deadlineGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private DateTime _deadlineGoal = DateTime.Now;

    //status
    public string StatusGoal {
        get => _statusGoal;
        set {
            if (_statusGoal != value) {
                _statusGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _statusGoal;
    //Decription
    public string DescriptionGoal {
        get => _descriptionGoal;
        set {
            if (_descriptionGoal != value) {
                _descriptionGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionGoal;
    //category
    public string CategoryGoal {
        get => _categoryGoal;
        set {
            if (_categoryGoal != value) {
                _categoryGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryGoal;
    //category item source
    public ObservableCollection<string> ItemsGoal {
        get => _itemsGoal;
        set {
            if (_itemsGoal != value) {
                _itemsGoal = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<string> _itemsGoal = new ();
    public string SelectedItemGoal {
        get => _selectedItemGoal;
        set {
            if (_selectedItemGoal != value) {
                if (value != null && value.CompareTo("<New>") == 0) {
                    //excute new category
                    CreateCategoryCommand.Execute(this);
                }
                _selectedItemGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _selectedItemGoal;
    //recurring item source
    public ObservableCollection<string> SourceRecurring {
        get => _sourceRecurring;
        set {
            if (_sourceRecurring != value) {
                _sourceRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<string> _sourceRecurring = new ();
    public string SelectedRecurring {
        get => _selectedRecurring;
        set {
            _selectedRecurring = value;
                OnPropertyChanged();
            }
    }
    private string _selectedRecurring;
    #endregion
    public ICommand CreateCategoryCommand {  get; set; }
    public ICommand CancelNewGoalCommand { get; set; }
    public ICommand ConfirmNewGoalCommand { get; set; }
    public ICommand LoadDataAddNewGoalCommand { get; set; }

    public GoalplanAddNewViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CreateCategoryCommand = new NavigateModalCommand<GoalAddNewCategoryViewModel>(serviceProvider);

        _goalStore.TriggerActionNewCategory += LoadNewCategory;

        LoadDataAddNewGoalCommand = new RelayCommand<object>(LoadItemSourceCategoryGoal);
        
        CancelNewGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmNewGoalCommand = new RelayCommand<object>(ConfirmNewGoal);
    }
    public void LoadNewCategory() {
        if (_goalStore.NewCategory != null) {
            SelectedItemGoal = _goalStore.NewCategory;
            CategoryGoal = _goalStore.NewCategory;
            _goalStore.NewCategory = null;
        }
    }
    public void LoadItemSourceCategoryGoal(object parameter) {
        ItemsGoal.Clear();
        CategoryGoal = (_goalStore.NewCategory != null) ? _goalStore.NewCategory : "";
        var item = DBManager.GetCondition<GoalCategory>(g => g.UserID == _accountStore.Users.UserID);
        ItemsGoal.Add("<New>");
        foreach (var it in item) {
            ItemsGoal.Add(it.Name);
        }
        SourceRecurring.Clear();
        SourceRecurring.Add("Daily");
        SourceRecurring.Add("Weekly");
        SourceRecurring.Add("Monthly");
        SourceRecurring.Add("Yearly");
    }
    private void CloseModal(object sender)
    {
        _goalStore.TriggerActionNewCategory -= LoadNewCategory;
        _modalNavigationStore.Close();
    }
    private void ConfirmNewGoal(object sender) {
        //add data to database
        //Goal
        try {
            Goal goal = new Goal() {
                Name = NameGoal,
                Target = long.Parse(TargetGoal),
                CurrentAmount = long.Parse(CurrentAmountGoal),
                Reminder = SelectedRecurring,
                Deadline = DeadlineGoal,
                Status = GoalStatus(),
                Description = DescriptionGoal,
                UserID = int.Parse(_accountStore.UsersID),
                CategoryName = CategoryGoal,
                StartDay = DateTime.Now,
            };
            DBManager.Insert(goal);

            _accountStore.UploadSaving();
            
            _goalStore.NotifyGoal();
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại nhé", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _goalStore.TriggerActionNewCategory -= LoadNewCategory;
        _modalNavigationStore.Close();
    }
    public void LoadSaving() {
        //load saving in user
        try {
            long saving = 0;
            var items = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
            foreach (var item in items) {
                item.Expenses = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && item.Month == e.ExBMonth && item.Year == e.ExBYear);
                saving += item.Expenses.Sum(ex => ex.Amount);
            }
            var itemgoal = DBManager.GetCondition<Goal>(g => g.UserID == _accountStore.Users.UserID);
            foreach (var item in itemgoal) {
                saving += item.CurrentAmount;
            }
            var usr = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));
            if (usr != null) { usr.Saving = saving; }
            DBManager.Update(usr);
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public string GoalStatus() {
        if(long.Parse(TargetGoal) <= long.Parse(CurrentAmountGoal)) return "Completed";
        else {
            if(DateOnly.FromDateTime(DateTime.Today) >= DateOnly.FromDateTime(DeadlineGoal)) return "Canceled";
            else return "Active";
        }
    }
}