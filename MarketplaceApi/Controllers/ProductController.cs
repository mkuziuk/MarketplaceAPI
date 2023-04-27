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
        
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
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
        public IActionResult GetByAttributes(string name, string material, int minLength, int maxLength, 
            int minWidth, int maxWidth, int minHeight, int maxHeight, int minPrice, int maxPrice)
        {
            var resultingProducts = _context.Product.AsNoTracking().Where(p => 
                (ProductService.CheckIfDefault(name) || p.Name.StartsWith(name))
                && (ProductService.CheckIfDefault(material)|| p.Material.StartsWith(material))
                
                && (ProductService.CheckIfDefault(minLength) || p.Length >= minLength)
                && (ProductService.CheckIfDefault(maxLength) || p.Length <= maxLength)
                && (ProductService.CheckIfDefault(minWidth) || p.Width >= minWidth)
                && (ProductService.CheckIfDefault(maxWidth) || p.Width <= maxWidth)
                && (ProductService.CheckIfDefault(minHeight) || p.Height >= minHeight)
                && (ProductService.CheckIfDefault(maxHeight) || p.Height <= maxHeight)
                && (ProductService.CheckIfDefault(minPrice) || p.Price >= minPrice)
                && (ProductService.CheckIfDefault(maxPrice) || p.Price <= maxPrice)
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