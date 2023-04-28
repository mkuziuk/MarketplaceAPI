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
                || u == shop.ModeratorUsers);
            
            if (admittedUser == null)
                return BadRequest("У вас нет прав на данную операцию");
            
            if (shop == null) 
                return BadRequest($"Магазин {id} не существует");

            return Ok(shop.ModeratorUsers);
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
                ModeratorUsers = new List<User>() {currentUser},
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
        public IActionResult PatchAddOwner(int userId, int shopId, int newModeratorId)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var currentShop = _context.Shop.FirstOrDefault(s => s.Id == shopId);
            if (currentShop == null)
                return BadRequest($"Магазин {shopId} не существует");

            var shopOwningUser = currentUser.ShopsOwned.FirstOrDefault(s => s.Id == currentShop.Id);
            if (shopOwningUser == null)
                return BadRequest("У вас не прав на добавление модераторов");
            
            var newModerator = _context.User.FirstOrDefault(u => u.Id == newModeratorId);
            if (newModerator == null)
                return BadRequest($"Пользователь {newModeratorId} не существует");
            
            var existingModerators = _context.User.Where(u => u
                                                              == currentShop.ModeratorUsers);
            for (var i = 0; i < existingModerators.Count(); i++)
            {
                var existingModeratorId = existingModerators.ElementAt(i).Id;
                if (newModeratorId == existingModeratorId)
                    return BadRequest($"Модератор {existingModeratorId} уже существует");
            }

            currentShop.ModeratorUsers = new List<User>() { newModerator };

            _context.Shop.Update(currentShop);
            _context.SaveChanges();
            
            return Ok();
        }
    }
}