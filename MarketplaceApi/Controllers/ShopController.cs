using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    public class ShopController : ControllerBase
    {
        private readonly MarketplaceContext _context;
        private readonly ShopService _shopService;
        
        public ShopController(MarketplaceContext context)
        {
            _context = context;
            _shopService = new ShopService(context);
        }

        [HttpGet("productsinshop")]
        public IActionResult GetShopProducts(int shopId)
        {
            var result = _shopService.ProductsInShop(shopId);

            return DoSwitch(result);
        }
        
        [HttpGet("shopmods")]
        public IActionResult GetShopModerators(int userId, int shopId)
        {
            var result = _shopService.ShopModerators(userId, shopId);

            return DoSwitch(result);
        }

        [HttpPost("createshop")]
        public IActionResult PostCreateShop(int userId, string shopName)
        {
            var result = _shopService.CreateShop(userId, shopName);

            return DoSwitch(result);
        }

        [HttpPatch("addmoderator")]
        public IActionResult PatchAddModerator(int userId, int shopId, int newModeratorId)
        {
            var result = _shopService.AddModerator(userId, shopId, newModeratorId);

            return DoSwitch(result);
        }

        [HttpDelete("deletemoderator")]
        public IActionResult DeleteModerator(int userId, int shopId, int moderatorId)
        {
            var result = _shopService.DeleteModerator(userId, shopId, moderatorId);

            return DoSwitch(result);

            /*
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");

            var shop = _context.Shop.FirstOrDefault(s => s.Id == shopId);
            if (shop == null)
                return BadRequest($"Магазин {shopId} не существует");
            
            var moderator = _context.User.FirstOrDefault(u => u.Id == moderatorId);
            if (moderator == null)
                return BadRequest($"Пользователь {moderatorId} не существует");
            
            var ifIsOwner = _context.Shop.Any(s => s.OwnerId == userId);
            if (!ifIsOwner & !moderator.Admin)
                return BadRequest("У вас нет прав на удаление модераторов");
            
            var ifIsModeratorOwner = _context.User.Any(u =>
                u.ShopsOwned.Any(so => so.OwnerId == moderatorId));
            if (ifIsModeratorOwner)
                return BadRequest("Вы не можете удалить сами себя из модераторов");

            var shopModerator = _context.Shop
                .Include(s => s.Moderators)
                .ThenInclude(m => m.ShopsOwned)
                .FirstOrDefault(s => s.Id == shopId && s.Moderators
                    .Any(m => m.Id == moderatorId));
            if (shopModerator == null)
                return BadRequest($"В магазине {shopId} нет модератора {moderatorId}");

            moderator.ShopsWhereModerator.Add(shop);
            _context.User.Attach(moderator);
            
            shopModerator.Moderators.Remove(moderator);
            _context.SaveChanges();
            
            return Ok();
            */
        }
    }
}