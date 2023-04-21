using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var product = _context.Find<Product>(Guid.Parse(id));
            return Ok(product);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            _context.Add(product);
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
                return BadRequest("Товар с таким Id не существует");
            }
        }
    }
}