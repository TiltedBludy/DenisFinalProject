using Microsoft.Win32;
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
using System.Reflection;
using WpfApp6.Classes;
using Image = System.Drawing.Image;

namespace WpfApp6.Pages
{
    public partial class AddProductPage : Page
    {
        byte[] ImageProduct;

        public AddProductPage()
        {
            InitializeComponent();
        }

        private bool CheckValidateInput()
        {
            if (string.IsNullOrEmpty(txbAmount.Text) ||
               string.IsNullOrEmpty(txbArticle.Text) ||
               string.IsNullOrEmpty(txbCategory.Text) ||
               string.IsNullOrEmpty(txbCost.Text) ||
               string.IsNullOrEmpty(txbDescription.Text) ||
               string.IsNullOrEmpty(txbInStock.Text) ||
               string.IsNullOrEmpty(txbManufacturer.Text) ||
               string.IsNullOrEmpty(txbName.Text))
            {
                return false;
            }

            return true;
        }

        private void SaveEditProduct()
        {
            if (!CheckValidateInput())
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

            Product product = new Product()
            {
                ProductArticleNumber = txbArticle.Text,
                ProductCategory = txbCategory.Text,
                ProductCost = decimal.Parse(txbCost.Text),
                ProductDescription = txbDescription.Text,
                ProductDiscountAmount = byte.Parse(txbAmount.Text),
                ProductManufacturer = txbManufacturer.Text,
                ProductName = txbName.Text,
                ProductQuantityInStock = int.Parse(txbInStock.Text),
                ProductStatus = productStatus,
                ProductPhoto = ImageProduct
            };

            AppData.aa.Product.Add(product);
            AppData.aa.SaveChanges();
        }

        private void AddImageProduct()
        {
            try
            {
                string imagePath;
                if (MessageBox.Show("Хотите добавить или обновить фотографию продукта?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {

                    Image image = Properties.Resources.picture;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Здесь вы можете указать нужный формат изображения
                        ImageProduct = ms.ToArray();
                    }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            SaveEditProduct();
        }
    }
}
