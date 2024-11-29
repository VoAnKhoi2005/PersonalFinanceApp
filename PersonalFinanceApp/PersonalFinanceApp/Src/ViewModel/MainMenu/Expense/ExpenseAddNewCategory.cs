using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseAddNewCategory : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    //name
    public string NameNewCategoryExpense {
        get => _nameNewCategoryExpense;
        set {
            if(_nameNewCategoryExpense != value) {
                _nameNewCategoryExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameNewCategoryExpense;
    public string ResourceNewCategoryExpense {
        get => _resourceNewCategoryExpense;
        set {
            if (_resourceNewCategoryExpense != value) {
                _resourceNewCategoryExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceNewCategoryExpense;
    public ICommand CancelNewCategoryExpenseCommand { get; set; }
    public ICommand ConfirmNewCategoryExpenseCommand { get; set; }

    public bool HasNoExpense { get; set; } = true;
    public ExpenseAddNewCategory(IServiceProvider serviceProvider) {
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
            Resources = ResourceNewCategoryExpense,
            UserID = _expenseStore.Expenses.UserID,
            ExBMonth = _expenseStore.Expenses.ExBMonth,
            ExBYear = _expenseStore.Expenses.ExBYear,
        };
        DBManager.Insert<Category>(newCategory);
        _modalNavigationStore.Close();
    }
}
