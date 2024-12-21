using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.ViewModel.Command;


namespace PersonalFinanceApp.ViewModel.MainMenu;
public class RecurringEditViewModel : BaseViewModel {
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
    public string TextFrequency {
        get => _textFrequency;
        set {
            if (_textFrequency != value) {
                _textFrequency = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textFrequency;
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
    //pick date
    public string PickDate {
        get => _pickDate;
        set {
            if (_pickDate != value) {
                _pickDate = value;
                OnPropertyChanged();
            }
        }
    }
    private string _pickDate;

    public User usr { get; set; }

    #endregion
    public ICommand CancelRecurringCommand { get; set; }
    public ICommand ConfirmRecurringCommand { get; set; }
    public ICommand LoadSourceCategoryCommand {  get; set; }
    public ICommand NewCategoryCommand { get; set; }

    public RecurringEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        usr = _accountStore.Users;
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();
        NewCategoryCommand = new NavigateModalCommand<ExpenseNewCategoryViewModel>(serviceProvider);

        LoadItemsource();
        SourceCategory = new();
        _expenseStore.TriggerNewCategory += () => LoadSourceCategory();
        CancelRecurringCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<object>(CloseModal);
        ConfirmRecurringCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<object>(NewRecurring);
        LoadSourceCategoryCommand = new CommunityToolkit.Mvvm.Input.RelayCommand<object>(LoadSourceCategory);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void LoadSourceCategory(object? parameter = null) {
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
                if (_expenseStore.Categorys != null) {
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
            
            //update recurring
            var re = DBManager.GetFirst<RecurringExpense>(rec => rec.UserID == usr.UserID && rec.RecurringExpenseID == _recurringStore.RecurringExpense.RecurringExpenseID);
            re.Name = NameRecurring;
            re.Interval = int.Parse(IntervalRecurring);
            re.StartDate = DateOnly.FromDateTime((DateTime)DateTimeRecurring);
            re.Frequency = SelectedFrequency;
            check = DBManager.Update(re);
            if (check == false) {
                throw new Exception("Update Recurring thất bại");
            }
            //create expense
            Expense exp = new Expense(long.Parse(AmountRecurring), NameRecurring, DateOnlyRecurring, SelectedCategory, _expenseStore.ExpenseBook, DescriptionRecurring);
            
            exp.RecurringExpenseID = _recurringStore.RecurringExpense.RecurringExpenseID;
            check = DBManager.Insert(exp);
            if (check == false) {
                throw new Exception("Thêm Expense thất bại");
            }
            _recurringStore.NotifyRecurring();
            _recurringStore.NotifyRecurringEdit();
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
        //load data
        var item = DBManager.GetFirst<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID && re.RecurringExpenseID == _recurringStore.RecurringExpense.RecurringExpenseID);
        NameRecurring = item.Name;
        IntervalRecurring = item.Interval.ToString();
        SelectedFrequency = item.Frequency;
        TextFrequency = item.Frequency.ToString();
        var exp = DBManager.GetFirst<Expense>(e => e.UserID == _accountStore.Users.UserID && e.RecurringExpenseID == item.RecurringExpenseID);
        PickDate = item.StartDate.ToString();
        AmountRecurring = exp.Amount.ToString();
        var cate = DBManager.GetFirst<Category>(c => c.UserID == _accountStore.Users.UserID && exp.CategoryID == c.CategoryID);
        CategoryRecurring = cate.Name;
        DescriptionRecurring = exp.Description;
    }
}
