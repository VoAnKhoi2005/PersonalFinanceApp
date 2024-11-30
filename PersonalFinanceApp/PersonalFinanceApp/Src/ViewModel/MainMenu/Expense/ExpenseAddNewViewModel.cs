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
    private readonly ExpenseStore _expenseStore;
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
            if (value) {
                _recurringExpense = true;
                OnPropertyChanged();
            }
            else {
                _recurringExpense = false;
                OnPropertyChanged();
            }
        }
    }
    private bool _recurringExpense;
    //selected category
    public CategoryItem SelectedCategory {
        get => _selectedCategory;
        set {
            if (_selectedCategory != value) {
                if(value != null && value.Name.CompareTo("<New>") == 0) {
                    //excute new category
                    NewCategoryCommand.Execute(this);
                }
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private CategoryItem _selectedCategory;
    //selected ExpenseBook item
    public ExpenseBookItem SelectedItemExpenseBook {
        get => _selectedItemExpenseBook;
        set {
            if (_selectedItemExpenseBook != value) {
                if (value != null && value.sExB.CompareTo("<New>") == 0) {
                    //excute new expensebook
                    NewExpenseBookCommand.Execute(this);
                }
                if(value != null) {
                    _expenseStore.ExpenseBook = value.exB;
                }
                _selectedItemExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseBookItem _selectedItemExpenseBook;
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
    //text expense
    public string TextChangedExpense {
        get => _textChangedExpense;
        set {
            if (_textChangedExpense != value) {
                _textChangedExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textChangedExpense;
    //text category
    public string TextChangedCategory {
        get => _textChangedCategory;
        set {
            if (_textChangedCategory != value) {
                _textChangedCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textChangedCategory;
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
    public bool NotExB {
        get => _notExB;
        set {
            if (_notExB != value) {
                _notExB = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _notExB = true;
    #endregion
    public ICommand CancelAddNewExpenseCommand { get; set; }
    public ICommand ConfirmAddNewExpenseCommand { get; set; }
    public ICommand NewCategoryCommand {  get; set; }
    public ICommand NewExpenseBookCommand {  get; set; }
    public ICommand SelectionChangedCommand { get; set; }
    public ICommand LoadDataAddCommand { get; set; }

    public ExpenseAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        NewCategoryCommand = new NavigateModalCommand<ExpenseAddNewCategory>(serviceProvider);
        NewExpenseBookCommand = new NavigateModalCommand<ExpenseNewExpenseBook>(serviceProvider);

        LoadDataAddCommand = new RelayCommand<object>(LoadItemSource);
        CancelAddNewExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmAddNewExpenseCommand = new RelayCommand<object>(ConfirmAddNewExpense);
        SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
    }
    public void SelectionChanged(object parameter) {
        if (SelectedItemExpenseBook == null) return;
        if(SelectedItemExpenseBook.sExB.CompareTo("<New>") == 0) {
            NotExB = false;
            return;
        }
        YearExpenseBook = SelectedItemExpenseBook.exB.Year.ToString();
        MonthExpenseBook = SelectedItemExpenseBook.exB.Month.ToString();
        BudgetExpenseBook = SelectedItemExpenseBook.exB.Budget.ToString(); 
        NotExB = true;

        ItemsExpense.Clear();
        ItemsExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        if (YearExpenseBook != "" && MonthExpenseBook != "") {
            var items = DBManager.GetCondition<Category>(c => c.UserID == int.Parse(_accountStore.UsersID) && c.ExBYear == int.Parse(YearExpenseBook) && c.ExBMonth == int.Parse(MonthExpenseBook));
            foreach (var item in items) {
                ItemsExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
            }
        }
    }
    public void LoadItemSource(object parameter) {
        NotExB = true;
        YearExpenseBook = "";
        MonthExpenseBook = "";
        BudgetExpenseBook = "";
        //load item source category
        ItemsExpense.Clear();
        ItemsExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        

        ItemsExpenseBook.Clear();
        ItemsExpenseBook.Add(new ExpenseBookItem { sExB = "<New>" });
        var itemexBs = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
        foreach (var item in itemexBs) {
            ItemsExpenseBook.Add(new ExpenseBookItem(item, (item.Month.ToString() + "/" + item.Year.ToString())));
        }
        //load item expensebook
        //if (_expenseStore.Categorys != null) {
        //    TextChangedCategory = _expenseStore.Categorys.Name;
        //    SelectionChanged(parameter);
        //}
        

        //if (_expenseStore.TextChangedExp != null) {
        //    TextChangedExpense = _expenseStore.TextChangedExp;
        //    SelectionChanged(parameter);
        //}
        
        
        
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
            CategoryID = SelectedCategory.Id,
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