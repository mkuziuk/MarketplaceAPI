using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Queries;

namespace MarketplaceApi.Services
{
    public class OrderService
    {
        private readonly MarketplaceContext _context;
        private readonly UserQueries _userQueries;
        private readonly OrderQueries _orderQueries;
        private readonly ProductQueries _productQueries;
        private readonly OrderedProductsQueries _orderedProductsQueries;

        public OrderService(MarketplaceContext context)
        {
            _context = context;
            _userQueries = new UserQueries(context);
            _orderQueries = new OrderQueries(context);
            _productQueries = new ProductQueries(context);
            _orderedProductsQueries = new OrderedProductsQueries(context);
        }

        public static DateTime? DefaultOrderDate() => null;
        public static DateTime? DefaultReceiveDate() => null;
        public static DateTime OrderedOrderDate() => DateTime.Now;
        private static DateTime OrderedSellDate() => DateTime.Now;
        private static DateTime OrderedReceiveDate() => OrderedSellDate().AddDays(3);

        public KeyValuePair<StatusCodeEnum, IQueryable<Order>> GetOrder(int userId, int orderId)
        {
            var user = _userQueries.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, IQueryable<Order>>(StatusCodeEnum.BadRequest, null);

            var orderUser = _userQueries.UserByOrderId(orderId);
            if (orderUser.Id != userId & !user.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, IQueryable<Order>>(StatusCodeEnum.BadRequest, null);
            }

            var order = _orderQueries.ExistingOrders(orderId);

            return new KeyValuePair<StatusCodeEnum, IQueryable<Order>>(StatusCodeEnum.Ok, order);
        }

        public KeyValuePair<StatusCodeEnum, IQueryable<Order>> GetUserOrders(int userId)
        {
            var user = _userQueries.ExistingUser(userId);
            var orders = _orderQueries.ExistingOrders(userId);
            
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, IQueryable<Order>>(StatusCodeEnum.BadRequest, orders);
            
            return new KeyValuePair<StatusCodeEnum, IQueryable<Order>>(StatusCodeEnum.Ok, orders);
        }
        
//StatusCodeEnum
        public KeyValuePair<StatusCodeEnum, string> SetupOrder(int userId, int orderId, int wayOfPayment, string deliveryAddress)
        {
            var order = _orderQueries.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest, ($"Заказ {orderId} не существует"));
             
            if (order!.OrderStatusId != (int)OrderSatus.Basket)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest, ("Данный заказ уже оформлен"));

            var currentUser = _userQueries.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest, ($"Пользователь {userId} не существует"));

            var user = _userQueries.UserByOrderId(orderId);
            if (user.Id != userId & !currentUser.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest, ("У вас нет прав на эту операцию"));
              
            var products = _productQueries.ProductsByOrder(orderId)
                .OrderBy(p => p.Id)
                .ToList();
            var orderedProducts = _orderedProductsQueries.OrderedProductsByOrder(orderId)
                .OrderBy(op => op.ProductId)
                .ToList();

            for (var i = 0; i < products.Count(); i++)
            {
                products[i].InStockQuantity -= orderedProducts[i].Quantity;
                _context.Product.Update(products[i]);
            }
              
            if (deliveryAddress != null)
            {
                user.DeliveryAddress = deliveryAddress;
                _context.User.Update(user);
            }
  
            var waysOfPayment = Enum.GetValues(typeof(WaysOfPaymentEnum)).Cast<WaysOfPaymentEnum>();
            var ifWayOfPaymentExists = waysOfPayment.Any(l => (int)l == wayOfPayment);
            if (!ifWayOfPaymentExists)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest, ("Данный способ оплаты не существует"));

            order.WayOfPayment = wayOfPayment;
            order.OrderStatusId = (int)OrderSatus.Ordered;
            order.SellDate = OrderedSellDate();
            order.DeliveryDate = OrderedReceiveDate();
            order.DeliveryAddress = user.DeliveryAddress;
  
            _context.Order.Update(order);
            _context.SaveChanges();
              
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, ("Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> CreateOrder(int userId)
        {
            var user = _userQueries.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest, 
                    $"Пользователь {userId} не существует");

            var order = new Order()
            {
                OrderDate = OrderedOrderDate(),
                OrderStatusId = (int)OrderSatus.Basket,
                UserId = userId
            };
            
            _context.Order.Add(order); 
            _context.SaveChanges();
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteOrder(int userId, int orderId)
        {
            var user = _userQueries.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Пользователь {userId} не существует");
            if (!user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    "У вас не прав на эту операцию");

            var order = _orderQueries.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.BadRequest,
                    $"Заказ {orderId} не существует");
            
            _context.Remove(order); 
            _context.SaveChanges();

            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Удалось");
        }
    }
}