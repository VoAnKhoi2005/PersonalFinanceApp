using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using Windows.Globalization;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanAddNewViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
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
    //reminder
    public string ReminderGoal {
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
    public string DeadlineGoal {
        get => _deadlineGoal;
        set {
            if (_deadlineGoal != value) {
                _deadlineGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _deadlineGoal;
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
    public string DecriptionGoal {
        get => _decriptionGoal;
        set {
            if (_decriptionGoal != value) {
                _decriptionGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _decriptionGoal;
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
    //category
    public ObservableCollection<string> _itemsGoal = new ObservableCollection<string> {
        #region Category of Goal
        "New vehicle",
        "New home",
        "Hoiliday trip",
        "Education",
        "Emergency fund",
        "Health care",
        "Party",
        "Kids spoiling",
        "Charity",
        "<New>",
        #endregion
    };
    public ObservableCollection<string> ItemsGoal {
        get => _itemsGoal;
        set {
            if (_itemsGoal != value) {
                _itemsGoal = value;
                OnPropertyChanged();
            }
        }
    }
    public string SelectedItemGoal { get; set; }
    #endregion
    public ICommand CancelNewGoalCommand { get; set; }
    public ICommand ConfirmNewGoalCommand { get; set; }

    public GoalplanAddNewViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelNewGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmNewGoalCommand = new RelayCommand<object>(ConfirmNewGoal);
    }

    private void CloseModal(object sender)
    {
        _modalNavigationStore.Close();
    }
    private void ConfirmNewGoal(object sender) {
        //add data to database
        //Goal Category 
        GoalCategory goalcategory = new GoalCategory() {
            Name = CategoryGoal,
            Description = DecriptionGoal,
        };
        DBManager.Insert(goalcategory);
        //Goal
        Goal goal = new Goal() {
            Name = NameGoal,
            Target = long.Parse(TargetGoal),
            CurrentAmount = long.Parse(CurrentAmountGoal),
            Resources = ResourceGoal,
            UserID = int.Parse(_accountStore.SharedUser[0]),
            CategoryName = goalcategory.Name,
            //User = _accountStore.Users,
            //GoalCategory = goalcategory,
            //Deadline = DeadlineGoal
        };
        //Goal History
        GoalHistory goalhistory = new GoalHistory() {
            //Goal = (Goal)goal,
            GoalID = goal.GoalID,
            TimeAdded = DateTime.Now,
            Amount = long.Parse(CurrentAmountGoal),
        };
        //goal.GoalHistories.Add(goalhistory);

        DBManager.Insert(goalhistory);

        DBManager.Insert(goal);

        _modalNavigationStore.Close();
    }
}