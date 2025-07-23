using BusinessObject;
using System.Windows;

namespace PresentationWPF
{
    public partial class OrderDetailDialogWindow : Window
    {
        private readonly Order _order;

        public OrderDetailDialogWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            LoadDetails();
        }

        private void LoadDetails()
        {
            TxtCustomer.Text = $"Khách hàng: {_order.User.FullName}";
            TxtDate.Text = $"Ngày đặt: {_order.OrderDate?.ToString("dd/MM/yyyy")}";
            TxtStatus.Text = $"Trạng thái: {_order.Status}";
            TxtTotal.Text = $"Tổng tiền: {_order.TotalAmount:N0} VNĐ";
            OrderDetailGrid.ItemsSource = _order.OrderDetails;
        }
    }
}
