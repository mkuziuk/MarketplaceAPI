using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderedProductController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public OrderedProductController(MarketplaceContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = _context.Find<User>(Guid.Parse(id));
            return Ok(user);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] OrderedProduct orderedProduct)
        {
            _context.Add(orderedProduct); 
            _context.SaveChanges();
            
            return Ok();
        }
        
        [HttpDelete]
        public IActionResult Delete([FromBody] int orderId, int productId)
        {
            var orderedProduct = _context.OrderedProduct
                .FirstOrDefault(o => o.OrderId == orderId && o.ProductId == productId);

            if (orderedProduct != null) 
            { 
                _context.Remove(orderedProduct); 
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest($"Продукт {productId} в заказе {orderId} не найден");
            }
        }
    }
}