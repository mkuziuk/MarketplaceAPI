using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class OrderedProductsRepository : Repository
    {
        public OrderedProductsRepository(MarketplaceContext context) : base(context){}
        
        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId) => 
            Context.OrderedProduct.Where(o => o.OrderId == orderId);
    }
}