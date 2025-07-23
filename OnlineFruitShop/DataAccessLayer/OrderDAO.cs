using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderDAO
    {
        public static List<Order> GetOrdersByUser(int userId)
        {
            using var context = new OnlineFruitShopContext();
            return context.Orders
                          .Include(o => o.OrderDetails)
                          .ThenInclude(od => od.Product)
                          .Where(o => o.UserId == userId)
                          .OrderByDescending(o => o.OrderDate)
                          .ToList();
        }
        public static void CancelOrder(int orderId)
        {
            using var context = new OnlineFruitShopContext();
            var order = context.Orders.FirstOrDefault(o => o.OrderId == orderId);

            if (order != null && order.Status != "Canceled")
            {
                order.Status = "Canceled";
                context.SaveChanges();
            }
        }

        public static List<Order> GetAllOrders()
        {
            using var db = new OnlineFruitShopContext();
            return db.Orders
                     .Include(o => o.User)
                     .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                     .ToList();
        }

        public static Order? GetOrderById(int orderId)
        {
            using var db = new OnlineFruitShopContext();
            return db.Orders
                     .Include(o => o.User)
                     .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                     .FirstOrDefault(o => o.OrderId == orderId);
        }

        public static List<Order> GetOrdersByDateRange(DateTime from, DateTime to)
        {
            using var context = new OnlineFruitShopContext();
            return context.Orders
                          .Include(o => o.OrderDetails)
                          .Include(o => o.User)
                          .Where(o => o.OrderDate >= from && o.OrderDate <= to)
                          .ToList();
        }

    }
}