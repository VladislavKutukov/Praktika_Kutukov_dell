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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        StorageDBEntities db = new StorageDBEntities();
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;
            if (login == "")
            {
                ErrorLabel.Content = "Логин не введён";
                return;
            }
            if (password == "")
            {
                ErrorLabel.Content = "Пароль не введён";
                return;
            }
            if (true == (from user
                     in db.Users 
                     where user.Name == login
                     select user).Any())
            {
                ErrorLabel.Content = "Учетная запись с таким именем уже существует";
                return;
            }
            var newuser = new Users { Name=login, Password=password, IsAdmin=false};
            db.Users.Add(newuser);
            db.SaveChanges();
            this.Close();
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorLabel.Content = "";
        }

        private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorLabel.Content = "";
        }
    }
}
