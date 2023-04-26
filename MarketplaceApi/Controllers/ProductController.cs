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
        
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == id);

            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest($"Товар {id} не существует");
            }
        }

        [HttpGet("search/{name}/{material}/{minLength}/{maxLength}/{minWidth}/{maxWidth}/{minHeight}/{maxHeight}/{minPrice}/{maxPrice}")]
        public IActionResult GetByAttributes([FromRoute] string name = " ", string material = " ", 
            int minLength = 0, int maxLength = 0, int minWidth = 0, int maxWidth = 0, int minHeight = 0, int maxHeight = 0, 
            int minPrice = 0, int maxPrice = 0)
        {
            var resultingProducts = _context.Product.Where(p => 
                (name == null || p.Name.StartsWith(name))
                && (material == null || p.Material.StartsWith(material))
                
                && (minLength == 0 || p.Length >= minLength)
                && (maxLength == 0 || p.Length <= maxLength)
                && (minWidth == 0 || p.Width >= minWidth)
                && (maxWidth == 0 || p.Width <= maxWidth)
                && (minHeight == 0 || p.Height >= minHeight)
                && (maxHeight == 0 || p.Height <= maxHeight)
                && (minPrice == 0 || p.Price >= minPrice)
                && (maxPrice == 0 || p.Price <= maxPrice)
            );
            
            return Ok(resultingProducts);
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