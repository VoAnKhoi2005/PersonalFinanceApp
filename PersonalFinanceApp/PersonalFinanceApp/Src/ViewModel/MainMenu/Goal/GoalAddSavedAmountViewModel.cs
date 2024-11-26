using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalAddSavedAmountViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    #region Properties
    //Plus
    public bool IsPlus {
        get => _isPlus;
        set {
            if (_isPlus != value) {
                _isPlus = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isPlus;
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
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelAddSavedGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmAddSavedGoalCommand = new RelayCommand<object>(ConfirmSavedGoal);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmSavedGoal(object sender) {
        //add data to database

        _modalNavigationStore.Close();
    }
}
