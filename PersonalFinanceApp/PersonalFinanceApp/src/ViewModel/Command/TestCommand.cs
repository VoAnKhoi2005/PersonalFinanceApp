using System.Windows;
using PersonalFinanceApp.View;

namespace PersonalFinanceApp.ViewModel.Command;

public class TestCommand : BaseCommand
{
    public static TestCommand Instance { get; } = new TestCommand();

    private TestCommand() { }

    public override void Execute(object parameter)
    {
        AccessDataContext();
    }

    private void AccessDataContext()
    {
        var window = Application.Current.Windows.OfType<LoginWindow>().FirstOrDefault();

        if (window != null)
        {
            var dataContext = window.DataContext;
        }
        else
        {
            Console.WriteLine("MainWindow not found in Application.Current.Windows.");
        }
    }
}
