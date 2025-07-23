using PresentationWPF.Admin.Product;
using System.Windows;
using System.Windows.Controls;
using PresentationWPF;

namespace PresentationWPF.Admin
{
    public partial class AdminDashboardWindow : Window
    {
        public AdminDashboardWindow()
        {
            InitializeComponent();
        }

        private void LoadContent(UserControl control)
        {
            WelcomePanel.Visibility = Visibility.Collapsed;
            MainContentArea.Visibility = Visibility.Visible;
            MainContentArea.Content = control;
        }

        private void ShowWelcome()
        {
            MainContentArea.Content = null;
            MainContentArea.Visibility = Visibility.Collapsed;
            WelcomePanel.Visibility = Visibility.Visible;
        }

        private void BtnManageProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadContent(new ProductManagementControl());
        }

        private void BtnManageCategories_Click(object sender, RoutedEventArgs e)
        {
             LoadContent(new CategoryManagementControl());
        }

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e)
        {
            LoadContent(new UserManagementControl());
        }

        private void BtnViewOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadContent(new OrderManagementControl());
        }

        private void BtnViewReport_Click(object sender, RoutedEventArgs e)
        {
            LoadContent(new ReportControl());
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

    }
}
