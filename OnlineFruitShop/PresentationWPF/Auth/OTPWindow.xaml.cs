using BusinessObject;
using DataAccessLayer;
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

namespace PresentationWPF.Auth
{
    /// <summary>
    /// Interaction logic for OTPWindow.xaml
    /// </summary>
    public partial class OTPWindow : Window
    {
        private readonly string _otpCode;
        private readonly User _user;
        public OTPWindow(User user, string otpCode)
        {
            InitializeComponent();
            _otpCode = otpCode;
            _user = user;
        }

        private async void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            BtnVerify.IsEnabled = false;
            string inputOtp = txtOtp.Text.Trim();

            if (string.IsNullOrEmpty(inputOtp))
            {
                MessageBox.Show("Vui lòng nhập mã OTP.", "Thiếu mã", MessageBoxButton.OK, MessageBoxImage.Warning);
                BtnVerify.IsEnabled = true;
                return;
            }

            if (inputOtp == _otpCode)
            {
                bool success = await Task.Run(() =>
                {
                    try
                    {
                        using var context = new OnlineFruitShopContext();
                        // ✅ Lưu user vào DB sau khi xác thực
                        _user.IsActive = true;
                        context.Users.Add(_user);
                        context.SaveChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });

                if (success)
                {
                    MessageBox.Show("Xác thực thành công! Tài khoản của bạn đã được kích hoạt.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    new LoginWindow().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi khi lưu tài khoản. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Mã OTP không chính xác.", "Sai mã", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            BtnVerify.IsEnabled = true;
        }


    }
}
