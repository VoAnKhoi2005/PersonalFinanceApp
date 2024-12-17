using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.MainMenu;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PersonalFinanceApp.Src.ViewModel.Stores; 
public class GoalStore : INotifyPropertyChanged {
    public ObservableCollection<GoalplanCardViewModel> SharedGoal { get; set; } = new ObservableCollection<GoalplanCardViewModel>();

    private Goal? _goals;
    public Goal? Goals {
        get => _goals;
        set {
            if (_goals != value) {
                _goals = value;
                OnPropertyChanged(nameof(_goals));
            }
        }
    }
    private string? _goalid;
    public string? GoalID {
        get => _goalid;
        set {
            if (_goalid != value) {
                _goalid = value;
                OnPropertyChanged(nameof(_goalid));
            }
        }
    }
    private string? _newCategory;
    public string? NewCategory {
        get => _newCategory;
        set {
            if (_newCategory != value) {
                _newCategory = value;
                OnPropertyChanged(nameof(_newCategory));
            }
        }
    }
    //reload
    public event Action TriggerAction;
    public void NotifyGoal() {
        TriggerAction?.Invoke();
    }
    //new cateogry
    public event Action TriggerActionNewCategory;
    public void NotifyNewCategory() {
        TriggerActionNewCategory?.Invoke();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
