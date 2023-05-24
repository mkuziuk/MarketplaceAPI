using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Queries
{
    public class OrderQueries
    {
        private readonly MarketplaceContext _context;

        public OrderQueries(MarketplaceContext context)
        {
            _context = context;
        }

        public Order ExistingOrder(int orderId) => _context.Order.FirstOrDefault(o => o.Id == orderId);
        public IQueryable<Order> ExistingOrders(int userId) => _context.Order.Where(o => o.UserId == userId);
    }
}