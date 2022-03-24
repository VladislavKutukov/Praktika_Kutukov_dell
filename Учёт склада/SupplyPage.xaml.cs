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
    /// Логика взаимодействия для SupplyPage.xaml
    /// </summary>
    public class ArrivalForDataGrid
    {
        public ArrivalForDataGrid(Arrivals arrival)
        {
            Id = arrival.Id;
            ProductName = arrival.Products.Name;
            DateTime = arrival.DateTime.ToString();
            Count = (int)arrival.Count;
            Cost = (double)arrival.Products.Cost;
            SummaryCost = (double)arrival.Products.Cost * (int)arrival.Count;
        }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string DateTime { get; set; }
        public int Count { get; set; }
        public double Cost { get; set; }
        public double SummaryCost { get; set; }
    }
    public partial class SupplyPage : UserControl
    {
        public MainWindow MainWindow { get; set; }
        StorageDBEntities db = new StorageDBEntities();
        public SupplyPage()
        {
            InitializeComponent();
            FilterBox.RefreshFunc = RefreshDataGrid;
            RefreshDataGrid();
        }
        public Action RefreshDataGrid()
        {
            ArrivalsDataGrid.Items.Clear();
            db = new StorageDBEntities();
            var arrivalsMas = db.Arrivals.ToList();

            if (FilterBox.SelectedProductType!="Все")
            {
                arrivalsMas = arrivalsMas.Where(arrival => arrival.Products.ProductTypes.Name == FilterBox.SelectedProductType).ToList();
            }
            switch (FilterBox.SelectedSort)
            {
                case "Назв. А-Я":
                    arrivalsMas = arrivalsMas.OrderBy(arrival => arrival.Products.Name).ToList();
                    break;
                case "Назв. Я-А":
                    arrivalsMas = arrivalsMas.OrderByDescending(arrival => arrival.Products.Name).ToList();
                    break;
                case "Возр. номера":
                    arrivalsMas = arrivalsMas.OrderBy(arrival => arrival.Id).ToList();
                    break;
                case "Убыв. номера":
                    arrivalsMas = arrivalsMas.OrderByDescending(arrival => arrival.Id).ToList();
                    break;
            }
            if (FilterBox.StartDate.Year>2000 && FilterBox.EndDate.Year > 2000)
            {
                arrivalsMas = arrivalsMas.Where(arrival => arrival.DateTime >= FilterBox.StartDate && arrival.DateTime <= FilterBox.EndDate).ToList();
            }

            foreach (var item in arrivalsMas)
            {
                ArrivalsDataGrid.Items.Add(new ArrivalForDataGrid(item));
            }
            return null;
        }
        private void NewSupplyButton_Click(object sender, RoutedEventArgs e)
        {
            new EditArrivalsWindow(-1).ShowDialog();
            RefreshDataGrid();
            MainWindow.RefreshAllTables();
        }

        private void EditSupplyButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedArrivals = (ArrivalsDataGrid.SelectedItem as ArrivalForDataGrid);
            if (selectedArrivals != null)
            {
                new EditArrivalsWindow(selectedArrivals.Id).ShowDialog();
                RefreshDataGrid();
                MainWindow.RefreshAllTables();
            }
            else
            {
                MessageBox.Show("Заказ не выбран");
            }
        }

        private void DeleteSupplyButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedArrivals = (ArrivalsDataGrid.SelectedItem as ArrivalForDataGrid);
            if (selectedArrivals != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить заказ \"" + selectedArrivals.Id + "?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.Arrivals.Remove(db.Arrivals.Where(arrival=>arrival.Id==selectedArrivals.Id).FirstOrDefault());
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
            List<ArrivalForDataGrid> dataGridMas = dataGrid.Items.Cast<ArrivalForDataGrid>().ToList();
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
            FileInfo fi = new FileInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\Excel\" + "Закупки.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet Worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "Worksheet");
                if (Worksheet == null)
                {
                    Worksheet = excelPackage.Workbook.Worksheets.Add("Worksheet");
                }
                DataTable dataTable = new DataTable();
                dataTable = ConvertDataGridIntoDataTable(ArrivalsDataGrid);
                Worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                excelPackage.Save();
            }
            MessageBox.Show("Таблица успешно экспортирована");
        }

        private void ArrivalsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
