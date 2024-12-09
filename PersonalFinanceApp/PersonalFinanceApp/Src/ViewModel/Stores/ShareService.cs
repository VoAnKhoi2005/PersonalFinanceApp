
using System.Windows;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public class SharedService {
    public event Action TriggerAction;
    public void Notify() {
        TriggerAction?.Invoke();
    }
    public Window? w {  get; set; }
}
