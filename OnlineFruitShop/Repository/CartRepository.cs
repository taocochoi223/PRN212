using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Repository
{
    public class CartRepository : ICartRepository
    {
        public List<Cart> GetCartsByUser(int userId)
        {
            using var context = new OnlineFruitShopContext();
            return context.Carts
                          .Include(c => c.Product)
                          .Where(c => c.UserId == userId)
                          .ToList();
        }

        public void AddToCart(Cart cart) => CartDAO.AddToCart(cart);
        public void RemoveCartItem(int cartId) => CartDAO.RemoveCartItem(cartId);
        public void ClearCart(int userId) => CartDAO.ClearCart(userId);
    }
}