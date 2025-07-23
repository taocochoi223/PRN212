using BusinessObject;
using PresentationWPF.Admin.Product;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PresentationWPF
{
    public partial class ProductManagementControl : UserControl
    {
        private readonly IProductService iProductService;
        private readonly ICategoryService iCategoryService;

        private List<Product> allProducts;
        private List<Category> allCategories;

        public ProductManagementControl()
        {
            InitializeComponent();
            iProductService = new ProductService();
            iCategoryService = new CategoryService();
            LoadData();
        }

        private void LoadData()
        {
            allProducts = iProductService.GetAllProducts();
            allCategories = iCategoryService.GetAllCategories();

            cbCategoryFilter.ItemsSource = allCategories;
            cbCategoryFilter.DisplayMemberPath = "CategoryName";
            cbCategoryFilter.SelectedValuePath = "CategoryId";
            cbCategoryFilter.SelectedIndex = -1;

            LoadProductTable(allProducts);
        }

        private void LoadProductTable(List<Product> products)
        {
            dgProducts.ItemsSource = products.Select(p => new
            {
                p.ProductId,
                p.ProductName,
                p.Price,
                Stock = p.Quantity,
                CategoryName = allCategories.FirstOrDefault(c => c.CategoryId == p.CategoryId)?.CategoryName ?? "(Không có)",
                p.ImageUrl
            }).ToList();
        }

        private void CbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCategoryFilter.SelectedItem is Category selectedCategory)
            {
                var filteredProducts = allProducts
                    .Where(p => p.CategoryId == selectedCategory.CategoryId)
                    .ToList();
                LoadProductTable(filteredProducts);
            }
        }

        private void BtnResetFilter_Click(object sender, RoutedEventArgs e)
        {
            cbCategoryFilter.SelectedIndex = -1;
            LoadProductTable(allProducts);
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ProductDialogWindow(iCategoryService);
            if (dialog.ShowDialog() == true)
            {
                iProductService.AddProduct(dialog.ProductResult);
                LoadData();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button)?.DataContext;
            if (item != null)
            {
                int productId = (int)item.GetType().GetProperty("ProductId")?.GetValue(item);
                var product = iProductService.GetProductById(productId);
                if (product != null)
                {
                    var dialog = new ProductDialogWindow(iCategoryService, product);
                    if (dialog.ShowDialog() == true)
                    {
                        iProductService.UpdateProduct(dialog.ProductResult);
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
                int productId = (int)item.GetType().GetProperty("ProductId")?.GetValue(item);
                if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    iProductService.DeleteProduct(productId);
                    LoadData();
                }
            }
        }
    }
}
