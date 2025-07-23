using BusinessObject;
using Repository;
using System.Collections.Generic;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService()
        {
            _orderRepository = new OrderRepository();
        }

        public List<Order> GetAllOrders() => _orderRepository.GetAllOrders();
        public Order? GetOrderById(int orderId) => _orderRepository.GetOrderById(orderId);
        public List<Order> GetOrdersByDateRange(DateTime from, DateTime to)
            => _orderRepository.GetOrdersByDateRange(from, to);
    }
}
