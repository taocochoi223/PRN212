using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;


namespace Repository
{
    public class OrderRepository : IOrderRepository
    {
        public List<Order> GetOrdersByUser(int userId)
        {
            return OrderDAO.GetOrdersByUser(userId);
        }
        public void CancelOrder(int orderId)
        {
            OrderDAO.CancelOrder(orderId);
        }

        public List<Order> GetAllOrders() => OrderDAO.GetAllOrders();
        public Order? GetOrderById(int orderId) => OrderDAO.GetOrderById(orderId);
        public List<Order> GetOrdersByDateRange(DateTime from, DateTime to)
            => OrderDAO.GetOrdersByDateRange(from, to);
    }
}
