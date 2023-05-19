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
using WpfApp6.Database;

namespace WpfApp6.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void listViewDataLoaded(object sender, RoutedEventArgs e)
        {
            listViewData.ItemsSource = AppData.aa.Product.ToList();

            textBlockCountAllProduct.Text = "Всего продукции: " + listViewData.Items.Count;
        }


        private bool CheckItemSelect()
        {
            if (listViewData.SelectedItem == null)
            {
                MessageBox.Show("Сначла выберите продукт из списка.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void DeleteProduct()
        {
            if (!CheckItemSelect())
            {
                return;
            }

            try
            {
                var selectProduct = listViewData.SelectedItem as Product;

                var checkStatus = AppData.aa.OrderProduct.FirstOrDefault(u => u.ProductArticleNumber == selectProduct.ProductArticleNumber);
                if (checkStatus != null)
                {
                    checkStatus = AppData.aa.OrderProduct.FirstOrDefault(u => u.Order.OrderID == checkStatus.OrderID &&
                                                                              u.Order.OrderStatus == "Новый");
                    if(checkStatus != null)
                    {
                        MessageBox.Show("Вы не можете удалить этот товар.\r\nТовар, который присутствует в заказе, удалить нельзя.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                AppData.aa.Product.Remove(selectProduct);
                AppData.aa.SaveChanges();
                listViewDataLoaded(null, null);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите удалить товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DeleteProduct();
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProductPage());
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы точно хотите изменить товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                if (!CheckItemSelect())
                {
                    return;
                }
                var selectProduct = listViewData.SelectedItem as Product;

                NavigationService.Navigate(new EditProductPage(selectProduct));
            }
        }


        private void SortProduct()
        {
            List<Product> products = AppData.aa.Product.ToList();

            string[] namesProduct = txbSearchProduct.Text.ToLower().Split(' ');

            int countProduct = products.Count;

            if (!string.IsNullOrEmpty(txbSearchProduct.Text))
            {
                for (int i = 0; i < namesProduct.Length; i++)
                {
                    products = products.Where(x => x.ProductName.ToLower().Contains(namesProduct[i]) ||
                                                   x.ProductCategory.ToLower().Contains(namesProduct[i]) ||
                                                   x.ProductDescription.ToLower().Contains(namesProduct[i]) ||
                                                   x.ProductManufacturer.ToLower().Contains(namesProduct[i]) ||
                                                   x.ProductStatus.ToLower().Contains(namesProduct[i]) ||
                                                   x.ProductArticleNumber.Contains(namesProduct[i])).ToList();

                }
            }

            if (rbSortUp.IsChecked == true)
            {
                products = products.OrderBy(x => x.ProductCost).ToList();
            }

            if (rbSortDown.IsChecked == true)
            {
                products =  products.OrderByDescending(x => x.ProductCost).ToList();
            }

            
            if (countProduct != products.Count)
            {
                textBlockCountSortProduct.Text = "Отфильтрованной продукции: " + products.Count.ToString();
            }

            listViewData.ItemsSource = products;


            if (products.Count == 0)
            {
                MessageBox.Show("Товар по запросу не найден.\r\n Настройки фильтра сброшены.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                
                rbSortDown.IsEnabled = false;
                rbSortUp.IsEnabled = false;
                txbSearchProduct.Text = "";
            }

        }

        private void rbSortDown_Checked(object sender, RoutedEventArgs e)
        {
            SortProduct();
        }

        private void rbSortUp_Checked(object sender, RoutedEventArgs e)
        {
            SortProduct();
        }

        private void txbSearchProduct_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            SortProduct();
        }
    }
}
