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
using System.Windows.Shapes;

namespace Учёт_склада
{
    /// <summary>
    /// Логика взаимодействия для EditArrivalsWindow.xaml
    /// </summary>
    public partial class EditArrivalsWindow : Window
    {
        StorageDBEntities db = new StorageDBEntities();
        public EditArrivalsWindow(int arrivalId)
        {
            InitializeComponent();
            ArrivalId = arrivalId;
            db.Products.ToList().ForEach(item => {
                ProductNameComboBox.Items.Add(item.Name);
            });

            if (arrivalId > 0)
            {
                Arrival = db.Arrivals.Find(arrivalId);
                CountTextBox.Text = Arrival.Count.ToString();
                ProductNameComboBox.SelectedItem = Arrival.Products.Name;
            }
            else
            {
                Arrival = new Arrivals();
            }
        }
        private Arrivals Arrival { get; set; }
        public int ArrivalId { get; set; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ArrivalId == -1)
            {
                db.Arrivals.Add(Arrival);
                Arrival.Count = int.Parse(CountTextBox.Text);
                Arrival.ProductId = db.Products.Where(item => item.Name == ProductNameComboBox.SelectedItem.ToString()).FirstOrDefault().Id;
                Arrival.DateTime = DateTime.Now;
                db.SaveChanges();
                this.Close();
            }
            else
            {
                Arrival.Count = int.Parse(CountTextBox.Text);
                Arrival.ProductId = db.Products.Where(item => item.Name == ProductNameComboBox.SelectedItem.ToString()).FirstOrDefault().Id;
                db.SaveChanges();
                this.Close();
            }
        }
    }
}
