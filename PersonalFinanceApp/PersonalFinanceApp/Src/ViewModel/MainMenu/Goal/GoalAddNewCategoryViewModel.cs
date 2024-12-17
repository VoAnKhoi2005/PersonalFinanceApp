using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 

public class GoalAddNewCategoryViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;

    #region Properties
    //name
    public string NewCategoryName {
        get => _newCategoryName;
        set {
            if(_newCategoryName != value) {
                _newCategoryName = value;
                OnPropertyChanged();
            }
        }
    }
    private string _newCategoryName;
    #endregion
    public ICommand CancelNewCategoryCommand { get; set; }
    public ICommand ConfirmNewCategoryCommand { get; set; }

    public GoalAddNewCategoryViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _goalStore = _serviceProvider.GetRequiredService<GoalStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        CancelNewCategoryCommand = new RelayCommand<object>(CloseModal);
        ConfirmNewCategoryCommand = new RelayCommand<object>(ConfirmNewCategoryGoal);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmNewCategoryGoal(object sender) {
        //add data to database
        GoalCategory goalCategory = new GoalCategory() {
            Name = NewCategoryName
        };
        DBManager.Insert(goalCategory);
        _goalStore.NewCategory = NewCategoryName;
        _goalStore.NotifyNewCategory();
        _modalNavigationStore.Close();
    }
}
