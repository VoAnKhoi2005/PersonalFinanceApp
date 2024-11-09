using PersonalFinanceApp.Src.ViewModel;
using PersonalFinanceApp.ViewModel.LoginMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel {
    public class MainViewModel : BaseViewModel {
        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }

        public MainViewModel() {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                Isloaded = true;
                if (p == null)
                    return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                p.Show();
                if (loginWindow.DataContext == null)
                    return;

                var loginVM = loginWindow.DataContext as LoginMainViewModel;

                if (loginVM.IsLogin) {
                    p.Show();
                }
                else {
                    p.Close();
                }
            }
              );

        }
    }
}
