using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF
{
    public partial class OrderManagementControl : UserControl
    {
        private readonly IOrderService _orderService;
        private List<Order> _orders;
        private Order? _selectedOrder;

        public OrderManagementControl()
        {
            InitializeComponent();
            _orderService = new OrderService();
            _orders = new List<Order>();
            LoadOrders();
        }

        private void LoadOrders()
        {
            _orders = _orderService.GetAllOrders();
            OrderDataGrid.ItemsSource = _orders;
        }

        private void OrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedOrder = OrderDataGrid.SelectedItem as Order;
        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder == null)
            {
                MessageBox.Show("Vui lòng chọn một đơn hàng để xem chi tiết.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dialog = new OrderDetailDialogWindow(_selectedOrder);
            dialog.ShowDialog();
        }
    }
}
