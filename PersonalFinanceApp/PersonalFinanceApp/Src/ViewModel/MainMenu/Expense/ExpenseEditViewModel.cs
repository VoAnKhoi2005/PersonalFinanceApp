using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
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
    //day selected
    public string DayExpenseEdit {
        get => _dayExpenseEdit;
        set {
            if (value != _dayExpenseEdit) {
                _dayExpenseEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private string _dayExpenseEdit;
    //day text
    public string TextDayExpenseEdit {
        get => _textDayExpenseEdit;
        set {
            if (value != _textDayExpenseEdit) {
                _textDayExpenseEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textDayExpenseEdit;
    //itemsource day
    public ObservableCollection<string> ItemDayExpense {
        get => _itemDayExpense;
        set {
            if (_itemDayExpense != value) {
                _itemDayExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _itemDayExpense = new();
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
    //category selected
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
    //Budget current
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
    #endregion
    public ICommand CancelEditExpenseCommand { get; set; }
    public ICommand ConfirmEditExpenseCommand { get; set; }
    public ICommand NewCategoryCommand { get; set; }
    public ICommand LoadDataAddCommand { get; set; }

    public ExpenseEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        LoadItemSource();

        NewCategoryCommand = new NavigateModalCommand<ExpenseNewCategoryViewModel>(serviceProvider);

        LoadDataAddCommand = new RelayCommand<object>(LoadItemSource);
        CancelEditExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditExpenseCommand = new RelayCommand<object>(ConfirmEditExpense);
    }
    public void LoadItemSource(object? parameter = null) {

        YearExpenseBook = _expenseStore.ExpenseBook.Year.ToString();
        MonthExpenseBook = _expenseStore.ExpenseBook.Month.ToString();
        BudgetExpenseBook = _expenseStore.BudgetCurrent;

        //load item source category
        ItemsEditExpense.Clear();
        ItemsEditExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        var items = DBManager.GetCondition<Category>(c => c.UserID == int.Parse(_accountStore.UsersID) && c.ExBYear == int.Parse(YearExpenseBook) && c.ExBMonth == int.Parse(MonthExpenseBook));
        foreach (var item in items) {
            ItemsEditExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
        }
        //day expense
        ItemDayExpense.Clear();
        switch (_expenseStore.ExpenseBook.Month) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                for (int i = 1; i <= 31; ++i) {
                    ItemDayExpense.Add(i.ToString());
                }
                break;
            case 2:
                int per;
                per = (_expenseStore.ExpenseBook.Year % 4 == 0 && _expenseStore.ExpenseBook.Year % 100 != 0) ? 29 : 28;
                for (int i = 1; i <= per; ++i) {
                    ItemDayExpense.Add(i.ToString());
                }
                break;
            default:
                for (int i = 1; i <= 30; ++i) {
                    ItemDayExpense.Add(i.ToString());
                }
                break;
        }
        var exp = _expenseStore.Expenses;
        NameEditExpense = exp.Name;
        AmountEditExpense = exp.Amount.ToString();
        DescriptionEditExpense = exp.Description;
        var cate = DBManager.GetFirst<Category>(c => exp.CategoryID == c.CategoryID && c.UserID == exp.UserID);
        TextChangedCategory = cate.Name;
        CategoryEditExpense = new CategoryItem { Id = cate.CategoryID, Name = cate.Name };
        MonthExpenseBook = exp.ExBMonth.ToString();
        YearExpenseBook = exp.ExBYear.ToString();
        BudgetExpenseBook = _expenseStore.BudgetCurrent;
        TextDayExpenseEdit = exp.Date.Day.ToString();
        DayExpenseEdit = exp.Date.Day.ToString();
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditExpense(object sender) {
        //add data to database
        //edit expense
        var itemExP = DBManager.GetFirst<Expense>(e => e.ExpenseID == _expenseStore.Expenses.ExpenseID && e.UserID == _expenseStore.Expenses.UserID);
        itemExP.Amount = long.Parse(AmountEditExpense);
        itemExP.Name = NameEditExpense;
        itemExP.Description = DescriptionEditExpense;
        itemExP.CategoryID = CategoryEditExpense.Id;
        itemExP.Date = new DateOnly(int.Parse(YearExpenseBook), int.Parse(MonthExpenseBook),int.Parse(DayExpenseEdit));

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