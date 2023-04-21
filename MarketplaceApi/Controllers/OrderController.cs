using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public OrderController(MarketplaceContext context)
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
            var user = _context.Find<Order>(Guid.Parse(id));
            return Ok(user);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            _context.Add(order);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var order = _context.Order.FirstOrDefault(o => o.Id == id);
            
            if (order != null) 
            { 
                _context.Remove(order); 
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest("Заказ с таким Id не существует");
            }
        }
    }
}