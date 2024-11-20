using PersonalFinanceApp.Database;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual void Dispose() { }
}