using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public class SharedService {
    public event Action TriggerAction;
    public void Notify() {
        TriggerAction?.Invoke();
    }
}
