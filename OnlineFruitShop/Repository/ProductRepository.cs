using BusinessObject;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(Product product) => ProductDAO.AddProduct(product);

        public void DeleteProduct(int id) => ProductDAO.DeleteProduct(id);

        public List<Product> GetAllProducts()
        {
            var products = ProductDAO.GetAllProducts();
            foreach (var p in products)
            {
                if (!string.IsNullOrEmpty(p.ImageUrl))
                {
                    // Gắn đường dẫn đầy đủ tới ảnh
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string imagePath = System.IO.Path.Combine(baseDir, "Images", p.ImageUrl);

                    // Nếu file tồn tại thì dùng đường dẫn đầy đủ
                    if (System.IO.File.Exists(imagePath))
                    {
                        p.ImageUrl = imagePath;
                    }
                    else
                    {
                        p.ImageUrl = null; // hoặc ảnh mặc định nếu cần
                    }
                }
            }
            return products;
        }






        public List<Product> GetProductByCategoryId(int categoryId) => ProductDAO.GetProductsByCategoryId(categoryId);

        public Product? GetProductById(int id) => ProductDAO.GetProductById(id);

        public void UpdateProduct(Product product) => ProductDAO.UpdateProduct(product);
    }
}
