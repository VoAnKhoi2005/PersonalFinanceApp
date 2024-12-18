using System.ComponentModel;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public class ThemeStore : INotifyPropertyChanged {
    public bool isLightTheme = true;

    public event Action TriggerAction;

    public void Notify() {
        if (isLightTheme) {
            TriggerAction = ChangedThemeDark; 
        }
        else {
            TriggerAction = ChangedThemeLight; 
        }

        TriggerAction?.Invoke();
    }
    public void ChangedThemeLight() {
        Apptheme.ChangeTheme(new Uri("Resources/Light.xaml", UriKind.Relative));
        isLightTheme = true; 
    }

    public void ChangedThemeDark() {
        Apptheme.ChangeTheme(new Uri("Resources/Dark.xaml", UriKind.Relative));
        isLightTheme = false; 
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
