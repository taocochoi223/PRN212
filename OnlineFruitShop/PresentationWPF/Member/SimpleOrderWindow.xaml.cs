using System;
using System.Windows;

namespace PresentationWPF.Member
{
    public partial class SimpleOrderWindow : Window
    {
        // Thuộc tính public để truyền dữ liệu ra ngoài
        public string ReceiverName { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;

        public SimpleOrderWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra thông tin nhập
            if (string.IsNullOrWhiteSpace(txtReceiverName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người nhận!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtReceiverName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ giao hàng!", "Thông báo",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAddress.Focus();
                return;
            }

            // Gán giá trị để truyền ra
            ReceiverName = txtReceiverName.Text.Trim();
            Phone = txtPhone.Text.Trim();
            Address = txtAddress.Text.Trim();

            this.DialogResult = true; // Trả về kết quả cho cửa sổ gọi
            this.Close();             // Đóng cửa sổ
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
