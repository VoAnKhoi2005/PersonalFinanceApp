using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalFinanceApp.ViewModel.Stores;

public class SharedDataService : INotifyPropertyChanged
{
    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            if (_message != value)
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}