
using PersonalFinanceApp.View;
using System.ComponentModel;
using System.Windows;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public class SharedService : INotifyPropertyChanged {
    public event Action TriggerAction;
    public void Notify() {
        TriggerAction?.Invoke();
    }
    public event Action TriggerActionNotification;
    public void NotifyNotification() {
        TriggerActionNotification?.Invoke();
    }
    public Window? w {  get; set; } // login window
    public MainWindow? m { get; set; } // main window

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
