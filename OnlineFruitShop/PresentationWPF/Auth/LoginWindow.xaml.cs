using BusinessObject;
using PresentationWPF.Admin;
using PresentationWPF.Auth;
using PresentationWPF.Member;
using Services;
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

namespace PresentationWPF
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IUserService iUserService;
        public LoginWindow()
        {
            InitializeComponent();
            iUserService = new UserService();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập email và mật khẩu", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                User user = iUserService.Login(email, password);
                if(user != null)
                {
                    MessageBox.Show($"Chào mừng {user.FullName}! - Vai trò {user.Role}", "Đăng nhập thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    if(user.Role.ToLower() == "admin")
                    {
                        AdminDashboardWindow adminDashboard = new AdminDashboardWindow();
                        adminDashboard.Show();
                    }
                    else
                    {
                        // Open User Dashboard
                        UserDashboardWindow userDashboard = new UserDashboardWindow(user);
                        userDashboard.Show();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sai email hoặc mật khẩu", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Login Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private void ForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ResetPasswordWindow resetwin = new ResetPasswordWindow();
            resetwin.Show();
            this.Close();
        }

    }
}
