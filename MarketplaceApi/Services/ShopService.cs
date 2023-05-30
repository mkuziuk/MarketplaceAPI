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
        private readonly UserRepositoryBase _userRepositoryBase;
        private readonly OrderRepositoryBase _orderRepositoryBase;
        private readonly ProductRepositoryBase _productRepositoryBase;
        private readonly OrderedProductRepositoryBase _orderedProductRepositoryBase;
        private readonly ShopRepositoryBase _shopRepositoryBase;
        
        private DateTime _defaultCreationDate = DateTime.Now;

        public ShopService(MarketplaceContext context)
        {
            _userRepositoryBase = new UserRepositoryBase(context);
            _orderRepositoryBase = new OrderRepositoryBase(context);
            _productRepositoryBase = new ProductRepositoryBase(context);
            _orderedProductRepositoryBase = new OrderedProductRepositoryBase(context);
            _shopRepositoryBase = new ShopRepositoryBase(context);
        }
        
        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> ProductsInShop(int shopId)
        {
            var shop = _shopRepositoryBase.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Магазин {shopId} не существует"));

            var products = _productRepositoryBase.ProductsInShop(shopId);
            if (products == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Товаров в магазине {shopId} нет"));
            
            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
            (StatusCodeEnum.Ok, new QueryableAndString<object>
                (products, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, QueryableAndString<object>> ShopModerators(int userId, int shopId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Пользователь {userId} не существует"));

            var shop = _shopRepositoryBase.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, $"Магазин {shopId} не существует"));

            var isUserModerator = _userRepositoryBase.IsModeratorInShop(userId, shopId);
            if (!isUserModerator & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
                (StatusCodeEnum.NotFound, new QueryableAndString<object>
                    (null, "У вас нет прав на данную операцию"));

            var shopModeratorIds = _shopRepositoryBase.ModeratorsInShop(shopId);
            var moderators = _userRepositoryBase.ExistingUsersView(shopModeratorIds);

            return new KeyValuePair<StatusCodeEnum, QueryableAndString<object>>
            (StatusCodeEnum.Ok, new QueryableAndString<object>
                (moderators, "Получилось"));
        }

        public KeyValuePair<StatusCodeEnum, string> CreateShop(int userId, string shopName)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");
            
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
            
            _userRepositoryBase.Update(user);
            
            _shopRepositoryBase.Add(newShop);
            _shopRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилсь");
        }

        public KeyValuePair<StatusCodeEnum, string> AddModerator(int userId, int shopId, int newModeratorId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");
            
            var shop = _shopRepositoryBase.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.NotFound, $"Магазин {shopId} не существует");
            
            var shopOwningUser = user.ShopsOwned.FirstOrDefault(so => so.Id == user.Id);
            if (shopOwningUser == null & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"У вас нет прав на добавление модераторов в магазин {shopId}");

            var newModerator = _userRepositoryBase.ExistingUser(newModeratorId);
            if (newModerator == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {newModeratorId} не существует");

            var isUserModerator = _shopRepositoryBase.IsUserModerator(newModeratorId, shopId);
            if (isUserModerator)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Модератор {newModeratorId} уже добавлен к магазину");
            
            shop.Moderators.Add(newModerator);
            
            _shopRepositoryBase.Update(shop);
            _shopRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилось");
        }

        public KeyValuePair<StatusCodeEnum, string> DeleteModerator(int userId, int shopId, int moderatorId)
        {
            var user = _userRepositoryBase.ExistingUser(userId);
            if (user == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {userId} не существует");
            
            var shop = _shopRepositoryBase.ExistingShop(shopId);
            if (shop == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Магазин {shopId} не существует");
            
            var moderator = _userRepositoryBase.ExistingUser(moderatorId);
            if (moderator == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"Пользователь {moderatorId} не существует");

            var isUserOwner = _shopRepositoryBase.IsUserOwner(userId, shopId);
            if (!isUserOwner & !user.Admin)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "У вас не прав на удаление модераторов");
            
            if (user.Id == moderatorId)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, "Вы не можете удалить сами себя из модераторов");

            var shopModerator = _shopRepositoryBase.IncludeModeratorInShop(shopId, userId);
            if (shopModerator == null)
                return new KeyValuePair<StatusCodeEnum, string>
                    (StatusCodeEnum.NotFound, $"В магазине {shopId} нет модератора {moderatorId}");
            
            moderator.ShopsWhereModerator.Add(shop);
            _userRepositoryBase.Attach(moderator);

            shopModerator.Moderators.Remove(moderator);
            _shopRepositoryBase.Save();
            
            return new KeyValuePair<StatusCodeEnum, string>
                (StatusCodeEnum.Ok, "Получилось");
        }
    }
}