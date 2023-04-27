using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var order = _context.Order.AsNoTracking().FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return BadRequest($"Заказ {id} не существует");
            }
            
            return Ok(order);
        }

        [HttpGet("ordersperuser/{userId}")]
        public IActionResult GetOrdersPerUser([FromRoute] int userId)
        {
            var order = _context.Order.AsNoTracking().Where(o => o.UserId == userId);
            
            if (order == null)
            {
                return BadRequest($"Пользователь {userId} не существует");
            }
            
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post([FromBody]int userId)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                var order = new Order()
                {
                    OrderDate = OrderService.DefaultOrderDate(),
                    ReceiveDate = OrderService.DefaultReceiveDate(),
                    OrderStatus = OrderService.DefaultOrderStatus,
                    UserId = userId
                };
                
                _context.Order.Add(order); 
                _context.SaveChanges(); 
                return Ok();
            }
            else 
            { 
                return BadRequest($"Пользователь {userId} не существует");
            }
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
                return BadRequest($"Заказ {id} не найден");            
            }
        }
    }
}