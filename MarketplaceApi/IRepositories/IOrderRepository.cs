using System.Linq;
using MarketplaceApi.Models;

namespace MarketplaceApi.IRepositories
{
    public interface IOrderRepository
    {
        public Order ExistingOrder(int orderId);
        public IQueryable<Order> ExistingOrders(int orderId);

        public Order OrderPerUser(int userId);
        //public OrderView OrderPerUserView(int userId);
        public IQueryable<Order> OrdersPerUser(int userId);

        public Order IncludeProductInOrder(int orderId, int productId);

        public void Update(Order order);

        public void Add(Order order);

        public void Remove(Order order);

        public void Save();
    }
}