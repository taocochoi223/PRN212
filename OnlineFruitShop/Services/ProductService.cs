using BusinessObject;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository iProductRepository;
        public ProductService()
        {
            iProductRepository = new ProductRepository();
        }
        public void AddProduct(Product product) => iProductRepository.AddProduct(product);

        public void DeleteProduct(int id) => iProductRepository.DeleteProduct(id);

        public List<Product> GetAllProducts() => iProductRepository.GetAllProducts();

        public List<Product> GetProductByCategoryId(int categoryId) => iProductRepository.GetProductByCategoryId(categoryId);

        public Product? GetProductById(int id) => iProductRepository.GetProductById(id);

        public void UpdateProduct(Product product) => iProductRepository.UpdateProduct(product);
    }
}
