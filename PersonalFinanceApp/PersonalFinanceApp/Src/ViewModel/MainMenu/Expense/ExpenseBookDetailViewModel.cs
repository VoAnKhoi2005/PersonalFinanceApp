using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseBookDetailViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;

    public User usr { get; set; }
    //selected expense book
    public ExpenseBookAdvance SelectedItem {
        get => _selectedItem;
        set {
            if (_selectedItem != value) {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseBookAdvance _selectedItem;
    //source expense book
    public ObservableCollection<ExpenseBookAdvance> SourceExpenseBook {
        get => _sourceExpenseBook;
        set {
            if (_sourceExpenseBook != value) {
                _sourceExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpenseBookAdvance> _sourceExpenseBook;
    public ICommand DeleteExpenseBook { get; set; }
    public ICommand CloseModalCommmand { get; set; }

    public ExpenseBookDetailViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        usr = _accountStore.Users;
        SourceExpenseBook = new();
        LoadData();
        DeleteExpenseBook = new RelayCommand<object>(Delete);
        CloseModalCommmand = new RelayCommand<object>(CloseModal);
    }
    public void CloseModal(object? parameter = null) {
        _sharedService.Notify();
        _modalNavigationStore.Close();
    }
    public void LoadData(object? parameter = null) {
        try {
            SourceExpenseBook.Clear();
            var items = DBManager.GetCondition<ExpensesBook>(e => e.UserID == usr.UserID);
            foreach (var item in items) {
                SourceExpenseBook.Add(new ExpenseBookAdvance(item));
            }
        }
        catch (Exception ex) { 
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void Delete(object? parameter = null) {
        try {
            var result = MessageBox.Show("Are you sure delete expensebook?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) {
                foreach (var exp in SelectedItem.exB.Expenses) {
                    DBManager.PermanentlyDelete(exp);
                }
                foreach (var ca in SelectedItem.exB.Categories) {
                    DBManager.Remove(ca);
                }
                var exBremove = DBManager.GetFirst<ExpensesBook>(e => e.UserID == SelectedItem.exB.UserID && e.Month == SelectedItem.exB.Month && e.Year == SelectedItem.exB.Year);
                DBManager.Remove(exBremove);
                LoadData();
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }
    public class ExpenseBookAdvance {
        public ExpenseBookAdvance(ExpensesBook e) {
            exB = e;
            exB.Categories = DBManager.GetCondition<Category>(ex => ex.UserID == e.UserID && ex.ExBMonth == e.Month && ex.ExBYear == e.Year);
            exB.Expenses = DBManager.GetCondition<Expense>(ex => ex.UserID == e.UserID && ex.ExBMonth == e.Month && ex.ExBYear == e.Year);
        }
        public ExpensesBook exB { get; set; }
        public string date => exB.Month.ToString() + "/" + exB.Year.ToString();
        public string Budget => exB.Budget.ToString();
        public string cExpenses => exB.Expenses.Count.ToString();
        public string cCategories => exB.Categories.Count.ToString();

    }
}
