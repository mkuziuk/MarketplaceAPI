using System;
using System.Linq;
using MarketplaceApi.Enums;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OrderedProductController : ControllerBase
    {
        private readonly MarketplaceContext _context;
        private readonly OrderedProductService _orderedProductService;
        
        public OrderedProductController(MarketplaceContext context)
        {
            _context = context;
            _orderedProductService = new OrderedProductService(context);
        }
        
        [HttpGet]
        public IActionResult Get(int userId, int orderId)
        {
            var result = _orderedProductService
                .GetProductsInTheOrder<dynamic>(userId, orderId);

            return DoSwitch(result);
        }
        
        [HttpPatch("patchquantity")]
        public IActionResult PatchQuantity(int userId, int orderId, int productId, int newQuantity)
        {
            var result = _orderedProductService
                .ChangeQuantity(userId, orderId, productId, newQuantity);

            return DoSwitch(result);
        }

        [HttpPost("addproducttoorder")]
        public IActionResult Post(int userId, int orderId, int productId, int quantity)
        {
            var result = _orderedProductService
                .AddProductToOrder(userId, orderId, productId, quantity);

            return DoSwitch(result);
        }

        
        [HttpDelete("productfromorder")]
        public IActionResult Delete(int userId, int orderId, int productId)
        {
            var result = _orderedProductService
                .DeleteProductFromOrder(userId, orderId, productId);

            return DoSwitch(result);
        }
    }
}