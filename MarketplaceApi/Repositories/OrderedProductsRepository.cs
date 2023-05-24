using MarketplaceApi.Models;
using System.Linq;

namespace MarketplaceApi.Queries
{
    public class OrderedProductsRepository
    {
        private readonly MarketplaceContext _context;

        public OrderedProductsRepository(MarketplaceContext context)
        {
            _context = context;
        }

        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId) => 
            _context.OrderedProduct.Where(o => o.OrderId == orderId);
    }
}