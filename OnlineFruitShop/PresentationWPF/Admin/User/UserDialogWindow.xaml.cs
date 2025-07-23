using BusinessObject;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF
{
    public partial class UserDialogWindow : Window
    {
        public User UserResult { get; private set; }

        public UserDialogWindow()
        {
            InitializeComponent();
            cbRole.SelectedIndex = 1; // Mặc định là Customer
        }

        public UserDialogWindow(User existingUser) : this()
        {
            txtFullName.Text = existingUser.FullName;
            txtEmail.Text = existingUser.Email;
            txtPhone.Text = existingUser.PhoneNumber;
            txtPassword.Password = existingUser.PasswordHash;
            cbRole.SelectedItem = cbRole.Items
                .Cast<ComboBoxItem>()
                .FirstOrDefault(i => i.Content.ToString() == existingUser.Role);
            UserResult = existingUser;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password) ||
                cbRole.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserResult ??= new User();
            UserResult.FullName = txtFullName.Text.Trim();
            UserResult.Email = txtEmail.Text.Trim();
            UserResult.PhoneNumber = txtPhone.Text.Trim();
            UserResult.PasswordHash = txtPassword.Password.Trim();
            UserResult.Role = ((ComboBoxItem)cbRole.SelectedItem).Content.ToString();

            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
