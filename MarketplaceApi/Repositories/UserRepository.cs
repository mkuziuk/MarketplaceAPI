using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketplaceApi.IRepositories;
using MarketplaceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MarketplaceContext _context;

        public UserRepository(MarketplaceContext context)
        {
            _context = context;
        }

        public User ExistingUser(int userId) => _context.User.FirstOrDefault(u => u.Id == userId);
        public async Task<User> ExistingUserAsync(int userId) => 
            await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

        public IQueryable<User> ExistingUsers(int userId) => _context.User.Where(u => u.Id == userId);

        public IQueryable<User> ExistingUsers(IEnumerable<int> userIds) => _context.User
                .Where(u => userIds.Contains(u.Id));

        public async Task<User[]> ExistingUsersAsync(IEnumerable<int> userIds)
        {
            var getUserTasks = userIds
                .Select(userId => _context.User.FirstOrDefaultAsync(u => u.Id == userId)).ToList();

            return await Task.WhenAll(getUserTasks);
        }

        public User UserByOrderId(int orderId) => _context.User
            .FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));
        
        public bool IsModeratorInShop(int userId, int shopId) => _context.User
            .Any(u => u.ShopsWhereModerator
                .FirstOrDefault(swm => swm.Moderators
                    .Any(m => m.Id == userId)).Id == shopId);
        public async Task<bool> IsModeratorInShopAsync(int userId, int shopId) => await _context.User
            .AnyAsync(u => u.ShopsWhereModerator
                .FirstOrDefault(swm => swm.Moderators
                    .Any(m => m.Id == userId)).Id == shopId);

        public void Update(User user) => _context.User.Update(user);
        public void Add(User user) => _context.User.Add(user);
        public void Attach(User user) => _context.User.Attach(user);
        public void Delete(User user) => _context.User.Remove(user);
        public void Save() => _context.SaveChanges();
    }
}