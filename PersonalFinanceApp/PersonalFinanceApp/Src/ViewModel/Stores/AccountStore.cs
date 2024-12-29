using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
    private string? _usersID;
    public string? UsersID {
        get => _usersID;
        set {
            if (_usersID != value) {
                _usersID = value;
                OnPropertyChanged(nameof(_usersID));
            }
        }
    }
    public event Action TriggerActionUploadSaving;
    public void UploadSaving() {
        TriggerActionUploadSaving = LoadSaving;
        TriggerActionUploadSaving?.Invoke();
    }
    public void LoadSaving() {
        //load saving in user
        try {
            long saving = 0;
            var items = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(UsersID));
            foreach (var item in items) {
                item.Expenses = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(UsersID) && item.Month == e.ExBMonth && item.Year == e.ExBYear);
                saving += item.Budget - item.Expenses.Sum(ex => ex.Amount);
            }
            var itemgoal = DBManager.GetCondition<Goal>(g => g.UserID == Users.UserID);
            foreach (var item in itemgoal) {
                saving += item.CurrentAmount;
            }
            var usr = DBManager.GetFirst<User>(u => u.UserID == int.Parse(UsersID));
            if (usr != null) { usr.Saving = saving; }
            DBManager.Update(usr);
            Users.Saving = saving;
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
