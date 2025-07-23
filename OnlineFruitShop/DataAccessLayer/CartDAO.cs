using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class CartDAO
    {
        public static List<Cart> GetCartsByUser(int userId)
        {
            try
            {
                using var context = new OnlineFruitShopContext();
                return context.Carts
                              .Include(c => c.Product)
                              .Where(c => c.UserId == userId)
                              .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi load giỏ hàng: " + ex.Message); 
                return new List<Cart>();
            }
        }


        public static void AddToCart(Cart cart)
        {
            using var context = new OnlineFruitShopContext();

            var existing = context.Carts.FirstOrDefault(c =>
                c.UserId == cart.UserId && c.ProductId == cart.ProductId);

            if (existing != null)
            {
                existing.Quantity += cart.Quantity;
            }
            else
            {
                var product = context.Products.FirstOrDefault(p => p.ProductId == cart.ProductId);
                if (product == null)
                {
                    throw new Exception("Sản phẩm không tồn tại!");
                }

                cart.Product = product;
                context.Carts.Add(cart);
            }

            context.SaveChanges();
        }

        public static void RemoveCartItem(int cartId)
        {
            using var context = new OnlineFruitShopContext();
            var cart = context.Carts.Find(cartId);
            if (cart != null)
            {
                context.Carts.Remove(cart);
                context.SaveChanges();
            }
        }

        public static void ClearCart(int userId)
        {
            using var context = new OnlineFruitShopContext();
            var items = context.Carts.Where(c => c.UserId == userId).ToList();
            context.Carts.RemoveRange(items);
            context.SaveChanges();
        }
    }
}