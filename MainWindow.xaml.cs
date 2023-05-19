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
using WpfApp6.Pages;

namespace WpfApp6
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadedStartAppPage();
        }

        private void LoadedStartAppPage()
        {
            MainFrame.NavigationService.Navigate(new StartAppPage());
        }

        private void btnGoBackPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainFrame.NavigationService.GoBack();
            }
            catch
            {
                MessageBox.Show("Не надо нажимать 'Назад', когда на главной странице.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
