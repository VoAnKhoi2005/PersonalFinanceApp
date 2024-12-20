using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using Syncfusion.Windows.Controls.Input;
using Microsoft.Office.Interop.Excel;


namespace PersonalFinanceApp.ViewModel.MainMenu;
public class RecurringDetailViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;

   
    public ObservableCollection<RecurringExpense> SourceRecurringExpense {
        get => _sourceRecurringExpense;
        set {
            if (value != _sourceRecurringExpense) {
                _sourceRecurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<RecurringExpense> _sourceRecurringExpense;

    public RecurringExpense SelectedRecurringExpense {
        get => _selectedRecurringExpense;
        set {
            if (value != _selectedRecurringExpense) {
                _selectedRecurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private RecurringExpense _selectedRecurringExpense;
    public string CountRecurring {
        get => _countRecurring;
        set {
            if (value != _countRecurring) {
                _countRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _countRecurring;
    public ObservableCollection<string> SourceStatusFilter {
        get => _sourceStatusFilter;
        set {
            _sourceStatusFilter = value;
            OnPropertyChanged();
        }
    }
    private ObservableCollection<string> _sourceStatusFilter;
    public string SelectedStatusFilter {
        get => _selectedStatusFilter;
        set {
            if (value != _selectedStatusFilter) {
                _selectedStatusFilter = value;
                OnPropertyChanged();
            }
        }
    }
    private string _selectedStatusFilter;
    public string TextStatus {
        get => _textStatus;
        set {
            if (value != _textStatus) {
                _textStatus = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textStatus;
    public ICommand ExitRecurringExpenseCommand { get; set; }
    public ICommand EditRecurringExpenseCommand { get; set; }
    public ICommand EditRecurringCommand { get; set; }
    public ICommand SuspendRecurringExpenseCommand { get; set; }
    public ICommand DeleteRecurringExpenseCommand { get; set; }
    public ICommand LoadDataRecurringCommand { get; set; }
    public ICommand SelectedChangedFilter { get; set; }

    public RecurringDetailViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        SourceStatusFilter = new();
        SourceRecurringExpense = new();

        TextStatus = "All";
        SelectedStatusFilter = "All";

        SourceStatusFilter.Add("All");
        SourceStatusFilter.Add("Active");
        SourceStatusFilter.Add("Canceled");


        EditRecurringCommand = new NavigateModalCommand<RecurringEditViewModel>(serviceProvider);
        SelectedChangedFilter = new RelayCommand<object>(SelectedChanged);
        LoadDataRecurring();

        _recurringStore.TriggerEditRecurring += LoadDataRecurring;

        EditRecurringExpenseCommand = new RelayCommand<object>(SelectedRecurring);
        SuspendRecurringExpenseCommand = new RelayCommand<object>(SuspendRecurring);
        DeleteRecurringExpenseCommand = new RelayCommand<object>(DeleteRecurring);
        ExitRecurringExpenseCommand = new RelayCommand<object>(CloseModal);
    }
    public void SelectedChanged(object? parameter = null) {
        LoadDataRecurring();
    }
    public void CloseModal(object? parameter = null) {
        _recurringStore.NotifyRecurring();
        _modalNavigationStore.Close();
    }
    public void LoadDataRecurring() {

        SourceRecurringExpense.Clear();

        if (SelectedStatusFilter.CompareTo("All") == 0) {
            var items = DBManager.GetCondition<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID);
            foreach (var item in items) {
                SourceRecurringExpense.Add(item);
            }
        }else if(SelectedStatusFilter.CompareTo("Active") == 0) { 
            var items = DBManager.GetCondition<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID && re.Status.CompareTo("Active") == 0);
            foreach (var item in items) {
                SourceRecurringExpense.Add(item);
            }
        }
        else {
            var items = DBManager.GetCondition<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID && re.Status.CompareTo("Canceled") == 0);
            foreach (var item in items) {
                SourceRecurringExpense.Add(item);
            }
        }
        
        CountRecurring = SourceRecurringExpense.Count.ToString();
    }
    public void SelectedRecurring(object? parameter = null) {
        _recurringStore.RecurringExpense = SelectedRecurringExpense;
        EditRecurringCommand.Execute(this);
    }
    public void SuspendRecurring(object? parameter = null) {
        try {
            var rec = DBManager.GetFirst<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID &&
                        re.RecurringExpenseID == SelectedRecurringExpense.RecurringExpenseID);
            if (SelectedRecurringExpense.Status.CompareTo("Active") == 0) {
                rec.Status = "Canceled";

                bool check = DBManager.Update(rec);

                if (!check) throw new Exception("Update Status failed");
            }
            else {
                //SelectedRecurringExpense.Status.CompareTo("Canceled")
                rec.Status = "Active";
                while (rec.LastTime < DateOnly.FromDateTime(DateTime.Today)) {

                    if (rec.Frequency.CompareTo("Daily") == 0) {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Weekly") == 0) {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval * 7);
                    }
                    else if (rec.Frequency.CompareTo("Monthly") == 0) {
                        rec.LastTime = rec.LastTime.AddMonths(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Yearly") == 0) {
                        rec.LastTime = rec.LastTime.AddYears(rec.Interval);
                    }
                }
                bool check = DBManager.Update(rec);

                if (!check) throw new Exception("Update Status failed");
            }
            LoadDataRecurring();
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }

    public void DeleteRecurring(object? parameter = null) {
        try {
            var result = MessageBox.Show("Are you sure about that!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes) {
                var items = DBManager.GetCondition<Expense>(e => e.UserID == _accountStore.Users.UserID && e.RecurringExpenseID == SelectedRecurringExpense.RecurringExpenseID);
                foreach (var item in items) {
                    item.RecurringExpenseID = null;
                    DBManager.Update(item);
                }
                var recurring = DBManager.GetFirst<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID && re.RecurringExpenseID == SelectedRecurringExpense.RecurringExpenseID);
                DBManager.Remove(recurring);
            }
            LoadDataRecurring();
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
}
