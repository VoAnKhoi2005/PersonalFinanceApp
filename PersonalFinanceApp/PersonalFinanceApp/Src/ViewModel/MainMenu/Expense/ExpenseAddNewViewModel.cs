using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Database;
using System.Collections.ObjectModel;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
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
    #endregion
    //Category
    public ObservableCollection<string> _itemsExpense = new ObservableCollection<string> {
        #region Category of expense
        "Ăn sáng", 
        "Ăn tiệm", 
        "Ăn tối", 
        "Ăn trưa", 
        "Cafe", 
        "Đi chợ / siêu thị", 
        "cho vay", 
        "Đồ chơi", 
        "Học phí", 
        "Sách vở", 
        "Sữa", 
        "Tiền tiêu vặt", 
        "Điện", 
        "Điện thoại cố định", 
        "Điện thoại di động", 
        "Gas", "Internet", 
        "Nước", 
        "Thuê người giúp việc", 
        "Truyền hình", 
        "Bảo hiểm xe", 
        "Gửi xe", 
        "Rửa xe", 
        "sửa chữa, bảo dưỡng xe", 
        "Taxi/ thuê xe", 
        "Xăng xe", 
        "Biếu tặng", 
        "Cưới xin", 
        "Ma chay", 
        "Thăm hỏi", 
        "Du lịch", 
        "Làm đẹp", 
        "Mỹ phẩm", 
        "Phim ảnh ca nhạc", 
        "Vui chơi giải trí", 
        "Phí chuyển khoản",
        "Mua sắm đồ đạc", 
        "Sửa chữa nhà cửa", 
        "Thuê nhà", 
        "Giao lưu, quan hệ", 
        "Học hành", 
        "Khám chữa bệnh", 
        "Thể thao", 
        "Thuốc men", 
        "Trả nợ", 
        "Giày dép", 
        "Phụ kiện khác", 
        "Quần áo", 
        "<New>"
        #endregion
    };
    public ObservableCollection<string> ItemsExpense {
        get => _itemsExpense;
        set {
            if (_itemsExpense != value) {
                _itemsExpense = value;
                OnPropertyChanged();
            }
        }
    }
    public string SelectedItemExpense { get; set; }
    public ICommand ExpenseCancelCommand { get; set; }
    public ICommand ExpenseConfirmCommand { get; set; }

    public ExpenseAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        ExpenseCancelCommand = new RelayCommand<object>(CloseModal);
        ExpenseConfirmCommand = new RelayCommand<object>(ConfirmAddNewExpense);
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private Category ca() {
        Category category = new Category();
        return category;
    }
    private ExpensesBook exB() {
        ExpensesBook exBook = new ExpensesBook();
        return exBook;
    }
    private void ConfirmAddNewExpense(object sender) {
        //add data to database
        var expense = new Expense() {
            Amount = int.Parse(AmountExpense),
            Name = NameExpense,
            Date = DateOnlyExpenseBook,
            //Recurring = recurring;
            //CategoryID = ca.CategoryID;
            //ExBMonth = exB.Month;
            //ExBYear = exB.Year;
            //UserID = exB.UserID;
            Description = DescriptionExpense,
            TimeAdded = DateTime.Now,
            Resources = ResourceExpense,
            Deleted = false,
        };
        DBManager.Insert(expense);
        _modalNavigationStore.Close();
    }
}