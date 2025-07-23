using BusinessObject;
using Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace PresentationWPF.Auth
{
    public partial class ResetPasswordWindow : Window
    {
        private readonly IUserService _userService;
        private readonly IPasswordResetService _resetService;

        private string _generatedOtp = "";
        private DateTime _otpGeneratedTime;
        private User? _currentUser;

        public ResetPasswordWindow()
        {
            InitializeComponent();
            _userService = new UserService();
            _resetService = new PasswordResetService();
        }

        private void BtnSendOtp_Click(object sender, RoutedEventArgs e)
        {
            var email = txtEmail.Text.Trim();
            _currentUser = _userService.GetUserByEmail(email);

            if (_currentUser == null)
            {
                MessageBox.Show("Email không tồn tại!");
                return;
            }

            var rnd = new Random();
            _generatedOtp = rnd.Next(100000, 999999).ToString();
            _otpGeneratedTime = DateTime.Now;

            _resetService.RequestReset(_currentUser.UserId);

            try
            {
                MailHelper.SendOtpEmail(email, _generatedOtp);
                MessageBox.Show("Đã gửi mã OTP đến email của bạn.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gửi email thất bại: " + ex.Message);
            }
        }

        private void BtnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var otp = txtOtp.Text.Trim();
            var newPassword = txtNewPassword.Password.Trim();

            if (_currentUser == null)
            {
                MessageBox.Show("Vui lòng nhập email và gửi mã OTP trước.");
                return;
            }

            if (string.IsNullOrEmpty(otp) || string.IsNullOrEmpty(newPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ OTP và mật khẩu mới.");
                return;
            }

            var request = _resetService.GetActiveRequest(_currentUser.UserId);
            if (request == null || request.IsUsed == true)
            {
                MessageBox.Show("Yêu cầu đã hết hạn hoặc không hợp lệ!");
                return;
            }

            if ((DateTime.Now - _otpGeneratedTime).TotalSeconds > 30)
            {
                MessageBox.Show("Mã OTP đã hết hạn (quá 30 giây). Vui lòng gửi lại.");
                return;
            }

            if (otp != _generatedOtp)
            {
                MessageBox.Show("Mã OTP không đúng!");
                return;
            }

            _userService.UpdatePassword(_currentUser.UserId, newPassword);
            _resetService.CompleteReset(request.ResetId);

            MessageBox.Show("Đặt lại mật khẩu thành công!");
            this.Close();
        }
    }
}
