using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp6.Database;
using Microsoft.Win32;
using System.Xaml;
using System.Text.RegularExpressions;
using WpfApp6.Classes;

namespace WpfApp6.Pages
{
    public partial class EditProductPage : Page
    {
        public Product SelectProduct { get; }

        byte[] ImageProduct;
        
        public EditProductPage(Product selectProduct)
        {
            InitializeComponent();

            SelectProduct = selectProduct;

        }

        private void LoadedProduct(object sender, RoutedEventArgs e)
        {
            txbAmount.Text = SelectProduct.ProductDiscountAmount.ToString();
            txbArticle.Text = SelectProduct.ProductArticleNumber.ToString();
            txbCost.Text = SelectProduct.ProductCost.ToString();
            txbDescription.Text = SelectProduct.ProductDescription.ToString();
            txbInStock.Text = SelectProduct.ProductQuantityInStock.ToString();
            txbManufacturer.Text = SelectProduct.ProductManufacturer.ToString();
            txbName.Text = SelectProduct.ProductName.ToString();

            var distinctCategories = AppData.aa.Product.Select(p => p.ProductCategory).Distinct().ToList();

            cmbCategory.ItemsSource = distinctCategories;

        }

        private bool CheckValidateInput()
        {
            if(string.IsNullOrEmpty(txbAmount.Text) ||
               string.IsNullOrEmpty(txbArticle.Text) ||
               string.IsNullOrEmpty(txbCost.Text) ||
               string.IsNullOrEmpty(txbDescription.Text) ||
               string.IsNullOrEmpty(txbInStock.Text) ||
               string.IsNullOrEmpty(txbManufacturer.Text) ||
               string.IsNullOrEmpty(txbName.Text))
            {
                MessageBox.Show("Укажите данные во всех полях.", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                return false;
            }
            if(cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Укажите категорию.", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                return false;
            }
            return true;
        }

        private void SaveEditProduct()
        {
            if(!CheckValidateInput())
            {
                return;
            }

            AddImageProduct();

            string productStatus;
            if (int.Parse(txbInStock.Text) >= 1)
            {
                productStatus = "В наличии";
            }
            else
            {
                productStatus = "Нет в наличии";
            }

            var selectedItem = SelectProduct;

            Product product = new Product()
            {
                ProductArticleNumber = txbArticle.Text,
                ProductCategory = cmbCategory.SelectedItem.ToString(),
                ProductCost = decimal.Parse(txbCost.Text),
                ProductDescription = txbDescription.Text,
                ProductDiscountAmount = byte.Parse(txbAmount.Text),
                ProductManufacturer = txbManufacturer.Text,
                ProductName = txbName.Text,
                ProductQuantityInStock = int.Parse(txbInStock.Text),
                ProductStatus = productStatus,
                ProductPhoto = ImageProduct
                
            };
        }

        private void AddImageProduct()
        {
            string imagePath;
            if (MessageBox.Show("Хотите добавить или обновить фотографию продукта?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                imagePath = "/Resources/picture.png";
                byte[] imageData = File.ReadAllBytes(imagePath);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg, *.jpeg, *.gif |*.png; *.jpg; *.jpeg; *.gif;)";
            if (openFileDialog.ShowDialog() == true)
            {
                imagePath = openFileDialog.FileName;
            }
            imagePath = openFileDialog.FileName;
            ImageProduct = File.ReadAllBytes(imagePath);
        }


        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            SaveEditProduct();
        }

        private void txbInStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9,]+");

            if(regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
