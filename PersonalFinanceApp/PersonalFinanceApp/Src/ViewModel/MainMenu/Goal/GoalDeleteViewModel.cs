using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel;
public class GoalDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;

    public ICommand CancelDeleteGoalCommand { get; set; }
    public ICommand ConfirmDeleteGoalCommand { get; set; }
    GoalDeleteViewModel(IServiceProvider serviceProvider) {

        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelDeleteGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmDeleteGoalCommand = new RelayCommand<object>(ConfirmDeleteGoal);
    }
    public void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    public void ConfirmDeleteGoal(object sender) {
        //Delete data in database
        _modalNavigationStore.Close();
    } 
}
