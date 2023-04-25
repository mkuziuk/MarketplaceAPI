using System;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
        public IActionResult Get([FromBody] int id)
        {
            var order = _context.Order.FirstOrDefault(o => o.Id == id);

            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return BadRequest($"Заказ {id} не существует");
            }
        }

        [HttpGet("orderperuser")]
        public IActionResult GetOrderPerUser([FromBody] int userId)
        {
            var order = _context.Order.Where(o => o.UserId == userId);
            var user = _context.User.FirstOrDefault(o => o.Id == userId);
            
            if (user == null)
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
                    ReceiveDate = OrderService.DefaultreceiveDate(),
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
                return BadRequest($"Заказ {order.Id} не найден");            
            }
        }
    }
}