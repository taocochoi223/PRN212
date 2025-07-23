using BusinessObject;
using System.Collections.Generic;

namespace Repository
{
    public interface ICartRepository
    {
        List<Cart> GetCartsByUser(int userId);
        void AddToCart(Cart cart);
        void RemoveCartItem(int cartId);
        void ClearCart(int userId);
    }

}
