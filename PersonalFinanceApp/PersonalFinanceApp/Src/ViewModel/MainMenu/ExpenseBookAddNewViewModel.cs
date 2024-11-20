using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu; 
internal class ExpenseBookAddNewViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;

    public ICommand ExpenseBookCancelCommand { get; set; }

    public ExpenseBookAddNewViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        ExpenseBookCancelCommand = new RelayCommand<object>(CloseModal);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
}
