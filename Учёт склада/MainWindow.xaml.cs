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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Users User { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            if (new LoginWindow().ShowDialog() == false)
            {
                this.Close();
            }
            SetUserPrivilegies((bool)User.IsAdmin);
            ProfileNameLabel.Content = User.Name;
            StoragePageMain.MainWindow = this;
            SellPageMain.MainWindow = this;
            SalesPageMain.MainWindow = this;
            AssortmantPageMain.MainWindow = this;
            SupplyPageMain.MainWindow = this;
        }
        public void RefreshAllTables()
        {
            StoragePageMain.RefreshDataGrid();
            SellPageMain.Refresh();
            SalesPageMain.RefreshDataGrid();
        }
        private void ChangeAccountButton_Click(object sender, RoutedEventArgs e)
        {
            User = null;
            if (new LoginWindow().ShowDialog() == false)
            {
                this.Close();
            }
            ProfileNameLabel.Content = User.Name;
            SetUserPrivilegies((bool)User.IsAdmin);
        }
        public void SetUserPrivilegies(bool isAdmin)
        {
            if (isAdmin)
            {
                StorageTabItem.IsEnabled = true;
                SellTabItem.IsEnabled = true;
                SalesTabItem.IsEnabled = true;
                AssortmentTabItem.IsEnabled = true;
                SupplyTabItem.IsEnabled = true;
            }
            else
            {
                StorageTabItem.IsEnabled = false;
                SellTabItem.IsEnabled = true;
                SalesTabItem.IsEnabled = false;
                AssortmentTabItem.IsEnabled = false;
                SupplyTabItem.IsEnabled = false;
            }
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((TabControl.SelectedItem as TabItem).Header.ToString() == "Склад")
            //{
            //    StoragePage.RefreshDataGrid();
            //}
        }

        private void SellPageMain_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
