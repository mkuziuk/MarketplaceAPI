using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class ShopRepository : Repository
    {
        public ShopRepository(MarketplaceContext context) : base(context) {}

        public Shop ExistingShop(int shopId) => Context.Shop.FirstOrDefault(s => s.Id == shopId);
        //public IQueryable<Shop> ExistingShops(int shopId) => Context.Shop.Where(s => s.Id == shopId);
        
        public bool IsUserModerator(int userId, int shopId) => 
            Context.Shop.Any(s=> s.Moderators.Any(m=> m.Id == userId) && s.Id == shopId);
        
        public Shop IncludeModeratorInShop(int shopId, int userId) => Context.Shop
            .FirstOrDefault(s => s.Id == shopId && s.Moderators
                .Any(m => m.Id == userId));
        
        public IEnumerable<int> ModeratorsInShop(int shopId)
        {
            var shops = Context.ShopModerator.Where(s => s.ShopId == shopId);

            var users = Context.User.AsQueryable();
            
            return users
                .Join(shops,
                    u => u.Id,
                    sm => sm.ModeratorId,
                    (u, sm) => u.Id);
        }

        public bool IsUserOwner(int userId, int shopId) => 
            Context.Shop.Any(s => s.OwnerId == userId && s.Id == shopId);

        public void Add(Shop shop) => Context.Shop.Add(shop);
        public void Update(Shop shop) => Context.Shop.Update(shop);
        public void Delete(Shop shop) => Context.Shop.Remove(shop);
    }
}