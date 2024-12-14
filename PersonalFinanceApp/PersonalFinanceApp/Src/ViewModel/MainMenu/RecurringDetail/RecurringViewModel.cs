using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class RecurringViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    public ObservableCollection<RecurringExpense> recurringExpenses {
        get => _recurringExpense;
        set {
            if (_recurringExpense != value) {
                _recurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<RecurringExpense> _recurringExpense = new();
    public RecurringExpense ItemsRecurring {
        get => _itemsRecurring;
        set {
            if (_itemsRecurring != value) {
                _itemsRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private RecurringExpense _itemsRecurring;

    public CalendarViewModel calendarViewModel { get; set; }

    public ICommand AddNewRecurringCommand { get; set; }
    public ICommand RefreshRecurringCommand { get; set;}

    public RecurringViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        calendarViewModel = new CalendarViewModel(serviceProvider);
        
        LoadData();
        AddNewRecurringCommand = new NavigateModalCommand<RecurringAddnew>(serviceProvider);
        RefreshRecurringCommand = new RelayCommand<object>(LoadData);
    }
    public void LoadData(object? parameter = null) {

    }
    
}
