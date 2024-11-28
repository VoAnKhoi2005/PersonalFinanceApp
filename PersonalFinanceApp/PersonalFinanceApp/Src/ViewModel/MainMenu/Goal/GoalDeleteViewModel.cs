using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class GoalDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;
    public ICommand CancelDeleteGoalCommand { get; set; }
    public ICommand ConfirmDeleteGoalCommand { get; set; }
    public GoalDeleteViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();

        CancelDeleteGoalCommand = new RelayCommand<object>(CloseModal);
        ConfirmDeleteGoalCommand = new RelayCommand<object>(ConfirmDeleteGoal);
    }
    public void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    public void ConfirmDeleteGoal(object sender) {
        //Delete data in database
        var goaldelete = DBManager.GetFirst<Goal>(g => g.GoalID == int.Parse(_goalStore.GoalID));
        DBManager.Remove<Goal>(goaldelete);
        _modalNavigationStore.Close();
    } 
}
