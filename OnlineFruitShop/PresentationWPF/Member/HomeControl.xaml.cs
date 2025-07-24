using BusinessObject;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF.Member
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : UserControl
    {
        private readonly IProductRepository _productRepo = new ProductRepository();
        private readonly ICategoryRepository _categoryRepo = new CategoryRepository();
        private readonly ICartRepository _cartRepo = new CartRepository();
        private List<Product> allProducts;
        private List<Category> allCategories;
        private readonly User _currentUser;

        public HomeControl()
        {
            InitializeComponent();
            LoadData();
        }

        public HomeControl(User currentUser) : this()
        {
            _currentUser = currentUser;
        }

        private void LoadData()
        {
            try
            {
                // Load products and categories
                allProducts = _productRepo.GetAllProducts();
                allCategories = _categoryRepo.GetAll();

                // Set category names for products
                foreach (var product in allProducts)
                {
                    var cat = allCategories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
                    product.CategoryName = cat?.CategoryName ?? "Không rõ";
                }

                LoadCategories();
                //LoadFeaturedProducts();
                LoadAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCategories()
        {
            CategoriesPanel.Children.Clear();

            foreach (var category in allCategories)
            {
                var button = new Button
                {
                    Content = category.CategoryName,
                    Style = (Style)FindResource("CategoryButtonStyle"),
                    Tag = category.CategoryId
                };
                button.Click += CategoryButton_Click;
                CategoriesPanel.Children.Add(button);
            }
        }

        //private void LoadFeaturedProducts()
        //{
        //    // Get top 6 products (you can modify this logic)
        //    var featuredProducts = allProducts.Take(6).ToList();
        //    FeaturedProductsPanel.ItemsSource = featuredProducts;
        //}

        private void LoadAllProducts()
        {
            AllProductsPanel.ItemsSource = allProducts;
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int categoryId)
            {
                var filteredProducts = allProducts.Where(p => p.CategoryId == categoryId).ToList();
                AllProductsPanel.ItemsSource = filteredProducts;
            }
        }

        private void ShopNow_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to products section by scrolling
            var scrollViewer = FindVisualParent<ScrollViewer>(this);
            if (scrollViewer != null)
            {
                scrollViewer.ScrollToVerticalOffset(800); // Scroll to products section
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            var filtered = string.IsNullOrWhiteSpace(keyword)
                ? allProducts
                : allProducts.Where(p => p.ProductName.ToLower().Contains(keyword)).ToList();

            AllProductsPanel.ItemsSource = filtered;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtSearch.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Detail_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Product product)
            {
                MessageBox.Show($"Chi tiết sản phẩm:\n\nTên: {product.ProductName}\nGiá: {product.Price:N0}₫\nMô tả: {product.Description}",
                    "Chi tiết sản phẩm", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null)
            {
                MessageBox.Show("Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (sender is Button btn && btn.DataContext is Product product)
            {
                var result = MessageBox.Show(
                    $"Bạn có muốn thêm '{product.ProductName}' vào giỏ hàng không?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var cart = new Cart
                        {
                            UserId = _currentUser.UserId,
                            ProductId = product.ProductId,
                            Quantity = 1
                        };

                        _cartRepo.AddToCart(cart);
                        MessageBox.Show("✅ Đã thêm vào giỏ hàng!", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm vào giỏ hàng: " + ex.Message, "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }



        // Helper method to find parent control
        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = System.Windows.Media.VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindVisualParent<T>(parentObject);
        }
    }
}
