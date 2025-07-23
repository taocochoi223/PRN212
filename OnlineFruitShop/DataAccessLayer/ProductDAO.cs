using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProductDAO
    {
        public static List<Product> GetAllProducts()
        {
            using var db = new OnlineFruitShopContext();
            return db.Products.ToList();
        }

        public static Product? GetProductById(int productId)
        {
            using var db = new OnlineFruitShopContext();
            return db.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public static void AddProduct(Product product)
        {
            using var db = new OnlineFruitShopContext();
            product.CreatedAt = DateTime.Now;
            db.Products.Add(product);
            db.SaveChanges();
        }

        public static void UpdateProduct(Product product)
        {
            using var db = new OnlineFruitShopContext();
            var existingProduct = db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.ImageUrl = product.ImageUrl;
                existingProduct.CategoryId = product.CategoryId;
                db.SaveChanges();
            }
        }

        public static void DeleteProduct(int productId)
        {
            using var db = new OnlineFruitShopContext();
            var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
        }

        public static List<Product> GetProductsByCategoryId(int categoryId)
        {
            using var db = new OnlineFruitShopContext();
            return db.Products.Where(p => p.CategoryId == categoryId)
                .ToList();
        }
    }
}
