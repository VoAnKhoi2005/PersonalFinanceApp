using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using XAct;
using XAct.Messages;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class RecurringAddnew : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;
    private readonly ExpenseStore _expenseStore;

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

    //category
    public string CategoryRecurring {
        get => _categoryRecurring;
        set {
            if (_categoryRecurring != value) {
                _categoryRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryRecurring;
    //source category
    public ObservableCollection<Category> SourceCategory {
        get => _sourceCategory;
        set {
            if (_sourceCategory != value) {
                _sourceCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Category> _sourceCategory;
    //selected Item category
    public Category SelectedCategory {
        get => _selectedCategory;
        set {
            if (_selectedCategory != value) {
                if (value != null && value.Name.CompareTo("<New>") == 0) {
                    //excute new category

                    NewCategoryCommand.Execute(this);
                }
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private Category _selectedCategory;
    //amount
    public string AmountRecurring {
        get => _amountRecurring;
        set {
            if (_amountRecurring != value) {
                _amountRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountRecurring;
    //description
    public string DescriptionRecurring {
        get => _descriptionRecurring;
        set {
            if (_descriptionRecurring != value) {
                _descriptionRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionRecurring;

    public User usr { get; set; }

    #endregion
    public ICommand CancelRecurringCommand {  get; set; }
    public ICommand ConfirmRecurringCommand { get; set; }
    public ICommand NewCategoryCommand { get; set; }
    public ICommand LoadSourceCategoryCommand {  get; set; }
    public RecurringAddnew(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        usr = _accountStore.Users;
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();
        NewCategoryCommand = new NavigateModalCommand<ExpenseNewCategoryViewModel>(serviceProvider);
        LoadItemsource();
        SourceCategory = new();
        _expenseStore.TriggerNewCategory += () => LoadSourceCategory();
        CancelRecurringCommand = new RelayCommand<object>(CloseModal);
        ConfirmRecurringCommand = new RelayCommand<object>(NewRecurring);
        LoadSourceCategoryCommand = new RelayCommand<object>(LoadSourceCategory);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void LoadSourceCategory(object ? parameter = null) {
        try {
            var exB = DBManager.GetFirst<ExpensesBook>(ex => ex.UserID == usr.UserID &&
                                                       ex.Month == DateTimeRecurring.Value.Month &&
                                                       ex.Year == DateTimeRecurring.Value.Year);

            if (exB != null) {
                //load category
                var categories = DBManager.GetCondition<Category>(c => c.UserID == _accountStore.Users.UserID && c.ExBMonth == exB.Month && c.ExBYear == exB.Year);
                SourceCategory.Clear();
                SourceCategory.Add(new Category("<New>", exB));
                foreach (var category in categories) {
                    SourceCategory.Add(category);
                }
                _expenseStore.ExpenseBook = exB;
                if(_expenseStore.Categorys != null) {
                    SelectedCategory = _expenseStore.Categorys;
                    CategoryRecurring = _expenseStore.Categorys.Name;
                }
                
            }
            else {
                var results = MessageBox.Show("ExpenseBook này hiện chưa có bạn có muốn tạo không", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (results == MessageBoxResult.Yes) {
                    //create expensebook

                    if (exB == null) {
                        exB = new ExpensesBook(DateTimeRecurring.Value.Month, DateTimeRecurring.Value.Year,
                                                _accountStore.Users, _accountStore.Users.DefaultBudget);
                        bool check = DBManager.Insert(exB);
                        if (check == false) {
                            throw new Exception("Thêm ExpenseBook thất bại");
                        }
                    }
                    SourceCategory.Clear();
                    SourceCategory.Add(new Category("<New>", exB));
                    _expenseStore.ExpenseBook = exB;
                }
                else {
                    SourceCategory.Clear();
                }

            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void NewRecurring(object? parameter = null) {
        try {
            bool check = true;
            
            
            //create recurring
            RecurringExpense re = new RecurringExpense(NameRecurring, SelectedFrequency, int.Parse(IntervalRecurring), DateOnlyRecurring, int.Parse(_accountStore.UsersID));
            check = DBManager.Insert(re);
            if (check == false) {
                throw new Exception("Thêm Recurring thất bại");
            }
            //create expense
            if(re.StartDate <= DateOnly.FromDateTime(DateTime.Today)) {
                var result = MessageBox.Show("Bạn có muốn thêm expense ngay không", "Notification", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes) {
                    Expense exp = new Expense(long.Parse(AmountRecurring), NameRecurring, DateOnlyRecurring, SelectedCategory, _expenseStore.ExpenseBook, DescriptionRecurring);
                    var recurring = DBManager.GetFirst<RecurringExpense>(re => re.StartDate == DateOnlyRecurring &&
                                                                        re.Name == NameRecurring && re.Frequency == SelectedFrequency &&
                                                                        re.Interval == int.Parse(IntervalRecurring) && re.UserID == int.Parse(_accountStore.UsersID));
                    exp.RecurringExpenseID = recurring.RecurringExpenseID;
                    check = DBManager.Insert(exp);
                    if (check == false) {
                        throw new Exception("Thêm Expense thất bại");
                    }
                }
            }
            
            _recurringStore.NotifyRecurring();
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

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
