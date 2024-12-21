using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseNewCategoryViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    //name
    public string NameNewCategoryExpense {
        get => _nameNewCategoryExpense;
        set {
            if (_nameNewCategoryExpense != value) {
                _nameNewCategoryExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameNewCategoryExpense;
    public ICommand CancelNewCategoryExpenseCommand { get; set; }
    public ICommand ConfirmNewCategoryExpenseCommand { get; set; }

    public ExpenseNewCategoryViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelNewCategoryExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmNewCategoryExpenseCommand = new RelayCommand<object>(ConfirmNewCategoryExpense);

    }
    public void CloseModal(object parameter) {
        _modalNavigationStore.Close();
    }
    public void ConfirmNewCategoryExpense(object parameter) {
        //add new category
        var newCategory = new Category() {
            Name = NameNewCategoryExpense,
            UserID = _expenseStore.ExpenseBook.UserID,
            ExBMonth = _expenseStore.ExpenseBook.Month,
            ExBYear = _expenseStore.ExpenseBook.Year,
        };
        DBManager.Insert(newCategory);
        _expenseStore.Categorys = newCategory;
        _expenseStore.NotifyHaveNewCategory();
        _modalNavigationStore.Close();
    }
}