using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class ShopRepository : Repository
    {
        public ShopRepository(MarketplaceContext context) : base(context) {}

        public Shop ExistingShop(int shopId) => Context.Shop.FirstOrDefault(s => s.Id == shopId);
    }
}