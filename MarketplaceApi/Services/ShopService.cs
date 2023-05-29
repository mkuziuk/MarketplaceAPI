using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Repositories;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MarketplaceApi.Services
{
    public class ShopService
    {
        private readonly UserRepository _userRepository;
        private readonly OrderRepository _orderRepository;
        private readonly ProductRepository _productRepository;
        private readonly OrderedProductRepository _orderedProductRepository;
        private readonly ShopRepository _shopRepository;
        
        private DateTime _defaultCreationDate = DateTime.Now;

        public ShopService(MarketplaceContext context)
        {
            _userRepository = new UserRepository(context);
            _orderRepository = new OrderRepository(context);
            _productRepository = new ProductRepository(context);
            _orderedProductRepository = new OrderedProductRepository(context);
            _shopRepository = new ShopRepository(context);
        }
        
        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> ProductsInShop(int shopId)
        {
            var shop = _shopRepository.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.BadRequest, new QueryableAndString<object>
                    (null, $"Магазин {shopId} не существует"));

            var products = _productRepository.ProductsInShop(shopId);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
            (StatusCodeEnum.Ok, new QueryableAndString<object>
                (products, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> ShopModerators(int userId, int shopId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.BadRequest, new QueryableAndString<object>
                    (null, $"Пользователь {userId} не существует"));

            var shop = _shopRepository.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.BadRequest, new QueryableAndString<object>
                    (null, $"Магазин {shopId} не существует"));

            var isUserModerator = _userRepository.IsModeratorInShop(userId, shopId);
            if (!isUserModerator & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.BadRequest, new QueryableAndString<object>
                    (null, "У вас нет прав на данную операцию"));

            var shopModeratorIds = _shopRepository.ModeratorsInShop(shopId);
            var moderators = _userRepository.ExistingUsersView(shopModeratorIds);
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
            (StatusCodeEnum.Ok, new QueryableAndString<object>
                (moderators, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> CreateShop(int userId, string shopName)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.BadRequest, $"Пользователь {userId} не существует");
            
            var newShop = new Shop()
            {
                Name = shopName,
                CreationDate = _defaultCreationDate,
                Moderators = new List<User>() {user},
                OwnerId = userId
            };

            user.Seller = true;
            user.ShopsWhereModerator = new List<Shop>() { newShop };
            user.ShopsOwned = new List<Shop>() { newShop };
            
            _userRepository.Update(user);
            
            _shopRepository.Add(newShop);
            _shopRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилсь");
        }

        public KeyValuePair<StatusCodeEnum, string> AddModerator(int userId, int shopId, int newModeratorId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {userId} не существует");
            
            var shop = _shopRepository.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.BadRequest, $"Магазин {shopId} не существует");
            
            var shopOwningUser = user.ShopsOwned.FirstOrDefault(so => so.Id == user.Id);
            if (shopOwningUser == null & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"У вас нет прав на добавление модераторов в магазин {shopId}");

            var newModerator = _userRepository.ExistingUser(newModeratorId);
            if (newModerator == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {newModeratorId} не существует");

            var isUserModerator = _shopRepository.IsUserModerator(newModeratorId, shopId);
            if (isUserModerator)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Модератор {newModeratorId} уже добавлен к магазину");
            
            shop.Moderators.Add(newModerator);
            
            _shopRepository.Update(shop);
            _shopRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteModerator(int userId, int shopId, int moderatorId)
        {
            var user = _userRepository.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {userId} не существует");
            
            var shop = _shopRepository.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Магазин {shopId} не существует");
            
            var moderator = _userRepository.ExistingUser(moderatorId);
            if (moderator == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"Пользователь {moderatorId} не существует");

            var isUserOwner = _shopRepository.IsUserOwner(userId, shopId);
            if (!isUserOwner & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, "У вас не прав на удаление модераторов");
            
            if (user.Id == moderatorId)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, "Вы не можете удалить сами себя из модераторов");

            var shopModerator = _shopRepository.IncludeModeratorInShop(shopId, userId);
            if (shopModerator == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.BadRequest, $"В магазине {shopId} нет модератора {moderatorId}");
            
            moderator.ShopsWhereModerator.Add(shop);
            _userRepository.Attach(moderator);

            shopModerator.Moderators.Remove(moderator);
            _shopRepository.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилось");
        }
    }
}