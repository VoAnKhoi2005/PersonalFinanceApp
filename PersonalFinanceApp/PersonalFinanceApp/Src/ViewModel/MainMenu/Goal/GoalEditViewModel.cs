using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu;

public class GoalEditViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
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
    public string DeadlineEditGoal {
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
    //Decription
    public string DecriptionEditGoal {
        get => _decriptionEditGoal;
        set {
            if (_decriptionEditGoal != value) {
                _decriptionEditGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _decriptionEditGoal;
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
    #endregion
    public ICommand CancelEditGoalCommand { get; set; }
    public ICommand ConfirmEditGoalCommand { get; set; }

    public GoalEditViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelEditGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditGoalCommand = new RelayCommand<object>(ConfirmEditGoal);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditGoal(object sender) {
        //add data to database

        _modalNavigationStore.Close();
    }
}
