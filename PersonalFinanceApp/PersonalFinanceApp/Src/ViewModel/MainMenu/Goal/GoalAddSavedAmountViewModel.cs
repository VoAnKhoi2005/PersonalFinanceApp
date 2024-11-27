using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalAddSavedAmountViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;
    #region Properties
    //Plus
    public bool IsDeposit {
        get => _isDeposit;
        set {
            if (_isDeposit != value) {
                _isDeposit = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isDeposit = false;
    //Amount
    public string AmountAddSaved {
        get => _amountAddSaved;
        set {
            if (_amountAddSaved != value) {
                _amountAddSaved = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountAddSaved;

    #endregion
    public ICommand CancelAddSavedGoalCommand { get; set; }
    public ICommand ConfirmAddSavedGoalCommand { get; set; }

    public GoalAddSavedAmountViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _goalStore = _serviceProvider.GetRequiredService<GoalStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelAddSavedGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmAddSavedGoalCommand = new RelayCommand<object>(ConfirmSavedGoal);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmSavedGoal(object sender) {
        //add data to database
        var item = DBManager.GetFirst<Goal>(g => g.GoalID == int.Parse(_goalStore.GoalID));
        if(!IsDeposit) {
            item.CurrentAmount += long.Parse(AmountAddSaved);
        }
        else {
            if (item.CurrentAmount - long.Parse(AmountAddSaved) < 0) item.CurrentAmount = 0;
            else item.CurrentAmount -= long.Parse(AmountAddSaved);
        }
         bool checkUpdate = DBManager.Update<Goal>(item);
        _modalNavigationStore.Close();
    }
}
