using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    public class ShopController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public ShopController(MarketplaceContext context)
        {
            _context = context;
        }

        [HttpGet("shopproducts")]
        public IActionResult GetShopProducts(int id)
        {
            var shop = _context.Shop.AsNoTracking().FirstOrDefault(s => s.Id == id);
            if (shop == null) return BadRequest($"Магазин {id} не существует");

            return Ok(shop);
        }
        
        [HttpGet("shopmods")]
        public IActionResult GetShopOwners(int userId, int shopId)
        {
            var currentUser = _context.User.AsNoTracking().FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var shop = _context.Shop.AsNoTracking().FirstOrDefault(s => s.Id == shopId);
            if (shop == null) 
                return BadRequest($"Магазин {shopId} не существует");
            
            var admittedUser = _context.User.AsNoTracking().FirstOrDefault(u => 
                u.Admin || u.ShopsWhereModerator.Any(sm=> sm.Moderators
                    .Any(m=> m.Id == userId)));
            if (admittedUser == null)
                return BadRequest("У вас нет прав на данную операцию");
            
            return Ok(shop.Moderators);
        }

        [HttpPost("createshop")]
        public IActionResult PostCreateShop(int userId, string shopname)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var newShop = new Shop()
            {
                Name = shopname,
                CreationDate = DateTime.Now,
                Moderators = new List<User>() {currentUser},
                OwnerId = userId
            };
            
            currentUser.Seller = true;
            currentUser.ShopsWhereModerator = new List<Shop>() { newShop };
            currentUser.ShopsOwned = new List<Shop>() { newShop };
            _context.User.Update(currentUser);

            _context.Shop.Add(newShop);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPatch("addmoderator")]
        public IActionResult PatchAddModerator(int userId, int shopId, int newModeratorId)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var currentShop = _context.Shop.FirstOrDefault(s => s.Id == shopId);
            if (currentShop == null)
                return BadRequest($"Магазин {shopId} не существует");

            var shopOwningUser = currentUser.ShopsOwned.FirstOrDefault(so => so.Id == currentShop.Id);
            if (!currentUser.Admin && shopOwningUser == null)
                return BadRequest("У вас нет прав на добавление модераторов");
            
            var newModerator = _context.User.FirstOrDefault(u => u.Id == newModeratorId);
            if (newModerator == null)
                return BadRequest($"Пользователь {newModeratorId} не существует");

            //var existingModerators = currentShop.ModeratorUsers.Any(mu => mu.Id == newModeratorId);
            var existingModerators = _context.Shop.Any(s=> 
                s.Moderators.Any(mu=> mu.Id == newModeratorId));
            if (existingModerators)
                return BadRequest($"Модератор {newModeratorId} уже добавлен к магазину");
                
            //currentShop.Moderators = new List<User>() { newModerator };

            currentShop.Moderators.Add(newModerator);
            
            _context.Shop.Update(currentShop);
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpDelete("deletemoderator")]
        public IActionResult DeleteModerator(int userId, int shopId, int moderatorId)
        {
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
            if (!ifIsOwner && !moderator.Admin)
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

            //moderator.ShopsWhereModerator.Remove(currentShop);
            shopModerator.Moderators.Remove(moderator);
            _context.SaveChanges();
            
            return Ok();
        }
    }
}