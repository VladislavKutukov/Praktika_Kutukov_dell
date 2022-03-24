using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Учёт_склада
{
    /// <summary>
    /// Логика взаимодействия для StoragePage.xaml
    /// </summary>
    public class ProductSum
    {
        public ProductSum(Products product)
        {
            ProductName = product.Name;
            Count = 0;
            Count += (int)(product.Arrivals.ToList().Sum(arrival => arrival.Count));
            Count -= (int)(product.Leaves.ToList().Sum(leave => leave.Count));
            Arrivals lastArrival = product.Arrivals.OrderByDescending(arrival => arrival.DateTime).FirstOrDefault();
            Leaves lastLeave = product.Leaves.OrderByDescending(leave => leave.DateTime).FirstOrDefault();
            if(lastArrival !=null)
            {
                LastArrivalDateTime = lastArrival.DateTime.ToString();
            }
            else
            {
                LastArrivalDateTime = "";
            }
            if(lastLeave !=null)
            {
                LastLeaveDateTime = lastLeave.DateTime.ToString();
            }
            else
            {
                LastLeaveDateTime = "";
            }
        }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public string LastArrivalDateTime { get; set; }
        public string LastLeaveDateTime { get; set; }
    }
    public partial class StoragePage : UserControl
    {
        public MainWindow MainWindow { get; set; }
        StorageDBEntities db = new StorageDBEntities();
        public StoragePage()
        {
            InitializeComponent();
            RefreshDataGrid();
        }
        public void RefreshDataGrid()
        {
            db = new StorageDBEntities();
            StorageDataGrid.Items.Clear();
            foreach (var product in db.Products)
            {
                StorageDataGrid.Items.Add(new ProductSum(product));
            }
        }
    }
}
