using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu; 
internal class ExpenseBookAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;

    public ICommand ExpenseBookCancelCommand { get; set; }
    public ICommand ExpenseBookConfirmCommand { get; set; }

    public ExpenseBookAddNewViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        ExpenseBookCancelCommand = new RelayCommand<object>(CloseModal);
        ExpenseBookConfirmCommand = new RelayCommand<object>(Confirm);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void Confirm(object sender) {
        //add new datagrid
        _modalNavigationStore.Close();
    }
}
