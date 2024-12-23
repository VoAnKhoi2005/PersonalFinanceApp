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
using PersonalFinanceApp.Src.View;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class CategoryDetailViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;

    public User usr { get; set; }
    //selected categories
    public CategoryAdvance SelectedItem {
        get => _selectedItem;
        set {
            if (_selectedItem != value) {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
    }
    private CategoryAdvance _selectedItem;
    //source Categories
    public ObservableCollection<CategoryAdvance> SourceCategories {
        get => _sourceCategories;
        set {
            if (_sourceCategories != value) {
                _sourceCategories = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<CategoryAdvance> _sourceCategories;
    public ICommand DeleteCategories { get; set; }
    public ICommand CloseModalCommmand { get; set; }
    public ICommand AddNewCategoryCommand { get; set; }

    public CategoryDetailViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        usr = _accountStore.Users;
        SourceCategories = new();
        LoadData();
        DeleteCategories = new CommunityToolkit.Mvvm.Input.RelayCommand<object>(Delete);
        CloseModalCommmand = new CommunityToolkit.Mvvm.Input.RelayCommand<object>(CloseModal);
    }
    public void CloseModal(object? parameter = null) {
        _sharedService.Notify();
        _modalNavigationStore.Close();
    }
    public void LoadData(object? parameter = null) {
        try {
            SourceCategories.Clear();
            var items = DBManager.GetCondition<Category>(e => e.UserID == usr.UserID);
            foreach (var item in items) {
                SourceCategories.Add(new CategoryAdvance(item));
            }
        }
        catch (Exception ex) { 
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void Delete(object? parameter = null) {
        try {
            var result = MessageBox.Show("Are you sure delete category?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) {
                foreach (var exp in SelectedItem.cate.Expenses) {
                    DBManager.PermanentlyDelete(exp);
                }
                var categoryremove = DBManager.GetFirst<Category>(c => c.UserID == SelectedItem.cate.UserID && c.CategoryID == SelectedItem.cate.CategoryID);
                DBManager.Remove(categoryremove);
                LoadData();
            }
        }
        catch (Exception ex) { 
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public class CategoryAdvance {
        public CategoryAdvance(Category c) {
            cate = c;
            cate.Expenses = DBManager.GetCondition<Expense>(ex => ex.UserID == c.UserID && ex.CategoryID == c.CategoryID);
        }
        public Category cate { get; set; }
        public string nameCate => cate.Name;
        public string ExpenseBook => cate.ExBMonth.ToString() + "/" + cate.ExBYear.ToString();
        public string cExpenses => cate.Expenses.Count.ToString();

    }
}
