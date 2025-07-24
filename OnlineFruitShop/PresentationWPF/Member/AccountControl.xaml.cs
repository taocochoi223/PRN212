using BusinessObject;
using Repository;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationWPF.Member
{
    /// <summary>
    /// Interaction logic for AccountControl.xaml
    /// </summary>
    public partial class AccountControl : UserControl
    {
        private readonly IUserRepository _userRepo = new UserRepository();
        private User _currentUser;

        public AccountControl()
        {
            InitializeComponent();
        }

        public AccountControl(User currentUser) : this()
        {
            _currentUser = currentUser;
            LoadUserInfo();
        }

        private void LoadUserInfo()
        {
            if (_currentUser != null)
            {
                txtFullName.Text = _currentUser.FullName;
                txtEmail.Text = _currentUser.Email;
                txtPhone.Text = _currentUser.PhoneNumber;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _currentUser.Email = txtEmail.Text.Trim();
            _currentUser.PhoneNumber = txtPhone.Text.Trim();

            _userRepo.UpdateUser(_currentUser);
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}