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
    public partial class CartControl : UserControl
    {
        private readonly ICartRepository _cartRepo = new CartRepository();
        private readonly User _currentUser;
        private List<Cart> userCart;

        public CartControl(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadCart();
        }

        private void LoadCart()
        {
            try
            {
                userCart = _cartRepo.GetCartsByUser(_currentUser.UserId);
                dgCart.ItemsSource = userCart;
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải giỏ hàng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Cart cartItem)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa '{cartItem.ProductName}' khỏi giỏ hàng?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _cartRepo.RemoveCartItem(cartItem.CartId);
                    LoadCart();
                }
            }
        }
        private void dgCart_CurrentCellChanged(object sender, EventArgs e)
        {
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            if (userCart == null) return;

            decimal total = userCart
                .Where(c => c.IsSelected)
                .Sum(c => c.Total);

            txtTotal.Text = $"{total:N0}₫";
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = userCart.Where(c => c.IsSelected).ToList();

            if (selectedItems == null || selectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất 1 sản phẩm để đặt hàng.");
                return;
            }

            var orderWindow = new OrderInfoWindow();
            if (orderWindow.ShowDialog() == true)
            {
                string receiver = orderWindow.ReceiverName;
                string phone = orderWindow.Phone;
                string address = orderWindow.Address;

                // Tạo đơn hàng mới
                var newOrder = new Order
                {
                    UserId = _currentUser.UserId,
                    OrderDate = DateTime.Now,
                    Status = "Confirmed",
                    TotalAmount = selectedItems.Sum(c => c.Total),
                    ReceiverName = receiver,
                    ReceiverPhone = phone,
                    ShippingAddress = address
                };

                using var context = new OnlineFruitShopContext();
                context.Orders.Add(newOrder);
                context.SaveChanges();

                foreach (var item in selectedItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    context.OrderDetails.Add(detail);

                    // Xóa sản phẩm đã đặt khỏi giỏ hàng
                    context.Carts.Remove(context.Carts.Find(item.CartId));
                }

                context.SaveChanges();

                MessageBox.Show("✅ Đặt hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Load lại giỏ hàng
                LoadCart();
            }
        }


        private void SaveOrder(string receiver, string phone, string address)
        {
            try
            {
                using var context = new OnlineFruitShopContext();

                // Tạo đơn hàng mới
                var newOrder = new Order
                {
                    UserId = _currentUser.UserId,
                    OrderDate = DateTime.Now,
                    Status = "Confirmed",
                    TotalAmount = userCart.Sum(c => c.Total),
                    ReceiverName = receiver,
                    ReceiverPhone = phone,
                    ShippingAddress = address
                };

                context.Orders.Add(newOrder);
                context.SaveChanges(); // Lưu để có OrderId

                // Thêm chi tiết đơn hàng
                foreach (var item in userCart)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    context.OrderDetails.Add(detail);
                }

                // Xoá giỏ hàng sau khi đặt
                context.Carts.RemoveRange(context.Carts.Where(c => c.UserId == _currentUser.UserId));
                context.SaveChanges();

                MessageBox.Show("✅ Đặt hàng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCart(); // cập nhật lại UI
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đặt hàng: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
