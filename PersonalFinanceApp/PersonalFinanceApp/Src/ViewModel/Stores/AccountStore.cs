using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public class AccountStore : INotifyPropertyChanged {
    public ObservableCollection<string> SharedUser { get; } = new ObservableCollection<string>();

    private string? _userId;
    public string? UserId {
        get => _userId;
        set {
            if (_userId != value) {
                _userId = value;
                OnPropertyChanged(nameof(_userId));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
