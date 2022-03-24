using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Учёт_склада
{
    /// <summary>
    /// Логика взаимодействия для SalesPage.xaml
    /// </summary>
    public class LeaveForDataGrid
    {
        public LeaveForDataGrid(Leaves leave)
        {
            Id = leave.Id;
            ProductName = leave.Products.Name;
            DateTime = leave.DateTime.ToString();
            Count = (int)leave.Count;
            Cost = (double)leave.Products.Cost;
            SummaryCost = (double)leave.Products.Cost * (int)leave.Count;
        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string DateTime { get; set; }
        public int Count { get; set; }
        public double Cost { get; set; }
        public double SummaryCost { get; set; }
    }
    public partial class SalesPage : UserControl
    {
        public MainWindow MainWindow { get; set; }
        StorageDBEntities db = new StorageDBEntities();
        public SalesPage()
        {
            InitializeComponent();
            FilterBox.RefreshFunc = RefreshDataGrid;
            RefreshDataGrid();
        }
        public Action RefreshDataGrid()
        {
            LeavesDataGrid.Items.Clear();
            db = new StorageDBEntities();
            var leavesMas = db.Leaves.ToList();

            if (FilterBox.SelectedProductType != "Все")
            {
                leavesMas = leavesMas.Where(leave => leave.Products.ProductTypes.Name == FilterBox.SelectedProductType).ToList();
            }
            switch (FilterBox.SelectedSort)
            {
                case "Назв. А-Я":
                    leavesMas = leavesMas.OrderBy(leave => leave.Products.Name).ToList();
                    break;
                case "Назв. Я-А":
                    leavesMas = leavesMas.OrderByDescending(leave => leave.Products.Name).ToList();
                    break;
                case "Возр. номера":
                    leavesMas = leavesMas.OrderBy(leave => leave.Id).ToList();
                    break;
                case "Убыв. номера":
                    leavesMas = leavesMas.OrderByDescending(leave => leave.Id).ToList();
                    break;
            }
            if (FilterBox.StartDate.Year > 2000 && FilterBox.EndDate.Year > 2000)
            {
                leavesMas = leavesMas.Where(leave => leave.DateTime >= FilterBox.StartDate && leave.DateTime <= FilterBox.EndDate).ToList();
            }

            foreach (var item in leavesMas)
            {
                LeavesDataGrid.Items.Add(new LeaveForDataGrid(item));
            }
            return null;
        }

        private void DeleteSalesButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedLeaves = (LeavesDataGrid.SelectedItem as LeaveForDataGrid);
            if (selectedLeaves != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить заказ \"" + selectedLeaves.Id + "?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.Leaves.Remove(db.Leaves.Where(leave => leave.Id == selectedLeaves.Id).FirstOrDefault());
                    db.SaveChanges();
                    RefreshDataGrid();
                    MainWindow.RefreshAllTables();
                }
            }
            else
            {
                MessageBox.Show("Заказ не выбран");
            }
        }
        public DataTable ConvertDataGridIntoDataTable(DataGrid dataGrid)
        {
            List<LeaveForDataGrid> dataGridMas = dataGrid.Items.Cast<LeaveForDataGrid>().ToList();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Товар", typeof(string));
            dataTable.Columns.Add("Дата", typeof(string));
            dataTable.Columns.Add("Количество", typeof(int));
            dataTable.Columns.Add("Цена за шт.", typeof(decimal));
            dataTable.Columns.Add("Итого, руб.", typeof(decimal));
            foreach (var item in dataGridMas)
            {
                dataTable.Rows.Add(item.ProductName, item.DateTime, item.Count, item.Cost, item.SummaryCost);
            }
            return dataTable;
        }
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            FileInfo fi = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Excel\" + "Продажи.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet Worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Worksheet");
                if (Worksheet == null)
                {
                    Worksheet = excelPackage.Workbook.Worksheets.Add("Worksheet");
                }
                DataTable dataTable = new DataTable();
                dataTable = ConvertDataGridIntoDataTable(LeavesDataGrid);
                Worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                excelPackage.Save();
            }
            MessageBox.Show("Таблица успешно экспортирована");
        }
    }
}
