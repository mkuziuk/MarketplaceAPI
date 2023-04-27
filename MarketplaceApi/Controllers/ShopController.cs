using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

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
            var shop = _context.Shop.FirstOrDefault(s => s.Id == id);

            if (shop == null) return BadRequest($"Магазин {id} не существует");

            return Ok(shop);
        }

        [HttpGet("productsfromshop")]
        public IActionResult GetProductsFromShop(int id)
        {
            var shop = _context.Shop.FirstOrDefault(s => s.Id == id);
            
            if (shop == null) return BadRequest($"Магазин {id} не существует");

            return Ok(shop.Products);
        }
        
        [HttpGet("shopowners")]
        public IActionResult GetShopOwners(int currentUserId, int id)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Admin == true);

            if (currentUser == null || currentUserId != id)
                return BadRequest("У вас нет прав на данную операцию");
            
            var shop = _context.Shop.FirstOrDefault(s => s.Id == id);
            
            if (shop == null) return BadRequest($"Магазин {id} не существует");

            return Ok(shop.ShopOwners);
        }
    }
}