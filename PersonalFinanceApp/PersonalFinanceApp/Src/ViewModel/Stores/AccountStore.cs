using PersonalFinanceApp.Model;
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

    private User? _users;
    public User? Users {
        get => _users;
        set {
            if (_users != value) {
                _users = value;
                OnPropertyChanged(nameof(_users));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
