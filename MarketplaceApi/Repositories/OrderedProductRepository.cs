using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class OrderedProductRepository : RepositoryBase
    {
        public OrderedProductRepository(MarketplaceContext context) : base(context){}
        
        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId) => 
            _context.OrderedProduct.Where(o => o.OrderId == orderId);
        
        public OrderedProduct ProductInOrder(int orderId, int productId) => 
            _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId && o.ProductId == productId);

        public void Delete(OrderedProduct orderedProduct) => _context.OrderedProduct.Remove(orderedProduct);
        public void Update(OrderedProduct orderedProduct) => _context.OrderedProduct.Update(orderedProduct);
    }
}