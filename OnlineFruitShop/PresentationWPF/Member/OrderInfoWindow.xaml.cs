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
        public string ReceiverName { get; private set; }
        public string Phone { get; private set; }
        public string Address { get; private set; }

        public OrderInfoWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            ReceiverName = txtReceiverName.Text.Trim();
            Phone = txtPhone.Text.Trim();
            Address = txtAddress.Text.Trim();

            if (string.IsNullOrWhiteSpace(ReceiverName) || string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Address))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}