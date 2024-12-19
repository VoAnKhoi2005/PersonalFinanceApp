using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Controls;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using System.Windows;
using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using Microsoft.Win32;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Data;
using OfficeOpenXml;
using System.IO;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingExportToExcelViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseStore _expenseStore;
    #region Properties
    public int i { get; set; } = 0;
    //have expenseBook
    public bool HaveExpenseBook {
        get => _haveExpenseBook;
        set {
            if (_haveExpenseBook != value) {
                _haveExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _haveExpenseBook;
    //source expense book
    public ObservableCollection<ExpenseBookAdvance> SourceExpenseBook {
        get => _sourceExpenseBook;
        set {
            if (value != _sourceExpenseBook) {

                _sourceExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpenseBookAdvance> _sourceExpenseBook = new();
    //source datagrid
    public ObservableCollection<ExpenseAdvance> SourceDatagrid {
        get => _sourceDatagrid;
        set {
            if (value != _sourceDatagrid) {

                _sourceDatagrid = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpenseAdvance> _sourceDatagrid = new();
    //selected exb
    public ExpenseBookAdvance SelectedExpenseBook {
        get => _selectedExpenseBook;
        set {
            if (_selectedExpenseBook != value) {
                if (value != null) {
                    _expenseStore.ExpenseBook = value.expensesBook;
                }
                _selectedExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseBookAdvance _selectedExpenseBook;
    //text exb
    public string TextExpenseBook {
        get => _textExpenseBook;
        set {
            if (_textExpenseBook != value) {
                _textExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textExpenseBook;
    #endregion
    #region Command
    public ICommand ExportFileCommand { get; set; }
    public ICommand BackModalCommand { get; set; }
    public ICommand ExpenseBookChangedCommand { get; set; }
    #endregion
    public SettingExportToExcelViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadExpenseBook();
        GetNewest();
        BackModalCommand = new RelayCommand<object>(CloseModal);
        ExportFileCommand = new RelayCommand<DataGrid>(ExportToExcel);
        ExpenseBookChangedCommand = new RelayCommand<object>(ExpenseBookChanged);
    }
    public void ExpenseBookChanged(object? parameter = null) {
        LoadData();
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void ExportToExcel(DataGrid dataGrid) {
        try {
            // Check if the DataGrid has data
            if (dataGrid.Items.Count == 0) {
                System.Windows.MessageBox.Show("No data to export.", "Export", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Configure the SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Save Excel File",
                FileName = "DataGridExport.xlsx"
            };

            // Show the dialog and check if the user clicked Save
            if (saveFileDialog.ShowDialog() == true) {
                string filePath = saveFileDialog.FileName;

                // Set the EPPlus license context
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Use EPPlus to create the Excel file
                using (ExcelPackage package = new ExcelPackage()) {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("DataGridExport");

                    // Add headers
                    for (int i = 0; i < dataGrid.Columns.Count; i++) {
                        worksheet.Cells[1, i + 1].Value = dataGrid.Columns[i].Header.ToString();
                    }

                    // Add data rows
                    for (int i = 0; i < dataGrid.Items.Count; i++) {
                        var row = dataGrid.Items[i];
                        if (row is not null) {
                            for (int j = 0; j < dataGrid.Columns.Count; j++) {
                                var cellContent = dataGrid.Columns[j].GetCellContent(row) as TextBlock;
                                if (cellContent != null) {
                                    worksheet.Cells[i + 2, j + 1].Value = cellContent.Text;
                                }
                            }
                        }
                    }

                    // Save the Excel file
                    FileInfo file = new FileInfo(filePath);
                    package.SaveAs(file);

                    System.Windows.MessageBox.Show("Data exported successfully!", "Export", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        catch(Exception ex) {
            MessageBox.Show($"Have error please try again: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void LoadExpenseBook() {
        SourceExpenseBook.Clear();
        var exBitem = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
        foreach (var item in exBitem) {
            SourceExpenseBook.Add(new ExpenseBookAdvance(item));
        }
        SortObservableCollection(SourceExpenseBook);
        if (_expenseStore.ExpenseBook != null) {
            ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook);
            SelectedExpenseBook = exbA;
            TextExpenseBook = exbA.DateExB;
        }
    }
    public void LoadData() {
        i = 1;
        //load data to datagrid
        SourceDatagrid.Clear();
        var exB = _expenseStore.ExpenseBook;
        var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID) && exB.Month == exp.ExBMonth && exB.Year == exp.ExBYear);
        foreach (var item in items) {
            SourceDatagrid.Add(new ExpenseAdvance(i, item));
            i++;
        }
        //expensebook
        if (_expenseStore.ExpenseBook != null) {
            ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook);
            SelectedExpenseBook = exbA;
            TextExpenseBook = exbA.DateExB;
        }
    }
    public void GetNewest() {
        var items = DBManager.GetCondition<ExpensesBook>(e => int.Parse(_accountStore.UsersID) == e.UserID);
        if (items.Count == 0) {
            HaveExpenseBook = false;
            return;
        }
        else {
            HaveExpenseBook = true;
            ExpensesBook itemmax = items[0];
            foreach (var item in items) {
                if (item.Year > itemmax.Year || (item.Month > itemmax.Month && item.Year == itemmax.Year)) {
                    itemmax = item;
                }
            }
            _expenseStore.ExpenseBook = itemmax;
            LoadData();
        }
    }
    static void SortObservableCollection(ObservableCollection<ExpenseBookAdvance> collection) {
        var sortedList = collection.OrderBy(x => x).ToList();

        collection.Clear();

        foreach (var item in sortedList) {
            collection.Add(item);
        }
    }
    public class ExpenseBookAdvance : IComparable<ExpenseBookAdvance> {
        public ExpenseBookAdvance(ExpensesBook e) {
            DateExB = e.Month.ToString() + "/" + e.Year.ToString();
            expensesBook = e;
            _month = e.Month;
            _year = e.Year;
        }
        public int _month { get; set; }
        public int _year { get; set; }
        public string DateExB { get; set; }
        public virtual ExpensesBook? ExpensesBook { get; set; }
        public ExpensesBook expensesBook { get; set; }
        public int CompareTo(ExpenseBookAdvance other) {
            if (other == null) return 1;

            if (_year == other._year) {
                return _month.CompareTo(other._month);
            }
            return _year.CompareTo(other._year);
        }
    }
    public class ExpenseAdvance {
        public int STT { get; set; }
        public Expense exp {  get; set; }
        public int ExpenseID { get; set; }
        public DateOnly Date { get; set; }
        public string Name { get; set; }
        public long Amount { get; set; }
        public string Category { get; set; }
        public DateTime TimeAdded { get; set; }
        public string? Description { get; set; }
        public bool Recurring { get; set; }
        public int CategoryID { get; set; }
        public string FormattedDate => Date.ToString("dd/MM/yyyy");
        public ExpenseAdvance() { }
        public ExpenseAdvance(int i, Expense ex) {
            STT = i;
            exp = ex;
            ExpenseID = ex.ExpenseID;
            Amount = ex.Amount;
            Name = ex.Name;
            Description = ex.Description;   
            Date = ex.Date;
            TimeAdded = ex.TimeAdded;
            CategoryID = ex.CategoryID;
            var cate = DBManager.GetFirst<Category>(c => c.CategoryID == ex.CategoryID && c.UserID == ex.UserID);
            Category = cate.Name;
        }
    }
}