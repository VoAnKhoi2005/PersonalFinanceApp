using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

class FilterExpenseViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //isday
    public bool IsDay {
        get=>_isDay;
        set {
            if (_isDay != value) {
                _isDay = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isDay;
    //selected options
    public string SelectedOption {
        get => _selectedOption;
        set {
            if(_selectedOption != value) {
                _selectedOption = value; 
                if(value.CompareTo("Expense") == 0) {
                    IsDay = true;
                }else IsDay = false;
                OnPropertyChanged(nameof(SelectedOption));
            }
        }
    }
    private string _selectedOption;
    //selected day
    public string Day {
        get => _day;
        set {
            if (_day != value) {
                _day = value;
                OnPropertyChanged();
            }
        }
    }
    private string _day;
    //selected month
    public string Month {
        get => _month;
        set {
            if (_month != value) {
                _month = value;
                OnPropertyChanged();
            }
        }
    }
    private string _month;
    //selected year
    public string Year {
        get => _year;
        set {
            if (_year!= value) {
                _year = value;
                OnPropertyChanged();
            }
        }
    }
    private string _year;
    #endregion
    public ICommand CancelFilterCommand { get; set; }
    public ICommand ConfirmFilterCommand { get; set; }

    public FilterExpenseViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelFilterCommand = new RelayCommand<object>(CloseModal);
        ConfirmFilterCommand = new RelayCommand<object>(ConfirmModal);
    }
    public void CloseModal(object parameter) {
        _modalNavigationStore.Close();
    }
    public void ConfirmModal(object parameter) {
        //confirm
        if(SelectedOption.CompareTo("Expense") == 0) {

        }
        _modalNavigationStore.Close();
    }
}
