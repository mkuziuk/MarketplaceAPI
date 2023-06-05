using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.IRepositories;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MarketplaceContext _context;
        public OrderRepository(MarketplaceContext context)
        {
            _context = context;
        }

        public Order ExistingOrder(int orderId) => _context.Order.FirstOrDefault(o => o.Id == orderId);
        public IQueryable<Order> ExistingOrders(int orderId) => _context.Order.Where(o => o.Id == orderId);

        public Order OrderPerUser(int userId) => _context.Order.FirstOrDefault(o => o.UserId == userId);
        //public OrderView OrderPerUserView(int userId) => SelectOrderView()
           //.FirstOrDefault(o => o.UserId == userId);
        public IQueryable<Order> OrdersPerUser(int userId) => _context.Order.Where(o => o.UserId == userId);

        public Order IncludeProductInOrder(int orderId, int productId) => _context.Order
            .FirstOrDefault(o => o.Id == orderId && o.Products
                .Any(p => p.Id == productId));

        public void Update(Order order) => _context.Order.Update(order);

        public void Add(Order order) => _context.Order.Add(order);

        public void Remove(Order order) => _context.Order.Remove(order);

        public void Save() => _context.SaveChanges();
    }
}