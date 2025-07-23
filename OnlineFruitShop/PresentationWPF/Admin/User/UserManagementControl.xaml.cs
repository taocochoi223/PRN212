using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF
{
    public partial class UserManagementControl : UserControl
    {
        private readonly IUserService _userService;
        private List<User> _users;

        public UserManagementControl()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadData();
        }

        private void LoadData()
        {
            _users = _userService.GetAllUsers();
            dgUsers.ItemsSource = _users.Select(u => new
            {
                u.UserId,
                FullName = u.FullName ?? "(Không có)",
                u.Email,
                u.PhoneNumber,
                u.Role
            }).ToList();
        }

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserDialogWindow();
            if (dialog.ShowDialog() == true)
            {
                _userService.Register(dialog.UserResult);
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext;
            if (item != null)
            {
                int userId = (int)item.GetType().GetProperty("UserId")?.GetValue(item);
                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    var dialog = new UserDialogWindow(user);
                    if (dialog.ShowDialog() == true)
                    {
                        _userService.UpdateUser(dialog.UserResult);
                        LoadData();
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext;
            if (item != null)
            {
                int userId = (int)item.GetType().GetProperty("UserId")?.GetValue(item);
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _userService.DeactivateUser(userId);
                    LoadData();
                }
            }
        }
    }
}
