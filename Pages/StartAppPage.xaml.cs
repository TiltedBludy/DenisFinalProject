using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp6.Classes;

namespace WpfApp6.Pages
{
    public partial class StartAppPage : Page
    {
        public StartAppPage()
        {
            InitializeComponent();
        }

        private bool CheckValidateInput()
        {
            if(string.IsNullOrEmpty(txbLogin.Text))
            {
                MessageBox.Show("Укажите логин.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Укажите пароль.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void AuthApp()
        {
            if(!CheckValidateInput())
            {
                return;
            }

            var currentUser = AppData.aa.User.FirstOrDefault(x => x.UserLogin == txbLogin.Text &&
                                                                  x.UserPassword == passwordBox.Password);


            if(currentUser != null)
            {
                var roleUser = AppData.aa.Role.FirstOrDefault(x => x.RoleID == 1);
                if(roleUser != null)
                {
                    NavigationService.Navigate(new AdminPage());
                    return;
                }

                NavigationService.Navigate(new ClientAndManagerPage());
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AuthApp();
        }

        private void btnGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GuestPage());
        }
    }
}
