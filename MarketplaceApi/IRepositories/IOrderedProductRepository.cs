using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.IRepositories
{
    public interface IOrderedProductRepository
    {
        public IQueryable<OrderedProduct> OrderedProductsByOrder(int orderId);
        
        public OrderedProduct ProductInOrder(int orderId, int productId);

        public void Delete(OrderedProduct orderedProduct);
        public void Update(OrderedProduct orderedProduct);

        public void Save();
    }
}