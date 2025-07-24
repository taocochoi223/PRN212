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

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Cart cartItem)
            {
                UpdateCartQuantity(cartItem, cartItem.Quantity + 1);
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Cart cartItem)
            {
                if (cartItem.Quantity > 1)
                {
                    UpdateCartQuantity(cartItem, cartItem.Quantity - 1);
                }
                else
                {
                    // If quantity is 1, ask if user wants to remove item
                    var result = MessageBox.Show(
                        $"Bạn có muốn xóa '{cartItem.ProductName}' khỏi giỏ hàng?",
                        "Xác nhận xóa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        _cartRepo.RemoveCartItem(cartItem.CartId);
                        LoadCart();
                    }
                }
            }
        }

        private void UpdateCartQuantity(Cart cartItem, int newQuantity)
        {
            try
            {
                using var context = new OnlineFruitShopContext();
                var cart = context.Carts.Find(cartItem.CartId);
                if (cart != null)
                {
                    cart.Quantity = newQuantity;
                    context.SaveChanges();
                    LoadCart(); // Refresh the cart
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật số lượng: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            try
            {
                var selectedItems = userCart?.Where(c => c.IsSelected).ToList();
                if (selectedItems == null || selectedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất 1 sản phẩm để đặt hàng.", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kiểm tra tồn kho
                if (!CheckStockAvailability(selectedItems))
                    return;

                // Mở cửa sổ nhập thông tin người nhận
                var orderWindow = new SimpleOrderWindow();
                orderWindow.Owner = Window.GetWindow(this); // Đặt Owner để hiển thị giữa màn hình

                bool? result = orderWindow.ShowDialog();
                if (result == true) // Nếu nhấn "Xác nhận"
                {
                    string receiverName = orderWindow.ReceiverName;
                    string phone = orderWindow.Phone;
                    string address = orderWindow.Address;

                    // Gọi xử lý đặt hàng
                    ProcessOrderSimple(selectedItems, receiverName, phone, address);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt hàng: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool CheckStockAvailability(List<Cart> selectedItems)
        {
            using var context = new OnlineFruitShopContext();
            var unavailableItems = new List<string>();

            foreach (var item in selectedItems)
            {
                var product = context.Products.Find(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                {
                    unavailableItems.Add($"• {item.ProductName}: Chỉ còn {product?.Quantity ?? 0} sản phẩm");
                }
            }

            if (unavailableItems.Any())
            {
                string message = "Một số sản phẩm không đủ số lượng:\n\n" +
                               string.Join("\n", unavailableItems) +
                               "\n\nVui lòng cập nhật giỏ hàng và thử lại.";
                MessageBox.Show(message, "Không đủ hàng", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ProcessOrderSimple(List<Cart> selectedItems, string receiverName, string phone, string address)
        {
            System.Diagnostics.Debug.WriteLine("ProcessOrderSimple started");

            try
            {
                using var context = new OnlineFruitShopContext();
                using var transaction = context.Database.BeginTransaction();

                System.Diagnostics.Debug.WriteLine("Creating new order");

                // Create new order
                var newOrder = new Order
                {
                    UserId = _currentUser.UserId,
                    OrderDate = DateTime.Now,
                    Status = "Confirmed",
                    TotalAmount = selectedItems.Sum(c => c.Total),
                    ReceiverName = receiverName,
                    ReceiverPhone = phone,
                    ShippingAddress = address
                };

                System.Diagnostics.Debug.WriteLine($"Order details: User={newOrder.UserId}, Total={newOrder.TotalAmount}, Receiver={newOrder.ReceiverName}");

                context.Orders.Add(newOrder);
                context.SaveChanges(); // Save to get OrderId

                System.Diagnostics.Debug.WriteLine($"Order saved with ID: {newOrder.OrderId}");

                // Add order details and update product quantities
                foreach (var item in selectedItems)
                {
                    System.Diagnostics.Debug.WriteLine($"Processing item: {item.ProductName}, Qty: {item.Quantity}");

                    // Add order detail
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    context.OrderDetails.Add(detail);

                    // Update product quantity
                    var product = context.Products.Find(item.ProductId);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity;
                        System.Diagnostics.Debug.WriteLine($"Updated product {product.ProductName} quantity: {product.Quantity}");
                    }

                    // Remove from cart
                    var cartItem = context.Carts.Find(item.CartId);
                    if (cartItem != null)
                    {
                        context.Carts.Remove(cartItem);
                        System.Diagnostics.Debug.WriteLine($"Removed cart item: {item.ProductName}");
                    }
                }

                context.SaveChanges();
                transaction.Commit();

                System.Diagnostics.Debug.WriteLine("Order processing completed successfully");

                // Show success message
                string successMessage = $"✅ Đặt hàng thành công!\n\n" +
                                      $"📋 Mã đơn hàng: #{newOrder.OrderId}\n" +
                                      $"💰 Tổng tiền: {newOrder.TotalAmount:N0}₫\n" +
                                      $"📞 Người nhận: {receiverName}\n" +
                                      $"📍 Địa chỉ: {address}\n\n" +
                                      $"Đơn hàng sẽ được giao trong 2-3 ngày làm việc.";

                MessageBox.Show(successMessage, "Đặt hàng thành công",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload cart
                LoadCart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ProcessOrderSimple: {ex}");
                MessageBox.Show($"Lỗi khi đặt hàng:\n\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in userCart)
                {
                    item.IsSelected = true;
                }
                dgCart.Items.Refresh();
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn tất cả: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnselectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in userCart)
                {
                    item.IsSelected = false;
                }
                dgCart.Items.Refresh();
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi bỏ chọn tất cả: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = userCart.Where(x => x.IsSelected).ToList();
                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm để xóa!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa {selectedItems.Count} sản phẩm đã chọn?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    foreach (var item in selectedItems)
                    {
                        _cartRepo.RemoveCartItem(item.CartId);
                    }
                    LoadCart();
                    MessageBox.Show("Đã xóa các sản phẩm đã chọn!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sản phẩm: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (userCart.Count == 0)
                {
                    MessageBox.Show("Giỏ hàng đã trống!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả sản phẩm trong giỏ hàng?",
                    "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _cartRepo.ClearCart(_currentUser.UserId);
                    LoadCart();
                    MessageBox.Show("Đã xóa tất cả sản phẩm!", "Thông báo",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa tất cả: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
