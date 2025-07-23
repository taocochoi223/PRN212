using BusinessObject;
using Services;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using PresentationWPF.Auth;

namespace PresentationWPF
{
    public partial class RegisterWindow : Window
    {
        private readonly UserService iUserService = new UserService();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();
            string password = txtPassword.Password.Trim();
            string confirmPassword = txtConfirmPassword.Password.Trim();

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Email không hợp lệ", "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Số điện thoại không hợp lệ", "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (iUserService.IsEmailExist(email))
            {
                MessageBox.Show("Email đã được sử dụng", "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp", "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // ✅ KHÔNG LƯU DB Ở ĐÂY
            try
            {
                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    PhoneNumber = phone,
                    PasswordHash = password,
                    Role = "Customer",
                    CreatedAt = DateTime.Now,
                    IsActive = false
                };

                string otpCode = GenerateOtp();
                MailHelper.SendOtpEmail(email, otpCode);

                new OTPWindow(user, otpCode).Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi đăng ký", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^(0|\+84)[0-9]{9}$");
        }

        private string GenerateOtp()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }
    }
}
