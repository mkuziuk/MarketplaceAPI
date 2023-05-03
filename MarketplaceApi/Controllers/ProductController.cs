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
        public IActionResult Get(int productId)
        {
            var product = _context.Product.AsNoTracking().FirstOrDefault(o => o.Id == productId);
            if (product == null)
                return BadRequest($"Товар {productId} не существует");
            
            return Ok(product);
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
            int height, int price, int quantity, bool isPublic)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");

            var shop = _context.Shop.FirstOrDefault(s => s.Id == shopId);
            if (shop == null)
                return BadRequest($"Магазин {shopId} не существует");

            var areModeratorsShop = _context.Shop.FirstOrDefault(s => s.Moderators
                .FirstOrDefault(m => m.Id == userId) == currentUser);
            if (!currentUser.Admin && areModeratorsShop == null)
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
                CreationDate = DateTime.Now,
                ShopId = shopId,
                IsPublic = isPublic
            };

            if (isPublic) product.PublicationDate = DateTime.Now;

            _context.Add(product);
            _context.SaveChanges(); 
            return Ok();
        }

        [HttpPatch("changecharacteristics")]
        public IActionResult PatchProduct(int userId, int productId, int? price, int? quantity, bool isPublic)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null) 
                return BadRequest($"Продукт {productId} не найден");
            
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) 
                return BadRequest($"Пользователь {userId} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.Moderators.Any(mu => mu.Id == userId));
            if (!currentUser.Admin && moderator == null)
                return BadRequest("У вас нет прав на редактирование товара");

            if (price != null) product.Price = price.Value;
            if (quantity != null) product.Quantity = quantity.Value;
            if (product.IsPublic != isPublic) product.IsPublic = isPublic;

            _context.Update(product);
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpPatch("changepublic")]
        public IActionResult PatchPublic(int userId, int productId, bool isPublic)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null) 
                return BadRequest($"Продукт {productId} не найден");
            
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) 
                return BadRequest($"Пользователь {userId} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.Moderators.Any(mu => mu.Id == userId));
            if (!currentUser.Admin && moderator == null)
                return BadRequest("У вас нет прав на редактирование товара");

            if (product.IsPublic == isPublic)
                return BadRequest("Товар уже находится в этом статусе");
            
            product.IsPublic = isPublic;
            if (isPublic) product.PublicationDate = DateTime.Now;
            else product.PublicationDate = null;

            _context.Update(product);
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int productId)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null) 
                return BadRequest($"Продукт {productId} не найден");
            
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) 
                return BadRequest($"Пользователь {userId} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.Moderators.Any(mu => mu.Id == userId));
            if (!currentUser.Admin && moderator == null)
                return BadRequest("У вас нет прав на удаление товара");
            
            _context.Remove(product); 
            _context.SaveChanges(); 
            return Ok();
        }
    }
}