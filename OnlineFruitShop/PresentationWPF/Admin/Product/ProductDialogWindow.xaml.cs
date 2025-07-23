using Microsoft.Win32;
using Services;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using BO = BusinessObject;

namespace PresentationWPF.Admin.Product
{
    public partial class ProductDialogWindow : Window
    {
        private readonly ICategoryService _categoryService;
        private readonly BO.Product? _productToEdit;

        public BO.Product ProductResult { get; private set; }

        private string? selectedImagePath;

        public ProductDialogWindow(ICategoryService categoryService, BO.Product? product = null)
        {
            InitializeComponent();
            _categoryService = categoryService;
            _productToEdit = product;
            LoadCategoryComboBox();
            if (_productToEdit != null)
            {
                LoadProductData();
            }
        }

        private void LoadCategoryComboBox()
        {
            var categories = _categoryService.GetAllCategories();
            cbCategory.ItemsSource = categories;
            cbCategory.DisplayMemberPath = "CategoryName";
            cbCategory.SelectedValuePath = "CategoryId";
        }

        private void LoadProductData()
        {
            txtName.Text = _productToEdit.ProductName;
            txtDescription.Text = _productToEdit.Description;
            txtPrice.Text = _productToEdit.Price.ToString("F0");
            txtQuantity.Text = _productToEdit.Quantity.ToString();
            cbCategory.SelectedValue = _productToEdit.CategoryId;

            if (!string.IsNullOrEmpty(_productToEdit.ImageUrl) && File.Exists(_productToEdit.ImageUrl))
            {
                imgPreview.Source = new BitmapImage(new Uri(_productToEdit.ImageUrl));
                selectedImagePath = _productToEdit.ImageUrl;
            }
        }

        private void BtnChooseImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Chọn ảnh sản phẩm",
                Filter = "Ảnh (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (dialog.ShowDialog() == true)
            {
                selectedImagePath = dialog.FileName;
                imgPreview.Source = new BitmapImage(new Uri(selectedImagePath));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || cbCategory.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm và chọn danh mục.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Giá không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Số lượng không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var categoryId = (int)cbCategory.SelectedValue;

            ProductResult = _productToEdit ?? new BO.Product
            {
                CreatedAt = DateTime.Now
            };

            ProductResult.ProductName = txtName.Text.Trim();
            ProductResult.Description = txtDescription.Text.Trim();
            ProductResult.Price = price;
            ProductResult.Quantity = quantity;
            ProductResult.CategoryId = categoryId;
            ProductResult.ImageUrl = selectedImagePath;

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
