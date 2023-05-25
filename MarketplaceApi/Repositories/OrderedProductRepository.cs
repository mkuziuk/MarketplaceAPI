using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.Repositories
{
    public class OrderedProductRepository : Repository
    {
        public OrderedProductRepository(MarketplaceContext context) : base(context){}
        
        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId) => 
            Context.OrderedProduct.Where(o => o.OrderId == orderId);
        
        public OrderedProduct ProductInOrder(int orderId, int productId) => 
            Context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId && o.ProductId == productId);

        public void Delete(OrderedProduct orderedProduct) => Context.OrderedProduct.Remove(orderedProduct);
        public void Update(OrderedProduct orderedProduct) => Context.OrderedProduct.Update(orderedProduct);
    }
}