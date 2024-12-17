
using OfficeOpenXml;
using System.Data;
using System.IO;
using System.Reflection;
using OfficeOpenXml.Table;
using PersonalFinanceApp.Database;

namespace PersonalFinanceApp.etc;

public static class ExportExcelServices
{
    public static DataTable ToDataTable<T>(IEnumerable<T> data) where T : class
    {
        DataTable dt = new DataTable(typeof(T).Name);
        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? throw new InvalidOperationException());
        }

        foreach (var item in data)
        {
            var row = dt.NewRow();
            foreach (PropertyInfo property in properties)
            {
                row[property.Name] = property.GetValue(item, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    public static void ExportDataTableToExcel(List<DataTable> dataTables, string filePath)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            foreach (DataTable dataTable in dataTables)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(dataTable.TableName ?? "Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true, TableStyles.Medium9);
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
            }
            FileInfo file = new FileInfo(filePath);
            package.SaveAs(file);
        }
    }

    public static void ExportExcels()
    {
        
    }
}