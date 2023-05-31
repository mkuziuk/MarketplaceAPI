using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Views;

namespace MarketplaceApi.Repositories
{
    public class OrderRepository : RepositoryBase
    {
        public OrderRepository(MarketplaceContext context) : base(context) {}
        
        
        public Order ExistingOrder(int orderId) => Context.Order.FirstOrDefault(o => o.Id == orderId);
        public IQueryable<Order> ExistingOrders(int orderId) => Context.Order.Where(o => o.Id == orderId);

        public Order OrderPerUser(int userId) => Context.Order.FirstOrDefault(o => o.UserId == userId);
        //public OrderView OrderPerUserView(int userId) => SelectOrderView()
           //.FirstOrDefault(o => o.UserId == userId);
        public IQueryable<Order> OrdersPerUser(int userId) => Context.Order.Where(o => o.UserId == userId);

        public Order IncludeProductInOrder(int orderId, int productId) => Context.Order
            .FirstOrDefault(o => o.Id == orderId && o.Products
                .Any(p => p.Id == productId));

        public void Update(Order order) => Context.Order.Update(order);

        public void Add(Order order) => Context.Order.Add(order);

        public void Remove(Order order) => Context.Order.Remove(order);
    }
}