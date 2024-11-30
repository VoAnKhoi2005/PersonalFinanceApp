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
    private readonly AccountStore _accountStore;

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
    //selected category
    public int SelectedCategory {
        get => _selectedCategory;
        set {
            if (_selectedCategory != value) {
                if(value == -1) {
                    //excute new category
                    NewCategoryCommand.Execute(this);
                }
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private int _selectedCategory;
    //selected ExpenseBook
    public string SelectedExpenseBook {
        get => _selectedExpenseBook;
        set {
            if (_selectedExpenseBook != value) {
                if(value.CompareTo("<New>") == 0) {
                    //excute new expensebook
                    NewExpenseBookCommand.Execute(this);
                }
                _selectedExpenseBook = value;
                var x = value.Split('/');
                if (x.Length > 1) { MonthExpenseBook = x[0]; YearExpenseBook = x[1]; BudgetExpenseBook = x[2]; }
                OnPropertyChanged();
            }
        }
    }
    private string _selectedExpenseBook;
    //month
    public string MonthExpenseBook {
        get =>_monthExpenseBook;
        set {
            if (_monthExpenseBook != value) {
                _monthExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _monthExpenseBook;
    //year
    public string YearExpenseBook {
        get => _yearExpenseBook;
        set {
            if (_yearExpenseBook != value) {
                _yearExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _yearExpenseBook;
    //Budget
    public string BudgetExpenseBook {
        get => _budgetExpenseBook;
        set {
            if (_budgetExpenseBook != value) {
                _budgetExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetExpenseBook;
    //itemsource category
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
    //itemsource exB
    public ObservableCollection<ExpenseBookItem> ItemsExpenseBook {
        get => _itemsExpenseBook;
        set {
            if (_itemsExpenseBook != value) {
                _itemsExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<ExpenseBookItem> _itemsExpenseBook = new();
    #endregion
    public ICommand CancelAddNewExpenseCommand { get; set; }
    public ICommand ConfirmAddNewExpenseCommand { get; set; }
    public ICommand NewCategoryCommand {  get; set; }
    public ICommand NewExpenseBookCommand {  get; set; }

    public ExpenseAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        LoadItemSource();
        NewCategoryCommand = new NavigateModalCommand<ExpenseAddNewCategory>(serviceProvider);
        //NewExpenseBookCommand = new NavigateModalCommand<ExpenseAddNewExpenseBook>(serviceProvider);
        CancelAddNewExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmAddNewExpenseCommand = new RelayCommand<object>(ConfirmAddNewExpense);
    }
    public void LoadItemSource() {
        //load item source category
        ItemsExpense.Clear();
        ItemsExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        var items = DBManager.GetCondition<Category>(c => c.UserID == _expenseBookStore.ExpenseBook.UserID && c.ExBMonth == _expenseBookStore.ExpenseBook.Month && c.ExBYear == _expenseBookStore.ExpenseBook.Year);
        foreach(var item in items) {
            ItemsExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
        }
        //load item expensebook
        ItemsExpenseBook.Clear();
        ItemsExpenseBook.Add(new ExpenseBookItem { sExB = "<New>" });
        var itemexBs = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
        foreach(var item in itemexBs) {
            ItemsExpenseBook.Add(new ExpenseBookItem(item, (item.Month.ToString() + "/" + item.Year.ToString() + "/" + item.Budget.ToString())));
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
            ExBMonth = int.Parse(MonthExpenseBook),
            ExBYear = int.Parse(YearExpenseBook),
            UserID = int.Parse(_accountStore.UsersID),
            Description = DescriptionExpense,
            TimeAdded = DateTime.Now,
            Deleted = false,
        };
        DBManager.Insert(expense);
        _modalNavigationStore.Close();
    }
    public class CategoryItem {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ExpenseBookItem {
        public ExpensesBook exB { get; set; }
        public string sExB {  get; set; }
        public ExpenseBookItem() { }
        public ExpenseBookItem (ExpensesBook e, string s) {
            exB = e;
            sExB = s;
        }
        public ExpenseBookItem(string s) {
            sExB = s;
        }
    }
}