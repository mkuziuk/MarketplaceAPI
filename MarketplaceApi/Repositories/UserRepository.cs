using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class UserRepository : Repository
    {
        private readonly MarketplaceContext _context;

        public UserRepository(MarketplaceContext context) : base(context)
        {
            _context = context;
        }

        public User ExistingUser(int userId) => _context.User.FirstOrDefault(u => u.Id == userId);
        
        public IQueryable<User> ExistingUsers(int userId) => _context.User.Where(u => u.Id == userId);

        public User UserByOrderId(int orderId) => _context.User
            .FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));

        public bool IsModeratorInShop(int userId, int shopId) => _context.User
            .Any(u => u.ShopsWhereModerator
                .FirstOrDefault(swm => swm.Moderators
                    .Any(m => m.Id == userId)).Id == shopId);
        
        public void Update(User user) => _context.User.Update(user);
        public void Add(User user) => Context.User.Add(user);
        public void Delete(User user) => Context.User.Remove(user);
    }
}