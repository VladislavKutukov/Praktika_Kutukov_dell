using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Учёт_склада
{
    /// <summary>
    /// Логика взаимодействия для EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        StorageDBEntities db = new StorageDBEntities();
        public EditProductWindow(int productId)
        {
            InitializeComponent();
            ProductId = productId;
            db.ProductTypes.ToList().ForEach(item=> {
                TypeNameComboBox.Items.Add(item.Name);
            });

            if (productId > 0)
            {
                Product = db.Products.Find(productId);
                NameTextBox.Text = Product.Name;
                DescriptionTextBox.Text = Product.Description;
                CostTextBox.Text = Product.Cost.ToString();
                TypeNameComboBox.SelectedItem = Product.ProductTypes.Name;
                ProductImageBox.Source = new BitmapImage(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\" + Product.ImagePath));
            }
            else
            {
                Product = new Products();
            }
        }
        private Products Product { get; set; }
        public int ProductId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductId == -1)
            {
                db.Products.Add(Product);
                Product.Name = NameTextBox.Text;
                Product.Description = DescriptionTextBox.Text;
                Product.Cost = double.Parse(CostTextBox.Text);
                Product.ImagePath = Product.ImagePath;
                Product.TypeId = db.ProductTypes.Where(item => item.Name == TypeNameComboBox.SelectedItem.ToString()).FirstOrDefault().Id;
                db.SaveChanges();
                this.Close();
            }
            else
            {
                Product.Name = NameTextBox.Text;
                Product.Description = DescriptionTextBox.Text;
                Product.Cost = double.Parse(CostTextBox.Text);
                Product.ImagePath = Product.ImagePath;
                Product.TypeId = db.ProductTypes.Where(item => item.Name == TypeNameComboBox.SelectedItem.ToString()).FirstOrDefault().Id;
                db.SaveChanges();
                this.Close();
            }
        }

        private void ChangeImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Файлы изображений|*.bmp;*.png;*.jpg";
            if (openDialog.ShowDialog() != true)
                return;
            Product.ImagePath = "images/" + openDialog.FileName.Split('\\').Last();
            ProductImageBox.Source = new BitmapImage(new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\..\..\" + Product.ImagePath));

        }
    }
}
