using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Queries
{
    public class OrderRepository
    {
        private readonly MarketplaceContext _context;

        public OrderRepository(MarketplaceContext context)
        {
            _context = context;
        }

        public Order ExistingOrder(int orderId) => _context.Order.FirstOrDefault(o => o.Id == orderId);
        public IQueryable<Order> ExistingOrders(int userId) => _context.Order.Where(o => o.UserId == userId);
    }
}