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
        public IActionResult Get([FromBody] int orderId)
        {
            var orderedProduct = _context.OrderedProduct.Where(o => o.OrderId == orderId);

            if (orderedProduct != null)
            {
                return Ok(orderedProduct);
            }
            
            return NotFound();
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = _context.Find<User>(Guid.Parse(id));
            return Ok(user);
        }

        [HttpPatch]
        public IActionResult Patch([FromBody] OrderedProduct orderedProduct)
        {
            _context.OrderedProduct.Update(orderedProduct);
            _context.SaveChanges();
            
            return Ok();
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] OrderedProduct orderedProduct)
        {
            _context.Add(orderedProduct); 
            _context.SaveChanges();
            
            return Ok();
        }
        
        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == id);
            
            if (product != null) 
            { 
                _context.Remove(product); 
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest($"Продукт {product.Id} не найден");            
            }
        }
    }
}