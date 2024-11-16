using System.Windows.Input;
namespace PersonalFinanceApp.ViewModel.Command;

public class RelayCommand<T> : ICommand
{
    private readonly Predicate<T> _canExecute;
    private readonly Action<T> _execute;

    public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
    {
        if (canExecute == null)
            _canExecute = obj => true;
        else
            _canExecute = canExecute;

        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    }

    public bool CanExecute(object parameter) => _canExecute((T)parameter);

    public void Execute(object parameter) => _execute((T)parameter);

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}