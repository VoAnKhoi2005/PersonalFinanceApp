using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Database;
using System.Collections.ObjectModel;
using PersonalFinanceApp.Src.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseBookStore _expenseBookStore;

    #region Properties
    //name
    public string NameExpense {
        get => _nameExpense;
        set {
            if (_nameExpense != value) {
                _nameExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameExpense;
    //amount
    public string AmountExpense {
        get => _amountExpense;
        set {
            if (_amountExpense != value) {
                _amountExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountExpense;
    //date
    public DateTime? DateTimeExpenseBook {
        get => _dateTimeExpenseBook;
        set {
            _dateTimeExpenseBook = value;
            OnPropertyChanged(nameof(DateTimeExpenseBook));
            DateOnlyExpenseBook = DateOnly.FromDateTime(_dateTimeExpenseBook.Value);
        }
    }
    private DateTime? _dateTimeExpenseBook;
    public DateOnly DateOnlyExpenseBook {
        get => _dateOnlyExpenseBook;
        private set {
            _dateOnlyExpenseBook = value;
            OnPropertyChanged(nameof(DateOnlyExpenseBook));
        }
    }
    private DateOnly _dateOnlyExpenseBook;
    //description
    public string DescriptionExpense {
        get => _descriptionExpense;
        set {
            if (_descriptionExpense != value) {
                _descriptionExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionExpense;
    //category
    public string CategoryExpense {
        get => _categoryExpense;
        set {
            if (_categoryExpense != value) {
                _categoryExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryExpense;
    //resource
    public string ResourceExpense {
        get => _resourceExpense;
        set {
            if (_resourceExpense != value) {
                _resourceExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceExpense;
    //recurring
    public bool RecurringExpense {
        get => _recurringExpense;
        set {
            if (_recurringExpense != value) {
                _recurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _recurringExpense;
    //category
    public int SelectedCategory {
        get => _selectedCategory;
        set {
            if (_selectedCategory != value) {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private int _selectedCategory;
    //itemsource
    public ObservableCollection<CategoryItem> ItemsExpense {
        get => _itemsExpense;
        set {
            if (_itemsExpense != value) {
                _itemsExpense = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<CategoryItem> _itemsExpense = new();
    #endregion
    public ICommand CancelAddNewExpenseCommand { get; set; }
    public ICommand ConfirmAddNewExpenseCommand { get; set; }

    public ExpenseAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadItemSource();
        CancelAddNewExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmAddNewExpenseCommand = new RelayCommand<object>(ConfirmAddNewExpense);
    }
    public void LoadItemSource() {
        ItemsExpense.Clear();
        ItemsExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        var items = DBManager.GetCondition<Category>(c => c.UserID == _expenseBookStore.ExpenseBook.UserID && c.ExBMonth == _expenseBookStore.ExpenseBook.Month && c.ExBYear == _expenseBookStore.ExpenseBook.Year);
        foreach(var item in items) {
            ItemsExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
        }
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmAddNewExpense(object sender) {
        //add data to database
        var expense = new Expense() {
            Amount = int.Parse(AmountExpense),
            Name = NameExpense,
            Date = DateOnlyExpenseBook,
            Recurring = true,
            CategoryID = SelectedCategory,
            ExBMonth = _expenseBookStore.ExpenseBook.Month,
            ExBYear = _expenseBookStore.ExpenseBook.Year,
            UserID = _expenseBookStore.ExpenseBook.UserID,
            Description = DescriptionExpense,
            TimeAdded = DateTime.Now,
            Resources = ResourceExpense,
            Deleted = false,
        };
        DBManager.Insert(expense);
        _modalNavigationStore.Close();
    }
    public class CategoryItem {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}