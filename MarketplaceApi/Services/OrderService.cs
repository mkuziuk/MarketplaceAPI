using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.ViewModels;

namespace MarketplaceApi.Services
{
    public class OrderService
    {
        private readonly UserRepository _userRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly OrderedProductRepository _orderedProductRepository;
        private readonly IMapper _mapper;

        public OrderService(MarketplaceContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _orderRepository = new OrderRepository(context);
            _productRepository = new ProductRepository(context);
            _orderedProductRepository = new OrderedProductRepository(context);
            _mapper = mapper;
        }

        //public static DateTime? DefaultOrderDate() => null;
        //public static DateTime? DefaultReceiveDate() => null;
        private static DateTime OrderedOrderDate() => DateTime.Now;
        private static DateTime OrderedSellDate() => DateTime.Now;
        private static DateTime OrderedReceiveDate() => OrderedSellDate().AddDays(3);

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>> GetOrder(int userId, int orderId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                    (StatusCodeEnum.NotFound, new EnumerableAndString<OrderView>
                        (null, $"Пользователь {userId} не существует"));

            var orderUser = _userRepository.UserByOrderId(orderId);
            if (orderUser.Id != userId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                    (StatusCodeEnum.NotFound, new EnumerableAndString<OrderView>
                        (null, "У вас нет прав на эту операцию"));
            

            var orders = _orderRepository.ExistingOrders(orderId);
            if (orders.FirstOrDefault() == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                (StatusCodeEnum.NotFound, new EnumerableAndString<OrderView>
                    (null, $"Заказ {orderId} не существует"));
            
            var ordersView = _mapper.ProjectTo<OrderView>(orders);

            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                (StatusCodeEnum.Ok, new EnumerableAndString<OrderView>(ordersView, null));
        }

        public KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>> GetUserOrders(int userId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                    (StatusCodeEnum.NotFound, new EnumerableAndString<OrderView>
                        (null, $"Пользователь {userId} не существует"));
            
            var orders = _orderRepository.OrdersPerUser(userId);
            if (orders.FirstOrDefault() == null)
                return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                (StatusCodeEnum.NotFound, new EnumerableAndString<OrderView>
                    (null, $"Пользователь {userId} не имеет заказов"));
            
            var ordersView = _mapper.ProjectTo<OrderView>(orders);

            return new KeyValuePair<StatusCodeEnum, EnumerableAndString<OrderView>>
                (StatusCodeEnum.Ok, new EnumerableAndString<OrderView>(ordersView, null));
        }
        
//StatusCodeEnum
        public KeyValuePair<StatusCodeEnum, string> SetupOrder(int userId, int orderId, int wayOfPayment, string deliveryAddress)
        {
            var order = _orderRepository.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ($"Заказ {orderId} не существует"));
             
            if (order!.OrderStatusId != (int)OrderStatusEnum.Basket)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ("Данный заказ уже оформлен"));

            var currentUser = _userRepository.ExistingUser(userId);
            if (currentUser == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ($"Пользователь {userId} не существует"));

            var user = _userRepository.UserByOrderId(orderId);
            if (user.Id != userId & !currentUser.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ("У вас нет прав на эту операцию"));
              
            var products = _productRepository.ProductsByOrder(orderId)
                .OrderBy(p => p.Id)
                .ToList();
            var orderedProducts = _orderedProductRepository.OrderedProductsByOrder(orderId)
                .OrderBy(op => op.ProductId)
                .ToList();

            for (var i = 0; i < products.Count(); i++)
            {
                products[i].InStockQuantity -= orderedProducts[i].Quantity;
                _productRepository.Update(products[i]);
            }
            
            if (deliveryAddress != null)
            {
                user.DeliveryAddress = deliveryAddress;
                _userRepository.Update(user);
            }
  
            var waysOfPayment = Enum.GetValues(typeof(WaysOfPaymentEnum))
                .Cast<WaysOfPaymentEnum>();
            var ifWayOfPaymentExists = waysOfPayment.Any(l => (int)l == wayOfPayment);
            if (!ifWayOfPaymentExists)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ("Данный способ оплаты не существует"));

            order.WayOfPayment = wayOfPayment;
            order.OrderStatusId = (int)OrderStatusEnum.Ordered;
            order.SellDate = OrderedSellDate();
            order.DeliveryDate = OrderedReceiveDate();
            order.DeliveryAddress = user.DeliveryAddress;
  
            _orderRepository.Update(order);
            _orderRepository.Save();
              
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, ("Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> CreateOrder(int userId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound, 
                    $"Пользователь {userId} не существует");

            var order = new Order()
            {
                OrderDate = OrderedOrderDate(),
                OrderStatusId = (int)OrderStatusEnum.Basket,
                UserId = userId
            };
            
            _orderRepository.Add(order);
            _orderRepository.Save();
            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteOrder(int userId, int orderId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Пользователь {userId} не существует");
            if (!user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    "У вас не прав на эту операцию");

            var order = _orderRepository.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.NotFound,
                    $"Заказ {orderId} не существует");
            
            _orderRepository.Remove(order);
            _orderRepository.Save();

            return new KeyValuePair<StatusCodeEnum, string>(StatusCodeEnum.Ok, "Получилось");
        }
    }
}