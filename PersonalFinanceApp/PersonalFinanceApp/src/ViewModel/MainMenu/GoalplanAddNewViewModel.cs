using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanAddNewViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;

    public ICommand CancelCommand { get; set; }

    public GoalplanAddNewViewModel(IServiceProvider serviceProvider)
    {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        CancelCommand = new RelayCommand<object>(CloseModal);
    }

    private void CloseModal(object sender)
    {
        _modalNavigationStore.Close();
    }
}