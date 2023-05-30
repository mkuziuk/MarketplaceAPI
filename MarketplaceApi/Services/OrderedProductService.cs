using System.Collections.Generic;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;

namespace MarketplaceApi.Services
{
    public class OrderedProductService
    {
        private readonly UserRepositoryBase _userRepositoryBase;
        private readonly OrderRepositoryBase _orderRepositoryBase;
        private readonly ProductRepositoryBase _productRepositoryBase;
        private readonly OrderedProductRepositoryBase _orderedProductRepositoryBase;

        public OrderedProductService(MarketplaceContext context)
        {
            _userRepositoryBase = new UserRepositoryBase(context);
            _orderRepositoryBase = new OrderRepositoryBase(context);
            _productRepositoryBase = new ProductRepositoryBase(context);
            _orderedProductRepositoryBase = new OrderedProductRepositoryBase(context);
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> GetProductsInTheOrder(int userId, int orderId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Пользователь {userId} не существует"));
            
            var order = _orderRepositoryBase.ExistingOrder(orderId);
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

            var products = _productRepositoryBase.GetProductsInOrderWithQuantity(orderId);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.Ok, new QueryableAndString<object>(products, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> ChangeQuantity(int userId, int orderId, int productId, int newQuantity)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");

            var order = _orderRepositoryBase.OrderPerUser(userId);
            if (order.Id != orderId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на редактироване данного заказа");
            
            if (order.OrderStatusId != (int)OrderSatus.Basket)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "Заказ уже оформлен");

            var product = _productRepositoryBase.ExistingProductView(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товар {productId} не существует");

            var orderedProduct = _orderedProductRepositoryBase.ProductInOrder(orderId, productId);
            if (orderedProduct == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Продукта {productId} в заказе {orderId} не существует");
            
            if (newQuantity <= 0)
            {
                _orderedProductRepositoryBase.Delete(orderedProduct);
                _orderedProductRepositoryBase.Save();
                
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.Ok, $"Продукт {productId} был удалён из заказа {orderId}");
            }
            
            if (newQuantity > product.InStockQuantity)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"В наличие {product.InStockQuantity} товаров");
            
            orderedProduct.Quantity = newQuantity;
            _orderedProductRepositoryBase.Update(orderedProduct);
            _orderedProductRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилось");
        }
        
        public KeyValuePair<StatusCodeEnum, string> AddProductToOrder(int userId, int orderId, int productId, int quantity)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");

            var order = _orderRepositoryBase.OrderPerUser(userId);
            if (order.Id != orderId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на редактироване данного заказа");
            
            if (order.OrderStatusId != (int)OrderSatus.Basket)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "Заказ уже оформлен");

            var product = _productRepositoryBase.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товар {productId} не существует");

            var orderedProduct = _orderedProductRepositoryBase.ProductInOrder(orderId, productId);
            if (orderedProduct == null)
            {
                if (product.InStockQuantity < quantity)
                    return new KeyValuePair<StatusCodeEnum, string>
                        (StatusCodeEnum.NotFound, $"В наличие {product.InStockQuantity} товаров");
                
                order.Products.Add(product);
                
                _orderRepositoryBase.Update(order);
                _orderedProductRepositoryBase.Save();

                var newOrderedProduct = _orderedProductRepositoryBase.ProductInOrder(orderId, productId);
                
                newOrderedProduct.Quantity = quantity;
                
                _orderedProductRepositoryBase.Update(newOrderedProduct);
                _orderedProductRepositoryBase.Save();
                
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.Ok, "Получилось добавить товар");
            }
            
            if (product.InStockQuantity < orderedProduct.Quantity + quantity)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"В наличие {product.InStockQuantity} товаров"); 
            
            orderedProduct.Quantity += quantity;
            _orderedProductRepositoryBase.Update(orderedProduct);
            _orderedProductRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, $"Получилось добавить больше {productId} в заказ");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteProductFromOrder(int userId, int orderId, int productId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");

            var order = _orderRepositoryBase.OrderPerUser(userId);
            if (order == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, ($"Заказ {orderId} не существует"));
            
            if (order.Id != orderId & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас нет прав на редактироване данного заказа");
            
            var product = _productRepositoryBase.ExistingProduct(productId);
            if (product == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товар {productId} не существует");

            var orderProduct = _orderRepositoryBase.IncludeProductInOrder(orderId, productId);
            if (orderProduct == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Товара {productId} нет в заказе {orderId}");
            
            product.Orders.Add(order);
            _productRepositoryBase.Attach(product);
            
            orderProduct.Products.Remove(product);
            _orderedProductRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, $"Получилось удалить товар {productId} из заказа");
        }
    }
}