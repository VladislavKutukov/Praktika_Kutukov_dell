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
    /// Логика взаимодействия для Filter.xaml
    /// </summary>
    public partial class Filter : UserControl
    {
        StorageDBEntities db = new StorageDBEntities();
        public Filter()
        {
            InitializeComponent();
            RefreshComboBox();
        }
        public void RefreshComboBox()
        {
            ProductTypeComboBox.Items.Clear();
            ProductTypeComboBox.Items.Add("Все");
            foreach (var item in db.ProductTypes.ToList())
            {
                ProductTypeComboBox.Items.Add(item.Name);
            }
            ProductTypeComboBox.SelectedIndex = 0;
            SortComboBox.SelectedIndex = 2;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Func<Action> RefreshFunc { get; set; }

        public string SelectedProductType = "Все";
        public string SelectedSort = "Возр. номера";

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshFunc();
        }

        private void ProductTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductTypeComboBox.Items.Count>0)
            {
                SelectedProductType = ProductTypeComboBox.SelectedItem.ToString();
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.Items.Count > 0)
            {
                SelectedSort = (SortComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            StartDate = (DateTime)StartDatePicker.SelectedDate;
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EndDate = (DateTime)EndDatePicker.SelectedDate;
        }
    }
}
