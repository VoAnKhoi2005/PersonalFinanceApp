using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PersonalFinanceApp.ViewModel.Stores;

public class SharedDataService : INotifyPropertyChanged
{
    public ObservableCollection<string> SharedList { get; } = new ObservableCollection<string>();

    public string? RandomCode
    {
        get => _randomCode;
        set
        {
            if (_randomCode != value)
            {
                _randomCode = value;
                OnPropertyChanged(nameof(RandomCode));
            }
        }
    }
    private string? _randomCode;
    public string? UserName {
        get => _userName;
        set {
            if (_userName != value) {
                _userName = value;
                OnPropertyChanged(nameof(_userName));
            }
        }
    }
    private string? _userName;

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}