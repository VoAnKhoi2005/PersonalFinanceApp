using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class RecurringAddnew : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly ModalNavigationStore _modalNavigationStore;

    #region Properties
    //name
    public string NameRecurring {
        get => _nameRecurring;
        set {
            if (_nameRecurring != value) {
                _nameRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameRecurring;
    //interval
    public string IntervalRecurring {
        get => _intervalRecurring;
        set {
            if (_intervalRecurring != value) {
                _intervalRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _intervalRecurring;
    //frequency
    public ObservableCollection<string> ItemsSourceFrequency {
        get => _itemsSourceFrequency;
        set {
            if (_itemsSourceFrequency != value) {
                _itemsSourceFrequency = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _itemsSourceFrequency = new();
    public string SelectedFrequency {
        get => _selectedFrequency;
        set {
            if (_selectedFrequency != value) {
                _selectedFrequency = value;
                OnPropertyChanged();
            }
        }
    }
    private string _selectedFrequency;
    //date
    public DateTime? DateTimeRecurring {
        get => _dateTimeRecurring;
        set {
            _dateTimeRecurring = value;
            OnPropertyChanged(nameof(DateTimeRecurring));
            DateOnlyRecurring = DateOnly.FromDateTime(_dateTimeRecurring.Value);
        }
    }
    private DateTime? _dateTimeRecurring;
    public DateOnly DateOnlyRecurring {
        get => _dateOnlyRecurring;
        private set {
            _dateOnlyRecurring = value;
            OnPropertyChanged(nameof(DateOnlyRecurring));
        }
    }
    private DateOnly _dateOnlyRecurring;
    #endregion
    public ICommand CancelRecurringCommand {  get; set; }
    public ICommand ConfirmRecurringCommand { get; set; }
    public RecurringAddnew(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadItemsource();
        CancelRecurringCommand = new RelayCommand<object>(CloseModal);
        ConfirmRecurringCommand = new RelayCommand<object>(NewRecurring);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void NewRecurring(object? parameter = null) {
        var recur = new RecurringExpense(NameRecurring, SelectedFrequency, int.Parse(IntervalRecurring), DateOnlyRecurring);
        DBManager.Insert(recur);
        _modalNavigationStore.Close();
    }
    public void LoadItemsource() {
        ItemsSourceFrequency.Clear();
        ItemsSourceFrequency.Add("Daily");
        ItemsSourceFrequency.Add("Weekly");
        ItemsSourceFrequency.Add("Monthly");
        ItemsSourceFrequency.Add("Yearly");
    }
}
