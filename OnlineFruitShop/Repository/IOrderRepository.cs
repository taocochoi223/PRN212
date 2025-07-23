using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IOrderRepository
    {
        List<Order> GetOrdersByUser(int userId);
        void CancelOrder(int orderId);
        List<Order> GetAllOrders();
        Order? GetOrderById(int orderId);
        List<Order> GetOrdersByDateRange(DateTime from, DateTime to);

    }
}