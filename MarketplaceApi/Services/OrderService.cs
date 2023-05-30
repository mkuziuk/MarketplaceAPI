using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.Views;

namespace MarketplaceApi.Services
{
    public class OrderService
    {
        private readonly UserRepositoryBase _userRepositoryBase;
        private readonly OrderRepositoryBase _orderRepositoryBase;
        private readonly ProductRepositoryBase _productRepositoryBase;
        private readonly OrderedProductRepositoryBase _orderedProductRepositoryBase;

        public OrderService(MarketplaceContext context)
        {
            _userRepositoryBase = new UserRepositoryBase(context);
            _orderRepositoryBase = new OrderRepositoryBase(context);
            _productRepositoryBase = new ProductRepositoryBase(context);
            _orderedProductRepositoryBase = new OrderedProductRepositoryBase(context);
        }

        //public static DateTime? DefaultOrderDate() => null;
        //public static DateTime? DefaultReceiveDate() => null;
        private static DateTime OrderedOrderDate() => DateTime.Now;
        private static DateTime OrderedSellDate() => DateTime.Now;
        private static DateTime OrderedReceiveDate() => OrderedSellDate().AddDays(3);

        public KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>> GetOrder(int userId, int orderId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                    (StatusCodeEnum.NotFound, new QueryableAndString<OrderView>
                        (null, $"Пользователь {userId} не существует"));

            var orderUser = _userRepositoryBase.UserByOrderId(orderId);
            if (orderUser.Id != userId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                    (StatusCodeEnum.NotFound, new QueryableAndString<OrderView>
                        (null, "У вас нет прав на эту операцию"));
            

            var order = _orderRepositoryBase.ExistingOrdersView(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                (StatusCodeEnum.NotFound, new QueryableAndString<OrderView>
                    (null, $"Заказ {orderId} не существует"));

            return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                (StatusCodeEnum.Ok, new QueryableAndString<OrderView>(order, null));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>> GetUserOrders(int userId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                    (StatusCodeEnum.NotFound, new QueryableAndString<OrderView>
                        (null, $"Пользователь {userId} не существует"));
            
            var orders = _orderRepositoryBase.OrdersPerUserView(userId);
            if (orders == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                (StatusCodeEnum.NotFound, new QueryableAndString<OrderView>
                    (null, $"Пользователь {userId} не имеет заказов"));
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<OrderView>>
                (StatusCodeEnum.Ok, new QueryableAndString<OrderView>(orders, null));
        }
        
//StatusCodeEnum
        public KeyValuePair<StatusCodeEnum, string> SetupOrder(int userId, int orderId, int wayOfPayment, string deliveryAddress)
        {
            var order = _orderRepositoryBase.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ($"Заказ {orderId} не существует"));
             
            if (order!.OrderStatusId != (int)OrderSatus.Basket)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ("Данный заказ уже оформлен"));

            var currentUser = _userRepositoryBase.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ($"Пользователь {userId} не существует"));

            var user = _userRepositoryBase.UserByOrderId(orderId);
            if (user.Id != userId & !currentUser.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ("У вас нет прав на эту операцию"));
              
            var products = _productRepositoryBase.ProductsByOrder(orderId)
                .OrderBy(p => p.Id)
                .ToList();
            var orderedProducts = _orderedProductRepositoryBase.OrderedProductsByOrder(orderId)
                .OrderBy(op => op.ProductId)
                .ToList();

            for (var i = 0; i < products.Count(); i++)
            {
                products[i].InStockQuantity -= orderedProducts[i].Quantity;
                _productRepositoryBase.Update(products[i]);
            }
            
            if (deliveryAddress != null)
            {
                user.DeliveryAddress = deliveryAddress;
                _userRepositoryBase.Update(user);
            }
  
            var waysOfPayment = Enum.GetValues(typeof(WaysOfPaymentEnum))
                .Cast<WaysOfPaymentEnum>();
            var ifWayOfPaymentExists = waysOfPayment.Any(l => (int)l == wayOfPayment);
            if (!ifWayOfPaymentExists)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ("Данный способ оплаты не существует"));

            order.WayOfPayment = wayOfPayment;
            order.OrderStatusId = (int)OrderSatus.Ordered;
            order.SellDate = OrderedSellDate();
            order.DeliveryDate = OrderedReceiveDate();
            order.DeliveryAddress = user.DeliveryAddress;
  
            _orderRepositoryBase.Update(order);
            _orderRepositoryBase.Save();
              
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, ("Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> CreateOrder(int userId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound, 
                    $"Пользователь {userId} не существует");

            var order = new Order()
            {
                OrderDate = OrderedOrderDate(),
                OrderStatusId = (int)OrderSatus.Basket,
                UserId = userId
            };
            
            _orderRepositoryBase.Add(order);
            _orderRepositoryBase.Save();
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteOrder(int userId, int orderId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не существует");
            if (!user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас не прав на эту операцию");

            var order = _orderRepositoryBase.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Заказ {orderId} не существует");
            
            _orderRepositoryBase.Remove(order);
            _orderRepositoryBase.Save();

            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }
    }
}