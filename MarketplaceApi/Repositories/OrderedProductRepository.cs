using System.Linq;
using MarketplaceApi.IRepositories;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class OrderedProductRepository : IOrderedProductRepository
    {
        private readonly MarketplaceContext _context;

        public OrderedProductRepository(MarketplaceContext context)
        {
            _context = context;
        }
        
        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId) => 
            _context.OrderedProduct.Where(o => o.OrderId == orderId);
        
        public OrderedProduct ProductInOrder(int orderId, int productId) => 
            _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId && o.ProductId == productId);

        public void Delete(OrderedProduct orderedProduct) => _context.OrderedProduct.Remove(orderedProduct);
        public void Update(OrderedProduct orderedProduct) => _context.OrderedProduct.Update(orderedProduct);
        public void Save() => _context.SaveChanges();
    }
}