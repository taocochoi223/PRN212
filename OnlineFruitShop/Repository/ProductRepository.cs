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
            string baseUrl = "https://raw.githubusercontent.com/taocochoi223/PRN212/main/images/";

            foreach (var p in products)
            {
                // Mapping tên sai -> đúng nếu cần
                if (p.ImageUrl == "tao_my.jpg") p.ImageUrl = "taomy.jpg";
                if (p.ImageUrl == "cam_sanh.jpg") p.ImageUrl = "camsanh.jpg";
                // ...

                p.ImageUrl = baseUrl + p.ImageUrl;
            }

            return products;
        }



        public List<Product> GetProductByCategoryId(int categoryId) => ProductDAO.GetProductsByCategoryId(categoryId);

        public Product? GetProductById(int id) => ProductDAO.GetProductById(id);

        public void UpdateProduct(Product product) => ProductDAO.UpdateProduct(product);
    }
}
