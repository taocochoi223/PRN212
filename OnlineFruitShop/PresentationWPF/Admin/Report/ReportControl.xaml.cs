using BusinessObject;
using Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF
{
    public partial class ReportControl : UserControl
    {
        private readonly IOrderService _orderService;

        public ReportControl()
        {
            InitializeComponent();
            _orderService = new OrderService();
        }

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var fromDate = dpFromDate.SelectedDate ?? DateTime.MinValue;
            var toDate = dpToDate.SelectedDate ?? DateTime.MaxValue;

            var orders = _orderService.GetOrdersByDateRange(fromDate, toDate);

            var displayList = orders.Select(o => new
            {
                o.OrderId,
                OrderDate = o.OrderDate?.ToString("dd/MM/yyyy"),
                Customer = o.User.FullName,
                Items = o.OrderDetails.Count,
                Total = o.TotalAmount?.ToString("N0") + " đ"
            }).ToList();

            dgOrders.ItemsSource = displayList;

            txtOrderCount.Text = $"Số đơn hàng: {orders.Count}";
            txtTotalRevenue.Text = $"Tổng doanh thu: {orders.Sum(o => o.TotalAmount ?? 0):N0} đ";
        }
    }
}
