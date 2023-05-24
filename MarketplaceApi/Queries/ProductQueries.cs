using System.Collections.Generic;
using MarketplaceApi.Models;
using System.Linq;

namespace MarketplaceApi.Queries
{
    public class ProductQueries
    {
        private readonly MarketplaceContext _context;

        public ProductQueries(MarketplaceContext context)
        {
            _context = context;
        }

        public IQueryable<Product> ProductsByOrder(int orderId) => _context.Product.Where(p => p.Orders.Any(o => o.Id == orderId));
    }
}