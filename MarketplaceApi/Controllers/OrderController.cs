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
        
        [HttpGet]
        public IActionResult Get(int id)
        {
            var order = _context.Order.AsNoTracking().FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return BadRequest($"Заказ {id} не существует");
            }
            
            return Ok(order);
        }

        [HttpGet("ordersperuser")]
        public IActionResult GetOrdersPerUser(int userId)
        {
            var order = _context.Order.AsNoTracking().Where(o => o.UserId == userId);

            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post(int userId)
        {
            var user = _context.User.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return BadRequest($"Пользователь {userId} не существует");

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

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var order = _context.Order.FirstOrDefault(o => o.Id == id);
            
            if (order == null) 
                return BadRequest($"Заказ {id} не найден");            

            _context.Remove(order); 
            _context.SaveChanges(); 
            return Ok();
        }
    }
}