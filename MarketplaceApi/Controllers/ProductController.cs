using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly MarketplaceContext _context;

        public ProductController(MarketplaceContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Get(int id)
        {
            var product = _context.Product.AsNoTracking().FirstOrDefault(o => o.Id == id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"Товар {id} не существует");
            }
        }

        [HttpGet("search")]
        public IActionResult GetByAttributes(string name, string material, int? minLength, int? maxLength, 
            int? minWidth, int? maxWidth, int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            var resultingProducts = _context.Product.AsNoTracking().Where(p => 
                (name == null || p.Name.StartsWith(name))
                && (material == null || p.Material.StartsWith(material))
                
                && (minLength == null || p.Length >= minLength)
                && (maxLength == null || p.Length <= maxLength)
                && (minWidth == null || p.Width >= minWidth)
                && (maxWidth == null || p.Width <= maxWidth)
                && (minHeight == null || p.Height >= minHeight)
                && (maxHeight == null || p.Height <= maxHeight)
                && (minPrice == null || p.Price >= minPrice)
                && (maxPrice == null || p.Price <= maxPrice)
            );
            
            return Ok(resultingProducts);
        }
        
        [HttpPost]
        public IActionResult Post(int userId, int shopId, string name, string material, int length, int width, 
            int height, int price, int quantity)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return BadRequest($"Пользователь {userId} не существует");

            var shop = _context.Shop.FirstOrDefault(s => s.Id == shopId);
            if (shop == null)
                return BadRequest($"Магазин {shopId} не существует");

            var ifModerator = _context.Shop.FirstOrDefault(s => s.ModeratorUsers
                .FirstOrDefault(m => m.Id == userId) == user);
            if (ifModerator == null)
                return BadRequest("У вас нет прав на добавление товаров в это магазин");
            
            var product = new Product()
            {
                UserId = userId,
                Name = name,
                Material = material,
                Length = length,
                Width = width,
                Height = height,
                Price = price,
                Quantity = quantity,
                PublicationDate = DateTime.Now,
                ShopId = shopId
            };
            
            _context.Add(product);
            _context.SaveChanges(); 
            return Ok();
            
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int id)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == id);
            if (product == null) 
                return BadRequest($"Продукт {id} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.ModeratorUsers.Any(mu => mu.Id == userId));
            if (moderator == null)
                return BadRequest("У вас нет прав на данную операцию");
            
            _context.Remove(product); 
            _context.SaveChanges(); 
            return Ok();
        }
    }
}