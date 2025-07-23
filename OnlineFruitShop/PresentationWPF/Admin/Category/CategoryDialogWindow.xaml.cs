using BusinessObject;
using System.Windows;

namespace PresentationWPF
{
    public partial class CategoryDialogWindow : Window
    {
        public Category CategoryResult { get; private set; }

        private readonly bool _isEdit;

        public CategoryDialogWindow()
        {
            InitializeComponent();
            CategoryResult = new Category();
            _isEdit = false;
        }

        public CategoryDialogWindow(Category category)
        {
            InitializeComponent();
            CategoryResult = new Category
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
            _isEdit = true;

            txtCategoryName.Text = CategoryResult.CategoryName;
            txtDescription.Text = CategoryResult.Description;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = txtCategoryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên danh mục không được để trống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CategoryResult.CategoryName = name;
            CategoryResult.Description = txtDescription.Text?.Trim();

            this.DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
