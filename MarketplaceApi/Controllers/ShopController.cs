using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql.TypeHandlers;

namespace MarketplaceApi.Controllers
{
    public class ShopController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public ShopController(MarketplaceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetShop(int id)
        {
            var shop = _context.Shop.AsNoTracking().FirstOrDefault(s => s.Id == id);

            if (shop == null) return BadRequest($"Магазин {id} не существует");

            return Ok(shop);
        }

        [HttpGet("shopproducts")]
        public IActionResult GetShopProducts(int id)
        {
            var shop = _context.Shop.AsNoTracking().FirstOrDefault(s => s.Id == id);
            
            if (shop == null) return BadRequest($"Магазин {id} не существует");

            return Ok(shop.Products);
        }
        
        [HttpGet("shopmods")]
        public IActionResult GetShopOwners(int currentUserId, int id)
        {
            var shop = _context.Shop.AsNoTracking().FirstOrDefault(s => s.Id == id);
            var admittedUser = _context.User.AsNoTracking().FirstOrDefault(u => u.Admin
                || u == shop.Moderators);
            
            if (admittedUser == null)
                return BadRequest("У вас нет прав на данную операцию");
            
            if (shop == null) 
                return BadRequest($"Магазин {id} не существует");

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
            Console.WriteLine(existingModerators);
            if (existingModerators)
                return BadRequest($"Модератор {newModeratorId} уже добавлен к магазину");
                
            currentShop.Moderators = new List<User>() { newModerator };

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
            
            var currentShop = _context.Shop.FirstOrDefault(s => s.Id == shopId);
            if (currentShop == null)
                return BadRequest($"Магазин {shopId} не существует");
            
            var shopOwningUser = currentUser.ShopsOwned.FirstOrDefault(so => so.Id == currentShop.Id);
            if (!currentUser.Admin && shopOwningUser == null)
                return BadRequest("У вас нет прав на удаление модераторов");
            
            var existingModerator = _context.Shop.FirstOrDefault(s=> 
                s.Moderators.Any(mu=> mu.Id == moderatorId));
            if (existingModerator == null)
                return BadRequest($"Модератор {moderatorId} не находится в магазине");

            var ifModeratorAdmin = _context.User.Any(u =>
                u.ShopsOwned.Any(so => so.OwnerId == moderatorId));
            if (ifModeratorAdmin)
                return BadRequest("Вы не можете удалить сами себя из модераторов");
            
            var toDeleteModerator = _context.ShopModerator.FirstOrDefault(sm =>
                sm.ModeratorId == moderatorId && sm.ShopId == shopId);
            
            _context.Remove(toDeleteModerator);
            _context.SaveChanges();
            
            return Ok();
        }
    }
}