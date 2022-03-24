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
    /// Логика взаимодействия для ProductCard.xaml
    /// </summary>
    public partial class ProductCard : UserControl
    {
        StorageDBEntities db = new StorageDBEntities();
        public ProductCard(Products product, SellPage parentPage)
        {
            InitializeComponent();
            DataContext = this;
            Product = product;
            ParentPage = parentPage;
        }
        public Products Product { get; set; }
        SellPage ParentPage { get; set; }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            ParentPage.AddProductToCartList(Product);
        }
    }
}
