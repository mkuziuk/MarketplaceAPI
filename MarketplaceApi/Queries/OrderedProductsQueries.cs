using MarketplaceApi.Models;
using System.Linq;

namespace MarketplaceApi.Queries
{
    public class OrderedProductsQueries
    {
        private readonly MarketplaceContext _context;

        public OrderedProductsQueries(MarketplaceContext context)
        {
            _context = context;
        }

        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId) => 
            _context.OrderedProduct.Where(o => o.OrderId == orderId);
    }
}