using BusinessObject;
using Services;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace PresentationWPF
{
    public partial class CategoryManagementControl : UserControl
    {
        private readonly ICategoryService _categoryService;
        private List<Category> _categories;

        public CategoryManagementControl()
        {
            InitializeComponent();
            _categoryService = new CategoryService();
            LoadData();
        }

        private void LoadData()
        {
            _categories = _categoryService.GetAllCategories();
            dgCategories.ItemsSource = _categories;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CategoryDialogWindow();
            if (dialog.ShowDialog() == true)
            {
                _categoryService.AddCategory(dialog.CategoryResult);
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var category = (sender as Button)?.DataContext as Category;
            if (category != null)
            {
                var dialog = new CategoryDialogWindow(category);
                if (dialog.ShowDialog() == true)
                {
                    _categoryService.UpdateCategory(dialog.CategoryResult);
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var category = (sender as Button)?.DataContext as Category;
            if (category != null && MessageBox.Show(
                "Bạn có chắc chắn muốn xóa danh mục này?",
                "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _categoryService.DeleteCategory(category.CategoryId);
                LoadData();
            }
        }
    }
}
