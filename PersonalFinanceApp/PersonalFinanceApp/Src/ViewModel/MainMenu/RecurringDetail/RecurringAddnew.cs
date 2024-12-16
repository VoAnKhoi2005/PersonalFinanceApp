using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using XAct.Messages;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class RecurringAddnew : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly AccountStore _accountStore;

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
    //name expense
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

    public User usr { get; set; }

    #endregion
    public ICommand CancelRecurringCommand {  get; set; }
    public ICommand ConfirmRecurringCommand { get; set; }
    public RecurringAddnew(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        usr = _accountStore.Users;

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
                if(check == false) {
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
            //create recurring
            RecurringExpense re = new RecurringExpense(NameRecurring, SelectedFrequency, int.Parse(IntervalRecurring), DateOnlyRecurring, int.Parse(_accountStore.UsersID));
            check = DBManager.Insert(re);
            if (check == false) {
                throw new Exception("Thêm Recurring thất bại");
            }
            //create expense
            
            Expense exp = new Expense(long.Parse(AmountRecurring), NameExpense, DateOnlyRecurring, cate, exB, DescriptionRecurring);
            var recurring = DBManager.GetFirst<RecurringExpense>(re => re.StartDate == DateOnlyRecurring && 
                                                                re.Name == NameRecurring && re.Frequency == SelectedFrequency && 
                                                                re.Interval == int.Parse(IntervalRecurring) && re.UserID == int.Parse(_accountStore.UsersID));
            exp.RecurringExpenseID = recurring.RecurringExpenseID;
            check = DBManager.Insert(exp);
            if (check == false) {
                throw new Exception("Thêm Expense thất bại");
            }
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
