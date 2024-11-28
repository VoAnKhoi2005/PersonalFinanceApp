using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseEditViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
    #region Properties
    //name
    public string NameEditExpense {
        get => _nameEditExpense;
        set {
            if (_nameEditExpense != value) {
                _nameEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameEditExpense;
    //amount
    public string AmountEditExpense {
        get => _amountEditExpense;
        set {
            if (_amountEditExpense != value) {
                _amountEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountEditExpense;
    //date
    public DateTime? DateTimeEditExpenseBook {
        get => _dateTimeEditExpenseBook;
        set {
            _dateTimeEditExpenseBook = value;
            OnPropertyChanged(nameof(DateTimeEditExpenseBook));
            DateOnlyExpenseBook = DateOnly.FromDateTime(_dateTimeEditExpenseBook.Value);
        }
    }
    private DateTime? _dateTimeEditExpenseBook;
    public DateOnly DateOnlyExpenseBook {
        get => _dateOnlyExpenseBook;
        private set {
            _dateOnlyExpenseBook = value;
            OnPropertyChanged(nameof(DateOnlyExpenseBook));
        }
    }
    private DateOnly _dateOnlyExpenseBook;
    //description
    public string DescriptionEditExpense {
        get => _descriptionEditExpense;
        set {
            if (_descriptionEditExpense != value) {
                _descriptionEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionEditExpense;
    //category
    public string CategoryEditExpense {
        get => _categoryEditExpense;
        set {
            if (_categoryEditExpense != value) {
                _categoryEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryEditExpense;
    //resource
    public string ResourceEditExpense {
        get => _resourceEditExpense;
        set {
            if (_resourceEditExpense != value) {
                _resourceEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceEditExpense;
    #endregion
    public ICommand CancelEditExpenseCommand { get; set; }
    public ICommand ConfirmEditExpenseCommand { get; set; }

    public ExpenseEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelEditExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditExpenseCommand = new RelayCommand<object>(ConfirmEditExpense);
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private Category ca() {
        Category category = new Category();
        return category;
    }
    private ExpensesBook exB() {
        ExpensesBook exBook = new ExpensesBook();
        return exBook;
    }
    private void ConfirmEditExpense(object sender) {
        //add data to database
        var expense = new Expense() {
            Amount = int.Parse(AmountEditExpense),
            Name = NameEditExpense,
            Date = DateOnlyExpenseBook,
            //Recurring = recurring;
            //CategoryID = ca.CategoryID;
            //ExBMonth = exB.Month;
            //ExBYear = exB.Year;
            //UserID = exB.UserID;
            Description = DescriptionEditExpense,
            TimeAdded = DateTime.Now,
            Resources = ResourceEditExpense,
            Deleted = false,
        };
        DBManager.Insert(expense);
        _modalNavigationStore.Close();
    }
}