using BusinessObject;
using DataAccessLayer;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF.Member
{
    public partial class ProductListControl : UserControl
    {
        private readonly IProductRepository _productRepo = new ProductRepository();
        private readonly ICategoryRepository _categoryRepo = new CategoryRepository();
        private List<Product> allProducts;
        private readonly ICartRepository _cartRepo = new CartRepository();

        // 👇 Thêm user hiện tại (giả định bạn đã truyền vào lúc đăng nhập)
        private readonly User _currentUser;

        public ProductListControl(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadProducts();
        }

        private void LoadProducts()
        {
            allProducts = _productRepo.GetAllProducts();
            var categories = _categoryRepo.GetAll();

            foreach (var product in allProducts)
            {
                var cat = categories.FirstOrDefault(c => c.CategoryId == product.CategoryId);
                product.CategoryName = cat?.CategoryName ?? "Không rõ";
            }

            itemsProduct.ItemsSource = allProducts;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            var filtered = string.IsNullOrWhiteSpace(keyword)
                ? allProducts
                : allProducts.Where(p => p.ProductName.ToLower().Contains(keyword)).ToList();

            itemsProduct.ItemsSource = filtered;
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
            if (sender is Button btn && btn.DataContext is Product product)
            {
                var result = MessageBox.Show(
                    $"Bạn có muốn thêm '{product.ProductName}' vào giỏ hàng không?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var cart = new Cart
                    {
                        UserId = _currentUser.UserId,  // lấy từ người dùng đang đăng nhập
                        ProductId = product.ProductId,
                        Quantity = 1
                    };

                    _cartRepo.AddToCart(cart);

                    MessageBox.Show("✅ Đã thêm vào giỏ hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}
