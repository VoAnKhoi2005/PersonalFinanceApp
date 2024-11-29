using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseEditViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseBookStore _expenseBookStore;
    private readonly ExpenseStore _expenseStore;
    #region Properties
    //name
    public string NameEditExpense {
        get => _nameEditExpense;
        set {
            if (_nameEditExpense != value) {
                _nameEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameEditExpense;
    //amount
    public string AmountEditExpense {
        get => _amountEditExpense;
        set {
            if (_amountEditExpense != value) {
                _amountEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountEditExpense;
    //date
    public DateTime? DateTimeEditExpenseBook {
        get => _dateTimeEditExpenseBook;
        set {
            _dateTimeEditExpenseBook = value;
            OnPropertyChanged(nameof(DateTimeEditExpenseBook));
            DateOnlyExpenseBook = DateOnly.FromDateTime(_dateTimeEditExpenseBook.Value);
        }
    }
    private DateTime? _dateTimeEditExpenseBook;
    public DateOnly DateOnlyExpenseBook {
        get => _dateOnlyExpenseBook;
        private set {
            _dateOnlyExpenseBook = value;
            OnPropertyChanged(nameof(DateOnlyExpenseBook));
        }
    }
    private DateOnly _dateOnlyExpenseBook;
    //description
    public string DescriptionEditExpense {
        get => _descriptionEditExpense;
        set {
            if (_descriptionEditExpense != value) {
                _descriptionEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionEditExpense;
    //category
    public string CategoryEditExpense {
        get => _categoryEditExpense;
        set {
            if (_categoryEditExpense != value) {
                _categoryEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryEditExpense;
    //resource
    public string ResourceEditExpense {
        get => _resourceEditExpense;
        set {
            if (_resourceEditExpense != value) {
                _resourceEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceEditExpense;
    //recurring
    public bool RecurringEditExpense {
        get => _recurringEditExpense;
        set {
            if (_recurringEditExpense != value) {
                _recurringEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _recurringEditExpense;
    //category
    public int SelectedEditCategory {
        get => _selectedEditCategory;
        set {
            if (_selectedEditCategory != value) {
                _selectedEditCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private int _selectedEditCategory;
    //itemsource
    public ObservableCollection<CategoryItem> ItemsEditExpense {
        get => _itemsEditExpense;
        set {
            if (_itemsEditExpense != value) {
                _itemsEditExpense = value;
                OnPropertyChanged();
            }
        }
    }
    public ObservableCollection<CategoryItem> _itemsEditExpense = new();
    #endregion
    public ICommand CancelEditExpenseCommand { get; set; }
    public ICommand ConfirmEditExpenseCommand { get; set; }

    public ExpenseEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadItemSource();
        CancelEditExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditExpenseCommand = new RelayCommand<object>(ConfirmEditExpense);
    }
    public void LoadItemSource() {
        //itemsource category
        ItemsEditExpense.Clear();
        ItemsEditExpense.Add(new CategoryItem { Id = -1, Name = "<New>" });
        var items = DBManager.GetCondition<Category>(c => c.UserID == _expenseBookStore.ExpenseBook.UserID && c.ExBMonth == _expenseBookStore.ExpenseBook.Month && c.ExBYear == _expenseBookStore.ExpenseBook.Year);
        foreach (var item in items) {
            ItemsEditExpense.Add(new CategoryItem { Id = item.CategoryID, Name = item.Name });
        }
        //load data
        var itemExP = _expenseStore.Expenses;
        //var cgr = DBManager.GetFirst<Category>(c => itemExP.CategoryID == c.CategoryID && c.UserID == itemExP.UserID && c.ExBYear == itemExP.ExBYear && c.ExBMonth == itemExP.ExBMonth);
        if (itemExP != null) {
            AmountEditExpense = itemExP.Amount.ToString();
            NameEditExpense = itemExP.Name;
            DateTimeEditExpenseBook = itemExP.Date.ToDateTime(TimeOnly.MinValue);
            //Recurring = recurring;
            //SelectedEditCategory = cgr.Name;
            DescriptionEditExpense = itemExP.Description;
            ResourceEditExpense = itemExP.Resources;
        }
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditExpense(object sender) {
        //add data to database

        var itemExP = DBManager.GetFirst<Expense>(e => e.ExpenseID == _expenseStore.Expenses.ExpenseID && e.UserID == _expenseStore.Expenses.UserID);
        itemExP.Amount = long.Parse(AmountEditExpense);
        itemExP.Name = NameEditExpense;
        itemExP.Description = DescriptionEditExpense;
        //itemExP.Recurring = true;
        itemExP.Resources = ResourceEditExpense;
        itemExP.CategoryID = SelectedEditCategory;
        itemExP.Date = DateOnlyExpenseBook;

        DBManager.Update<Expense>(itemExP);
        _modalNavigationStore.Close();
    }
    public class CategoryItem {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}