using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class OrderRepository : Repository
    {
        public OrderRepository(MarketplaceContext context) : base(context) {}

        public Order ExistingOrder(int orderId) => Context.Order.FirstOrDefault(o => o.Id == orderId);
        
        public IQueryable<Order> ExistingOrders(int userId) => Context.Order.Where(o => o.UserId == userId);

        public void Update(Order order) => Context.Order.Update(order);

        public void Add(Order order) => Context.Order.Add(order);

        public void Remove(Order order) => Context.Order.Remove(order);
    }
}