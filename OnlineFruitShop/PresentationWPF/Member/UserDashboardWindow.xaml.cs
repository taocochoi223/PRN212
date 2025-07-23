using BusinessObject;
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
using PresentationWPF.Auth;

namespace PresentationWPF.Member
{
    /// <summary>
    /// Interaction logic for UserDashboardWindow.xaml
    /// </summary>
     public partial class UserDashboardWindow : Window
     {
        private User _currentUser;

        public UserDashboardWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            MainContent.Content = new HomeControl();
        }
        public User GetCurrentUser()
        {
            return _currentUser;
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomeControl();
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ProductListControl(_currentUser);

        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new CartControl(_currentUser);
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new OrderHistoryControl(_currentUser);
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AccountControl();
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}