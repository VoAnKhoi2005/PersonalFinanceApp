using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
internal class ExpenseBookViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    public ICommand AddNewExpenseBookCommand { get; set; }

    public bool HasNoExpense { get; set; } = true;

    public ExpenseBookViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        AddNewExpenseBookCommand = new NavigateModalCommand<ExpenseBookAddNewViewModel>(serviceProvider);
    }

}
