using System.Collections.Generic;
using System.Threading.Tasks;
using MarketplaceApi.Models;

namespace MarketplaceApi.IRepositories
{
    public interface IShopRepository
    {
        public Shop ExistingShop(int shopId);

        public Task<Shop> ExistingShopAsync(int shopId);

        //public IQueryable<Shop> ExistingShops(int shopId) => Context.Shop.Where(s => s.Id == shopId);
        
        public bool IsUserModerator(int userId, int shopId);
        
        public Shop IncludeModeratorInShop(int shopId, int userId);

        public IEnumerable<int> ModeratorsInShop(int shopId);

        public bool IsUserOwner(int userId, int shopId);

        public void Add(Shop shop);
        public void Update(Shop shop);
        public void Delete(Shop shop);
        public void Save();
    }
}