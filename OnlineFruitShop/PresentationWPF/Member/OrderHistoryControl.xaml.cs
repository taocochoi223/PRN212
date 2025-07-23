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
        private readonly User _currentUser;

        public OrderHistoryControl(User user)
        {
            InitializeComponent();
            _currentUser = user;
            LoadOrders();
        }
        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Order order)
            {
                var result = MessageBox.Show("Bạn có chắc muốn hủy đơn hàng này?", "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _orderRepo.CancelOrder(order.OrderId);
                    LoadOrders(); 
                }
            }
        }

        private void LoadOrders()
        {
            List<Order> orders = _orderRepo.GetOrdersByUser(_currentUser.UserId);
            dgOrders.ItemsSource = orders;
        }
    }
}