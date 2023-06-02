using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketplaceApi.IRepositories;
using MarketplaceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly MarketplaceContext _context;
        
        public ShopRepository(MarketplaceContext context)
        {
            _context = context;
        }

        public Shop ExistingShop(int shopId) => _context.Shop.FirstOrDefault(s => s.Id == shopId);

        public async Task<Shop> ExistingShopAsync(int shopId) =>
            await _context.Shop.FirstOrDefaultAsync(s => s.Id == shopId);

        //public IQueryable<Shop> ExistingShops(int shopId) => Context.Shop.Where(s => s.Id == shopId);
        
        public bool IsUserModerator(int userId, int shopId) => 
            _context.Shop.Any(s=> s.Moderators.Any(m=> m.Id == userId) && s.Id == shopId);
        
        public Shop IncludeModeratorInShop(int shopId, int userId) => _context.Shop
            .FirstOrDefault(s => s.Id == shopId && s.Moderators
                .Any(m => m.Id == userId));
        
        public IEnumerable<int> ModeratorsInShop(int shopId)
        {
            var shops = _context.ShopModerator.Where(s => s.ShopId == shopId);

            var users = _context.User.AsQueryable();
            
            return users
                .Join(shops,
                    u => u.Id,
                    sm => sm.ModeratorId,
                    (u, sm) => u.Id);
        }

        public bool IsUserOwner(int userId, int shopId) => 
            _context.Shop.Any(s => s.OwnerId == userId && s.Id == shopId);

        public void Add(Shop shop) => _context.Shop.Add(shop);
        public void Update(Shop shop) => _context.Shop.Update(shop);
        public void Delete(Shop shop) => _context.Shop.Remove(shop);
        public void Save() => _context.SaveChanges();
    }
}