using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using MarketplaceApi.Views;

namespace MarketplaceApi.Services
{
    public class OrderedProductService
    {
        private readonly UserRepository _userRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly OrderedProductRepository _orderedProductRepository;
        private readonly IMapper _mapper;

        public OrderedProductService(MarketplaceContext context, IMapper mapper)
        {
            _userRepository = new UserRepository(context);
            _orderRepository = new OrderRepository(context);
            _productRepository = new ProductRepository(context);
            _orderedProductRepository = new OrderedProductRepository(context);
            _mapper = mapper;
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> GetProductsInTheOrder(int userId, int orderId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Пользователь {userId} не существует"));
            
            var order = _orderRepository.ExistingOrder(orderId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Заказ {orderId} не существует"));
            
            if (order.UserId != userId & !user.Admin)
            {
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, "У вас нет прав на эту операцию"));
            }

            var products = _productRepository.GetProductsInOrderWithQuantity(orderId).Item1;
            var productsView = _mapper.ProjectTo<ProductView>(products);
            var orderedProducts = _productRepository.GetProductsInOrderWithQuantity(orderId).Item2;
            
            var productsWithQuantity = productsView
                .Join(orderedProducts,
                p => p.Id, 
                op => op.ProductId,
                (p, op) => new {p, op.Quantity});
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.Ok, new QueryableAndString<object>(productsWithQuantity, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> ChangeQuantity(int userId, int orderId, int productId, int newQuantity)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");

            var order = _orderRepository.OrderPerUser(userId);
            if (order.Id != orderId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на редактироване данного заказа");
            
            if (order.OrderStatusId != (int)OrderStatusEnum.Basket)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "Заказ уже оформлен");

            var product = _productRepository.ExistingProduct(productId);
            var productView = _mapper.Map<ProductView>(product);
            if (productView == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товар {productId} не существует");

            var orderedProduct = _orderedProductRepository.ProductInOrder(orderId, productId);
            if (orderedProduct == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Продукта {productId} в заказе {orderId} не существует");
            
            if (newQuantity <= 0)
            {
                _orderedProductRepository.Delete(orderedProduct);
                _orderedProductRepository.Save();
                
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.Ok, $"Продукт {productId} был удалён из заказа {orderId}");
            }
            
            if (newQuantity > productView.InStockQuantity)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"В наличие {productView.InStockQuantity} товаров");
            
            orderedProduct.Quantity = newQuantity;
            _orderedProductRepository.Update(orderedProduct);
            _orderedProductRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилось");
        }
        
        public KeyValuePair<StatusCodeEnum, string> AddProductToOrder(int userId, int orderId, int productId, int quantity)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");

            var order = _orderRepository.OrderPerUser(userId);
            if (order.Id != orderId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на редактироване данного заказа");
            
            if (order.OrderStatusId != (int)OrderStatusEnum.Basket)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "Заказ уже оформлен");

            var product = _productRepository.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товар {productId} не существует");

            var orderedProduct = _orderedProductRepository.ProductInOrder(orderId, productId);
            if (orderedProduct == null)
            {
                if (product.InStockQuantity < quantity)
                    return new KeyValuePair<StatusCodeEnum, string>
                        (StatusCodeEnum.NotFound, $"В наличие {product.InStockQuantity} товаров");
                
                order.Products.Add(product);
                
                _orderRepository.Update(order);
                _orderedProductRepository.Save();

                var newOrderedProduct = _orderedProductRepository.ProductInOrder(orderId, productId);
                
                newOrderedProduct.Quantity = quantity;
                
                _orderedProductRepository.Update(newOrderedProduct);
                _orderedProductRepository.Save();
                
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.Ok, "Получилось добавить товар");
            }
            
            if (product.InStockQuantity < orderedProduct.Quantity + quantity)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"В наличие {product.InStockQuantity} товаров"); 
            
            orderedProduct.Quantity += quantity;
            _orderedProductRepository.Update(orderedProduct);
            _orderedProductRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, $"Получилось добавить больше {productId} в заказ");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteProductFromOrder(int userId, int orderId, int productId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");

            var order = _orderRepository.OrderPerUser(userId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ($"Заказ {orderId} не существует"));
            
            if (order.Id != orderId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на редактироване данного заказа");
            
            var product = _productRepository.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товар {productId} не существует");

            var orderProduct = _orderRepository.IncludeProductInOrder(orderId, productId);
            if (orderProduct == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товара {productId} нет в заказе {orderId}");
            
            product.Orders.Add(order);
            _productRepository.Attach(product);
            
            orderProduct.Products.Remove(product);
            _orderedProductRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, $"Получилось удалить товар {productId} из заказа");
        }
    }
}