using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ShopService _shopService;
        public ShopController(MarketplaceContext context, IMapper mapper)
        {
            _shopService = new ShopService(context, mapper);
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
        }
    }
}