using System.Windows;
using System.Windows.Controls;
using PersonalFinanceApp.View;

namespace PersonalFinanceApp.ViewModel.Command;

public class TestCommand : BaseCommand
{
    public static TestCommand Instance { get; } = new TestCommand();

    public TestCommand() { }

    public override void Execute(object parameter)
    {
        AccessDataContext();
    }

    private void AccessDataContext()
    {
        var window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        var window2 = Application.Current.MainWindow;
        if (window != null)
        {
            var dataContext = window.DataContext;

            foreach (var item in window.TabControl1.Items)
            {
                if (item is TabItem tabItem)
                {
                    var dataContext1 = tabItem.DataContext;
                }
                else
                {
                    Console.WriteLine("MainWindow not found in Application.Current.Windows.");
                }
            }
        }
    }
}