using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketplaceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Repositories
{
    public class UserRepository : RepositoryBase
    {
        public UserRepository(MarketplaceContext context) : base(context) {}

        public User ExistingUser(int userId) => Context.User.FirstOrDefault(u => u.Id == userId);
        public async Task<User> ExistingUserAsync(int userId) => 
            await Context.User.FirstOrDefaultAsync(u => u.Id == userId);

        public IQueryable<User> ExistingUsers(int userId) => Context.User.Where(u => u.Id == userId);

        public IQueryable<User> ExistingUsers(IEnumerable<int> userIds) => Context.User
                .Where(u => userIds.Contains(u.Id));

        public async Task<User[]> ExistingUsersAsync(IEnumerable<int> userIds)
        {
            var getUserTasks = userIds
                .Select(userId => Context.User.FirstOrDefaultAsync(u => u.Id == userId)).ToList();

            return await Task.WhenAll(getUserTasks);
        }

        public User UserByOrderId(int orderId) => Context.User
            .FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));
        
        public bool IsModeratorInShop(int userId, int shopId) => Context.User
            .Any(u => u.ShopsWhereModerator
                .FirstOrDefault(swm => swm.Moderators
                    .Any(m => m.Id == userId)).Id == shopId);

        public void Update(User user) => Context.User.Update(user);
        public void Add(User user) => Context.User.Add(user);
        public void Attach(User user) => Context.User.Attach(user);
        public void Delete(User user) => Context.User.Remove(user);
    }
}