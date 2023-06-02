using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketplaceApi.Models;

namespace MarketplaceApi.IRepositories
{
    public interface IUserRepository
    {
        public User ExistingUser(int userId);
        public Task<User> ExistingUserAsync(int userId);

        public IQueryable<User> ExistingUsers(int userId);

        public IQueryable<User> ExistingUsers(IEnumerable<int> userIds);

        public Task<User[]> ExistingUsersAsync(IEnumerable<int> userIds);

        public User UserByOrderId(int orderId);
        
        public bool IsModeratorInShop(int userId, int shopId);
        public Task<bool> IsModeratorInShopAsync(int userId, int shopId);

        public void Update(User user);
        public void Add(User user);
        public void Attach(User user);
        public void Delete(User user);
        public void Save();
    }
}