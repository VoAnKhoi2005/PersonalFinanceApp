using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingBudgetDefaultViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //BudgetDefault
    public string BudgetDefault {
        get => _budgetDefault;
        set {
            if (value != _budgetDefault) {
                _budgetDefault = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetDefault;

    #endregion
    #region Command
    public ICommand CancelChangedBudgetDefaultCommand { get; set; }
    public ICommand ConfirmChangedBudgetDefaultCommand { get; set; }
    #endregion
    public SettingBudgetDefaultViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        BudgetDefault = _accountStore.Users.DefaultBudget.ToString();

        CancelChangedBudgetDefaultCommand = new RelayCommand<object>(CloseModal);
        ConfirmChangedBudgetDefaultCommand = new RelayCommand<object>(ChangedBudgetDefault);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void ChangedBudgetDefault(object? parameter = null) {
        try {
            if (!long.TryParse(_budgetDefault, out var budget)) {
                throw new Exception("Dữ liệu không hợp lệ vui lòng thử lại");
            }
            var usr = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));
            if (usr != null) {
                usr.DefaultBudget = long.Parse(BudgetDefault);
            }
            DBManager.Update<User>(usr);
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
}

