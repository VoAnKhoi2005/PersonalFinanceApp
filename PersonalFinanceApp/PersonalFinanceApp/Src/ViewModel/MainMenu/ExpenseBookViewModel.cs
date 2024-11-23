using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.Input;
using Windows.Devices.Sensors;
using System.Security.Cryptography.X509Certificates;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu; 
internal class ExpenseBookViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;



    //private ObservableCollection<Expense> _expenses = new ObservableCollection<Expense> { };
    public ObservableCollection<Expense> Expenses {get; set;}
        //get => _expenses;
        //set {
        //    _expenses = value;
        //    OnPropertyChanged();
        //}

    //}

    public ICommand AddNewExpenseBookCommand { get; set; }
    public ICommand RefreshCommand { get; set; }
    public ExpenseBookViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        AddNewExpenseBookCommand = new NavigateModalCommand<ExpenseBookAddNewViewModel>(serviceProvider);
        //Expenses = new ObservableCollection<Expense>();
        //Expense exb = new Expense() {
        //};
        Expenses = new ObservableCollection<Expense>();
        RefreshCommand = new RelayCommand(AddExpense);
    }


    public void AddExpense() {
        //Expenses.Add(new Expense {
        //    Name = "NewName",
        //    Amount = 5,
        //    DateEx = "17",
        //    Category = "hello world",
        //    Status = "success",
        //});
        var items = DBManager.GetAll<Expense>();
        var c = DBManager.GetAll<Expense>().Count();
        //Expenses = new(items);
        foreach(var item in items) {
            Expenses.Add(item);
        }

    }

}
