using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Database;
using System.Collections.ObjectModel;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Windows;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    private readonly SharedService _shareService;

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
    //date selected
    public string DayExpense {
        get => _dayExpense;
        set {
            if (value != _dayExpense) {
                _dayExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _dayExpense;
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
    //selected item category
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
    private ObservableCollection<CategoryItem> _itemsExpense = new();
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
    public ICommand CancelAddNewExpenseCommand { get; set; }
    public ICommand ConfirmAddNewExpenseCommand { get; set; }
    public ICommand NewCategoryCommand {  get; set; }
    public ICommand LoadDataAddCommand { get; set; }

    public ExpenseAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _shareService = serviceProvider.GetRequiredService<SharedService>();

        NewCategoryCommand = new NavigateModalCommand<ExpenseNewCategoryViewModel>(serviceProvider);

        LoadDataAddCommand = new RelayCommand<object>(LoadItemSource);
        CancelAddNewExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmAddNewExpenseCommand = new RelayCommand<object>(ConfirmAddNewExpense);
    }
    public void LoadItemSource(object parameter) {
        try {
            TextChangedCategory = "";
            YearExpenseBook = _expenseStore.ExpenseBook.Year.ToString(); ;
            MonthExpenseBook = _expenseStore.ExpenseBook.Month.ToString();
            BudgetExpenseBook = _expenseStore.BudgetCurrentExb;

            //load item source category
            ItemsExpense.Clear();
            ItemsExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
            var items = DBManager.GetCondition<Category>(c => c.UserID == int.Parse(_accountStore.UsersID) && c.ExBYear == int.Parse(YearExpenseBook) && c.ExBMonth == int.Parse(MonthExpenseBook));
            if (items != null) {
                foreach (var item in items) {
                    ItemsExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
                }
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
        }
        catch(Exception ex) {
            MessageBox.Show("Có lỗi xảy ra xin vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmAddNewExpense(object sender) {
        //add data to database
        long result;
        if (!long.TryParse(AmountExpense, out result)) {
            MessageBox.Show("Amount quá to! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        else {
            if(result == 0) {
                MessageBox.Show("Amount phải khác 0! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        try {
            if (long.Parse(_expenseStore.BudgetCurrentExb) - long.Parse(AmountExpense) < 0) {
                MessageBoxResult rs = MessageBox.Show(
                    "Ngân sách âm rồi, bạn có muốn tiếp tục không?",
                    "Warning!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (rs == MessageBoxResult.No) {
                    return;
                }
                
            }
            var expense = new Expense() {
                Amount = long.Parse(AmountExpense),
                Name = NameExpense,
                Description = DescriptionExpense,
                Date = new DateOnly(int.Parse(YearExpenseBook), int.Parse(MonthExpenseBook), int.Parse(DayExpense)),
                TimeAdded = DateTime.Now,
                Deleted = false,
                CategoryID = SelectedCategory.Id,
                ExBMonth = int.Parse(MonthExpenseBook),
                ExBYear = int.Parse(YearExpenseBook),
                UserID = int.Parse(_accountStore.UsersID),
                
            };
            DBManager.Insert(expense);

            LoadSaving();

            _shareService.Notify();
        }catch(Exception ex) {
            MessageBox.Show("Vui lòng nhập đầy đủ và đúng chuẩn thông tin nhé!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
    public void LoadSaving() {
        //load saving in user
        try {
            long saving = 0;
            var items = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
            foreach (var item in items) {
                item.Expenses = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && item.Month == e.ExBMonth && item.Year == e.ExBYear);
                saving += item.Expenses.Sum(ex => ex.Amount);
            }
            var usr = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));
            if (usr != null) { usr.Saving = saving; }
            DBManager.Update(usr);
        }
        catch(Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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