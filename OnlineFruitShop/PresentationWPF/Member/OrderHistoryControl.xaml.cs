using BusinessObject;
using Repository;
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

namespace PresentationWPF.Member
{
    /// <summary>
    /// Interaction logic for OrderHistoryControl.xaml
    /// </summary>
    public partial class OrderHistoryControl : UserControl
    {
        private readonly IOrderRepository _orderRepo = new OrderRepository();
        private User? _currentUser;

        public OrderHistoryControl()
        {
            InitializeComponent();
        }

        public OrderHistoryControl(User user) : this()
        {
            _currentUser = user;
            Loaded += OrderHistoryControl_Loaded;
        }

        private void OrderHistoryControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }
        private void ViewOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order order)
            {
                var orderDetailWindow = new OrderDetailWindow(order);
                orderDetailWindow.ShowDialog();

                // Refresh orders in case status changed
                LoadOrders();
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order order)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn hủy đơn hàng #{order.OrderId}?\n\n" +
                    "Lưu ý: Sau khi hủy, số lượng sản phẩm sẽ được hoàn lại kho.",
                    "Xác nhận hủy đơn hàng",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _orderRepo.CancelOrder(order.OrderId);
                        MessageBox.Show("✅ Đã hủy đơn hàng thành công!", "Thông báo",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi hủy đơn hàng: {ex.Message}", "Lỗi",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LoadOrders()
        {
            try
            {
                if (_currentUser == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    dgOrders.ItemsSource = new List<Order>();
                    return;
                }

                // Test database connection first
                TestDatabaseConnection();

                // Debug: Show user info
                System.Diagnostics.Debug.WriteLine($"Loading orders for user ID: {_currentUser.UserId}");

                List<Order> orders = _orderRepo.GetOrdersByUser(_currentUser.UserId);

                // Debug: Show order count
                System.Diagnostics.Debug.WriteLine($"Found {orders.Count} orders");

                dgOrders.ItemsSource = orders;

                // Show message if no orders found (but don't show popup every time)
                if (orders.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No orders found for this user");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đơn hàng:\n\n{ex.Message}\n\nChi tiết: {ex.InnerException?.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                dgOrders.ItemsSource = new List<Order>();
            }
        }

        private void TestDatabaseConnection()
        {
            try
            {
                using var context = new DataAccessLayer.OnlineFruitShopContext();
                var canConnect = context.Database.CanConnect();
                System.Diagnostics.Debug.WriteLine($"Database connection test: {canConnect}");

                if (!canConnect)
                {
                    throw new Exception("Không thể kết nối đến database");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection error: {ex.Message}");
                throw;
            }
        }
    }
}