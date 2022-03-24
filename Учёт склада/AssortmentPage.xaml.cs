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
    /// Логика взаимодействия для AssortmentPage.xaml
    /// </summary>
    //public class ProductForDataGrid : Products
    //{
    //    public ProductForDataGrid()
    //    {
    //        this.
    //    }
    //    public string TypeName { get; set; }
    //}
    public partial class AssortmentPage : UserControl
    {
        public MainWindow MainWindow { get; set; }
        StorageDBEntities db = new StorageDBEntities();
        public AssortmentPage()
        {
            InitializeComponent();
            RefreshDataGrid();
        }
        public void RefreshDataGrid()
        {
            ProductsDataGrid.ItemsSource = null;
            db = new StorageDBEntities();

            //ProductsDataGrid.ItemsSource = db.Products.ToList().Cast<ProductForDataGrid>().Select<ProductForDataGrid, ProductForDataGrid>(item=>
            //{
            //    item.TypeName = item.ProductTypes.Name;
            //    return item;
            //});
            ProductsDataGrid.ItemsSource = db.Products.ToList();
        }
        private void NewProductButton_Click(object sender, RoutedEventArgs e)
        {
            new EditProductWindow(-1).ShowDialog();
            RefreshDataGrid();
            MainWindow.RefreshAllTables();
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (ProductsDataGrid.SelectedItem as Products);
            if (selectedProduct != null)
            {
                new EditProductWindow(selectedProduct.Id).ShowDialog();
                RefreshDataGrid();
                MainWindow.RefreshAllTables();
            }
            else
            {
                MessageBox.Show("Товар не выбран");
            }
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = (ProductsDataGrid.SelectedItem as Products);
            if (selectedProduct != null)
            {
                if (MessageBox.Show("Вы действительно хотите удалить товар \"" + selectedProduct.Name+"?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    db.Products.Remove(selectedProduct);
                    db.SaveChanges();
                    RefreshDataGrid();
                    MainWindow.RefreshAllTables();
                }
            }
            else
            {
                MessageBox.Show("Товар не выбран");
            }
        }

        private void ProductsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
