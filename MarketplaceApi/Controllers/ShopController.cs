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
        
        [HttpGet("shopowners")]
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
            
            currentUser.Seller = true;
            _context.User.Update(currentUser);

            var newShop = new Shop()
            {
                Name = shopname,
                CreationDate = DateTime.Now,
                ModeratorUsers = new List<User>() {currentUser}
            };

            _context.Shop.Add(newShop);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPatch("addowner")]
        public IActionResult PatchAddOwner(int userId, int shopId, int newOwnerId)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);

            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var currentShop = _context.Shop.FirstOrDefault(u => u.Id == shopId);

            if (currentShop == null)
                return BadRequest($"Магазин {shopId} не существует");
            
            var newOwner = _context.User.FirstOrDefault(u => u.Id == newOwnerId);

            if (newOwner == null)
                return BadRequest($"Пользователь {newOwnerId} не существует");
            
            var existingOwners = _context.User.Where(u => u == currentShop.ModeratorUsers);
            
            for (int i = 0; i < existingOwners.Count(); i++)
            {
                int existingOwnerId = existingOwners.ElementAt(i).Id;
                if (newOwner.Id == existingOwnerId)
                    return BadRequest($"Владелец {existingOwnerId} уже существует");
            }

            currentShop.ModeratorUsers = new List<User>() { newOwner };

            _context.Shop.Update(currentShop);
            _context.SaveChanges();
            
            return Ok();
        }
    }
}