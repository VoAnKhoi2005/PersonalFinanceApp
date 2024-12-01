using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseEditViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
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
            if(value != null) DateOnlyExpenseBook = DateOnly.FromDateTime(_dateTimeEditExpenseBook.Value);
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
    public CategoryItem CategoryEditExpense {
        get => _categoryEditExpense;
        set {
            if (_categoryEditExpense != value) {
                if (value != null && value.Name.CompareTo("<New>") == 0) {
                    //excute new category
                    NewCategoryCommand.Execute(this);
                }
                _categoryEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private CategoryItem _categoryEditExpense;
    //recurring
    public string RecurringEditExpense {
        get => _recurringEditExpense;
        set {
            if (_recurringEditExpense != value) {
                _recurringEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _recurringEditExpense;
    //recurring
    public string TextRecurring {
        get => _textRecurring;
        set {
            if (_textRecurring != value) {
                _textRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textRecurring;
    //expense book
    public ExpenseBookItem SelectedEditExpenseBook {
        get => _selectedEditExpenseBook;
        set {
            if (_selectedEditExpenseBook != value) {
                if (value != null && value.sExB.CompareTo("<New>") == 0) {
                    //excute new expensebook
                    NewExpenseBookCommand.Execute(this);
                }
                if (value != null) {
                    _expenseStore.ExpenseBook = value.exB;
                }
                _selectedEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseBookItem _selectedEditExpenseBook;
    //month
    public string MonthExpenseBook {
        get => _monthExpenseBook;
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
    //itemsource caetegory
    public ObservableCollection<CategoryItem> ItemsEditExpense {
        get => _itemsEditExpense;
        set {
            if (_itemsEditExpense != value) {
                _itemsEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<CategoryItem> _itemsEditExpense = new();
    //itemsource expensebook
    public ObservableCollection<ExpenseBookItem> ItemsEditExpenseBook {
        get => _itemsEditExpenseBook;
        set {
            if (_itemsEditExpenseBook != value) {
                _itemsEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<ExpenseBookItem> _itemsEditExpenseBook = new();
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
    //not expenseBook
    public bool NotExB {
        get => _notExB;
        set {
            if (_notExB != value) {
                _notExB = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _notExB;
    #endregion
    public ICommand CancelEditExpenseCommand { get; set; }
    public ICommand ConfirmEditExpenseCommand { get; set; }
    public ICommand NewCategoryCommand { get; set; }
    public ICommand NewExpenseBookCommand { get; set; }
    public ICommand LoadDataAddCommand { get; set; }
    public ICommand SelectionChangedCommand { get; set; }

    public ExpenseEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        LoadItemSource();

        NewCategoryCommand = new NavigateModalCommand<ExpenseNewCategoryViewModel>(serviceProvider);
        NewExpenseBookCommand = new NavigateModalCommand<ExpenseNewExBViewModel>(serviceProvider);

        LoadDataAddCommand = new RelayCommand<object>(LoadItemSource);
        CancelEditExpenseCommand = new RelayCommand<object>(CloseModal);
        SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
        ConfirmEditExpenseCommand = new RelayCommand<object>(ConfirmEditExpense);
    }
    public void SelectionChanged(object parameter) {
        TextChangedCategory = "";
        if (SelectedEditExpenseBook == null) return;
        if (SelectedEditExpenseBook.sExB.CompareTo("<New>") == 0) {
            NotExB = false;
            return;
        }
        YearExpenseBook = SelectedEditExpenseBook.exB.Year.ToString();
        MonthExpenseBook = SelectedEditExpenseBook.exB.Month.ToString();
        BudgetExpenseBook = SelectedEditExpenseBook.exB.Budget.ToString();

        NotExB = true;

        ItemsEditExpense.Clear();
        ItemsEditExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        if (YearExpenseBook != "" && MonthExpenseBook != "") {
            var items = DBManager.GetCondition<Category>(c => c.UserID == int.Parse(_accountStore.UsersID) && c.ExBYear == int.Parse(YearExpenseBook) && c.ExBMonth == int.Parse(MonthExpenseBook));
            foreach (var item in items) {
                ItemsEditExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
            }
        }
    }
    public void LoadItemSource(object? parameter = null) {
        NotExB = false;
        YearExpenseBook = "";
        MonthExpenseBook = "";
        BudgetExpenseBook = "";
        //load item source category
        ItemsEditExpense.Clear();
        ItemsEditExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });

        ItemsEditExpenseBook.Clear();
        ItemsEditExpenseBook.Add(new ExpenseBookItem { sExB = "<New>" });
        var itemexBs = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
        foreach (var item in itemexBs) {
            ItemsEditExpenseBook.Add(new ExpenseBookItem(item, (item.Month.ToString() + "/" + item.Year.ToString())));
        }

        var exp = _expenseStore.Expenses;
        NameEditExpense = exp.Name;
        AmountEditExpense = exp.Amount.ToString();
        DescriptionEditExpense = exp.Description;
        DateTimeEditExpenseBook = exp.Date.ToDateTime(TimeOnly.MinValue);
        TextRecurring = (exp.Recurring == true) ? "YES":"NO";
        var cate = DBManager.GetFirst<Category>(c => exp.CategoryID == c.CategoryID && c.UserID == exp.UserID);
        TextChangedCategory = cate.Name;
        TextChangedExpense = exp.ExBMonth.ToString() + "/" + exp.ExBYear.ToString();
        MonthExpenseBook = exp.ExBMonth.ToString();
        YearExpenseBook = exp.ExBYear.ToString();
        var exB = DBManager.GetFirst<ExpensesBook>(e => e.UserID == exp.UserID && e.Year == exp.ExBYear && e.Month == exp.ExBMonth);
        BudgetExpenseBook = exB.Budget.ToString();
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditExpense(object sender) {
        //add data to database
        //update expenseBook
        var exB = DBManager.GetFirst<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.Year == int.Parse(YearExpenseBook) && e.Month == int.Parse(MonthExpenseBook));
        if(exB.Budget != long.Parse(BudgetExpenseBook)) {
            exB.Budget = long.Parse(BudgetExpenseBook);
            DBManager.Update<ExpensesBook>(exB);
        }
        //edit expense
        var itemExP = DBManager.GetFirst<Expense>(e => e.ExpenseID == _expenseStore.Expenses.ExpenseID && e.UserID == _expenseStore.Expenses.UserID);
        itemExP.Amount = long.Parse(AmountEditExpense);
        itemExP.Name = NameEditExpense;
        itemExP.Description = DescriptionEditExpense;
        itemExP.Recurring = (RecurringEditExpense.CompareTo("YES") == 0) ? true:false;
        itemExP.CategoryID = CategoryEditExpense.Id;
        itemExP.Date = DateOnlyExpenseBook;

        DBManager.Update<Expense>(itemExP);
        _modalNavigationStore.Close();
    }
    public class CategoryItem {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ExpenseBookItem {
        public ExpensesBook exB { get; set; }
        public string sExB { get; set; }
        public ExpenseBookItem() { }
        public ExpenseBookItem(ExpensesBook e, string s) {
            exB = e;
            sExB = s;
        }
        public ExpenseBookItem(string s) {
            sExB = s;
        }
    }
}