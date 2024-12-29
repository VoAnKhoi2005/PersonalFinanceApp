using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Database;
using System.Collections.ObjectModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class NotificationRecurringCard : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;
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
    public string StartDate {
        get => _startDate;
        set {
            if (_startDate != value) {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }
    private string _startDate;
    public string LastTime {
        get => _lastTime;
        set {
            if (_lastTime != value) {
                _lastTime = value;
                OnPropertyChanged();
            }
        }
    }
    private string _lastTime;
    public ObservableCollection<Category> SourceCategories {
        get => _sourceCategories;
        set {
            if (_sourceCategories != value) {
                _sourceCategories = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Category> _sourceCategories;
    public Category SelectionCategory {
        get => _selectedCategory;
        set {
            if (_selectedCategory != value) {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private Category _selectedCategory;
    public RecurringExpense Rec {
        get => _rec; 
        set {
            if (_rec != value) {
                _rec = value;
                OnPropertyChanged();
            }
        }
    }
    private RecurringExpense _rec;


    public ICommand CreateCommand { get; set; }
    public ICommand CancelCommand { get; set; }

    public NotificationRecurringCard(IServiceProvider serviceProvider, RecurringExpense re) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();
        Rec = new();
        Rec.LastTime = re.LastTime;
        Rec.UserID = re.UserID;
        Rec.RecurringExpenseID = re.RecurringExpenseID;
        SourceCategories = new();
        LoadRecurring(re);
        CreateCommand = new RelayCommand(CreateExpense);
        CancelCommand = new RelayCommand(CancelExpense);
    }

    private void CreateExpense()
    {
        try {
            var recurring = DBManager.GetFirst<RecurringExpense>(re => re.UserID == Rec.UserID && re.RecurringExpenseID == Rec.RecurringExpenseID);
            if (recurring.LastTime < Rec.LastTime) {
                recurring.LastTime = Rec.LastTime;
                DBManager.Update(recurring);
            }
            var exp = DBManager.GetFirst<Expense>(e => e.UserID == Rec.UserID && e.RecurringExpenseID == Rec.RecurringExpenseID);
            if (exp != null)
            {
                if (SelectionCategory == null)
                    throw new Exception("Category không hợp lệ.");
                var expense = new Expense() {
                    Amount = exp.Amount,
                    Name = exp.Name,
                    Description = exp.Description,
                    Date = Rec.LastTime,
                    TimeAdded = DateTime.Now,
                    Deleted = false,
                    CategoryID = SelectionCategory.CategoryID,
                    ExBMonth = exp.ExBMonth,
                    ExBYear = exp.ExBYear,
                    UserID = _accountStore.Users.UserID,
                    RecurringExpenseID = Rec.RecurringExpenseID,
                };
                DBManager.Insert<Expense>(expense);
                _recurringStore.ShareRecurring.Remove(this);
                _recurringStore.NotifyUpload();
            }
        }
        catch(Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void CancelExpense()
    {
        _recurringStore.ShareRecurring.Remove(this);
        _recurringStore.NotifyUpload();
    }

    public void LoadRecurring(RecurringExpense re) {
        try {
            var cate = DBManager.GetCondition<Category>(c => c.UserID == re.UserID && c.ExBMonth == re.LastTime.Month && c.ExBYear == re.LastTime.Year);
            SourceCategories.Clear();
            NameRecurring = re.Name;
            StartDate = re.StartDate.ToString("dd/MM/yyyy");
            LastTime = re.LastTime.ToString("dd/MM/yyyy");
            foreach (var c in cate) {
                SourceCategories.Add(c);
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }
}
