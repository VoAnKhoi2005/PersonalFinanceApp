using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Input;


namespace PersonalFinanceApp.ViewModel.MainMenu;
public class RecurringEditViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;

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
    public RecurringEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        usr = _accountStore.Users;
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        LoadItemsource();

        CancelRecurringCommand = new RelayCommand<object>(CloseModal);
        ConfirmRecurringCommand = new RelayCommand<object>(NewRecurring);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void NewRecurring(object? parameter = null) {
        try {
            bool check = true;
            //create expensebook
            var exB = DBManager.GetFirst<ExpensesBook>(ex => ex.UserID == usr.UserID &&
                                                       ex.Month == DateTimeRecurring.Value.Month &&
                                                       ex.Year == DateTimeRecurring.Value.Year);
            if (exB == null) {
                exB = new ExpensesBook(DateTimeRecurring.Value.Month, DateTimeRecurring.Value.Year,
                                        _accountStore.Users, _accountStore.Users.DefaultBudget);
                if (long.Parse(AmountRecurring) > exB.Budget) {
                    var result = MessageBox.Show("Ngân sách sẽ âm nếu bạn thêm vào, bạn có muốn tiếp tục không", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.No) {
                        return;
                    }
                }
                check = DBManager.Insert(exB);
                if (check == false) {
                    throw new Exception("Thêm ExpenseBook thất bại");
                }
            }
            //create catgory
            var cate = DBManager.GetFirst<Category>(c => c.UserID == usr.UserID && c.ExBYear == exB.Year &&
                                                    c.ExBMonth == exB.Month && c.Name == CategoryRecurring);
            if (cate == null) {
                cate = new Category(CategoryRecurring, exB);
                check = DBManager.Insert(cate);
                if (check == false) {
                    throw new Exception("Thêm Category thất bại");
                }
            }
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
            Expense exp = new Expense(long.Parse(AmountRecurring), NameRecurring, DateOnlyRecurring, cate, exB, DescriptionRecurring);
            
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
