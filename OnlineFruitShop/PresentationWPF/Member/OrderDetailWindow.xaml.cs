using BusinessObject;
using Repository;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PresentationWPF.Member
{
    public partial class OrderDetailWindow : Window
    {
        private readonly IOrderRepository _orderRepo = new OrderRepository();
        private Order _order;

        public OrderDetailWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            if (_order == null) return;

            // Basic order info
            txtOrderId.Text = $"#{_order.OrderId}";
            txtOrderDate.Text = _order.OrderDate?.ToString("dd/MM/yyyy HH:mm") ?? "";
            txtReceiverName.Text = _order.ReceiverName;
            txtPhone.Text = _order.ReceiverPhone;
            txtAddress.Text = _order.ShippingAddress;
            txtTotalAmount.Text = $"{_order.TotalAmount:N0}₫";

            // Status with color
            txtStatus.Text = GetStatusText(_order.Status);
            statusBorder.Background = GetStatusColor(_order.Status);

            // Order details
            if (_order.OrderDetails != null)
            {
                var orderDetailsWithTotal = _order.OrderDetails.Select(od => new
                {
                    od.Product,
                    od.Quantity,
                    od.UnitPrice,
                    Total = od.Quantity * od.UnitPrice
                }).ToList();

                dgOrderDetails.ItemsSource = orderDetailsWithTotal;
            }

            // Show/hide cancel button based on status
            btnCancel.Visibility = CanCancelOrder(_order.Status) ? Visibility.Visible : Visibility.Collapsed;
        }

        private string GetStatusText(string? status)
        {
            return status switch
            {
                "Confirmed" => "Đã xác nhận",
                "Delivered" => "Đã giao hàng",
                "Canceled" => "Đã hủy",
                _ => status ?? "Không rõ"
            };
        }

        private Brush GetStatusColor(string? status)
        {
            return status switch
            {
                "Confirmed" => new SolidColorBrush(Color.FromRgb(52, 152, 219)), // Blue
                "Delivered" => new SolidColorBrush(Color.FromRgb(39, 174, 96)),  // Green
                "Canceled" => new SolidColorBrush(Color.FromRgb(231, 76, 60)),   // Red
                _ => new SolidColorBrush(Color.FromRgb(149, 165, 166))           // Gray
            };
        }

        private bool CanCancelOrder(string? status)
        {
            return status == "Confirmed";
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Bạn có chắc chắn muốn hủy đơn hàng #{_order.OrderId}?\n\n" +
                "Lưu ý: Sau khi hủy, số lượng sản phẩm sẽ được hoàn lại kho.",
                "Xác nhận hủy đơn hàng",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _orderRepo.CancelOrder(_order.OrderId);

                    MessageBox.Show(
                        "✅ Đã hủy đơn hàng thành công!\n\nSố lượng sản phẩm đã được hoàn lại kho.",
                        "Hủy đơn hàng thành công",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    // Refresh order details
                    var updatedOrder = _orderRepo.GetOrderById(_order.OrderId);
                    if (updatedOrder != null)
                    {
                        _order = updatedOrder;
                        LoadOrderDetails();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Lỗi khi hủy đơn hàng: {ex.Message}",
                        "Lỗi",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
