using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Queries
{
    public class UserQueries
    {
        private readonly MarketplaceContext _context;

        public UserQueries(MarketplaceContext context)
        {
            _context = context;
        }

        public User ExistingUser(int userId) => _context.User.FirstOrDefault(u => u.Id == userId);
        
        public User UserByOrderId(int orderId) => _context.User.FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));
    }
}