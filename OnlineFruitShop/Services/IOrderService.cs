using BusinessObject;
using System.Collections.Generic;

namespace Services
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order? GetOrderById(int orderId);
        List<Order> GetOrdersByDateRange(DateTime from, DateTime to);
    }
}
