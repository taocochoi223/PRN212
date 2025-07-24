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
using System.Windows.Shapes;

namespace PresentationWPF.Member
{
    /// <summary>
    /// Interaction logic for OrderInfoWindow.xaml
    /// </summary>
    public partial class OrderInfoWindow : Window
    {
        public string ReceiverName { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;
        public string PaymentMethod { get; private set; } = string.Empty;

        public OrderInfoWindow()
        {
            InitializeComponent();
        }

        public OrderInfoWindow(int itemCount, decimal totalAmount) : this()
        {
            Loaded += (s, e) => {
                txtItemCount.Text = $"{itemCount} sản phẩm";
                txtTotalAmount.Text = $"{totalAmount:N0}₫";
            };
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (!ValidateInput())
                return;

            ReceiverName = txtReceiverName.Text.Trim();
            Phone = txtPhone.Text.Trim();
            Address = txtAddress.Text.Trim();
            PaymentMethod = ((ComboBoxItem)cmbPaymentMethod.SelectedItem)?.Content?.ToString() ?? "";

            this.DialogResult = true;
        }

        private bool ValidateInput()
        {
            var errors = new List<string>();

            // Validate receiver name
            if (string.IsNullOrWhiteSpace(txtReceiverName.Text))
                errors.Add("• Tên người nhận không được để trống");
            else if (txtReceiverName.Text.Trim().Length < 2)
                errors.Add("• Tên người nhận phải có ít nhất 2 ký tự");

            // Validate phone
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
                errors.Add("• Số điện thoại không được để trống");
            else if (!IsValidPhoneNumber(txtPhone.Text.Trim()))
                errors.Add("• Số điện thoại không hợp lệ (10-11 số)");

            // Validate address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
                errors.Add("• Địa chỉ giao hàng không được để trống");
            else if (txtAddress.Text.Trim().Length < 10)
                errors.Add("• Địa chỉ giao hàng phải có ít nhất 10 ký tự");

            if (errors.Any())
            {
                string errorMessage = "Vui lòng kiểm tra lại thông tin:\n\n" + string.Join("\n", errors);
                MessageBox.Show(errorMessage, "Thông tin không hợp lệ",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Remove spaces and special characters
            phone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            // Check if it's all digits and has correct length
            return phone.All(char.IsDigit) && (phone.Length == 10 || phone.Length == 11);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}