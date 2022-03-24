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
    /// Логика взаимодействия для SellPage.xaml
    /// </summary>
    public class CartItem
    {
        public Products Product { get; set; }
        public int Count { get; set; }
    }
    public partial class SellPage : UserControl
    {
        public MainWindow MainWindow { get; set; }
        StorageDBEntities db = new StorageDBEntities();
        public List<CartItem> CartMas=new List<CartItem>();
        public SellPage()
        {
            InitializeComponent();
            Refresh();
        }
        public void Refresh()
        {
            db = new StorageDBEntities();
            ProductsListView.Items.Clear();
            foreach (var item in db.Products.Where(product=>product.Name.Contains(SearchTextBox.Text)).ToList())
            {
                ProductsListView.Items.Add(new ProductCard(item, this));
            }
        }
        public void RefreshCartList()
        {
            CartListBox.Items.Clear();
            double summary = 0;
            foreach (var item in CartMas)
            {
                CartListBox.Items.Add(item.Count + " шт. " + item.Product.Name);
                summary += item.Count * (double)item.Product.Cost;
            }
            SummaryCostLabel.Content = summary.ToString() + " руб.";
        }
        public void AddProductToCartList(Products product)
        {
            if (CartMas.Exists(item=>item.Product==product))
            {
                CartMas.Find(item => item.Product == product).Count++;
            }
            else
            {
                CartMas.Add(new CartItem { Product = product, Count = 1 });
            }
            RefreshCartList();
        }
        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in CartMas)
            {
                int Count = 0;
                Count += (int)(item.Product.Arrivals.ToList().Sum(arrival => arrival.Count));
                Count -= (int)(item.Product.Leaves.ToList().Sum(leave => leave.Count));
                if (Count<item.Count)
                {
                    MessageBox.Show("Недостаточно товара на складе: "+item.Product.Name);
                    CartMas.Clear();
                    RefreshCartList();
                    db = new StorageDBEntities();
                    return;
                }
                db.Leaves.Add(new Leaves { DateTime = DateTime.Now, ProductId = item.Product.Id, Count = item.Count });
            }
            db.SaveChanges();
            CartMas.Clear();
            RefreshCartList();
            MainWindow.RefreshAllTables();
        }

        private void ClearCartButton_Click(object sender, RoutedEventArgs e)
        {
            CartMas.Clear();
            RefreshCartList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }
    }
}
