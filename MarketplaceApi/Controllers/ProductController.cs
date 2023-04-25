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
        public IActionResult Get([FromBody] int id)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"Товар {id} не существует ");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            var user = _context.User.FirstOrDefault(o => o.Id == product.UserId);
            
            if (user != null)
            {
                product.PublicationDate = DateTime.Now;
                _context.Add(product);
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest($"Пользователь {product.UserId} не существует");
            }
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
                return BadRequest($"Продукт {id} не найден");            
            }
        }
    }
}