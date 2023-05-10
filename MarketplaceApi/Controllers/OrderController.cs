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
                return BadRequest($"Заказ {id} не существует");

            return Ok(order);
        }

        [HttpGet("ordersperuser")]
        public IActionResult GetOrdersPerUser(int userId)
        {
            var order = _context.Order.AsNoTracking().Where(o => o.UserId == userId);

            return Ok(order);
        }
        
        [HttpPatch("setuporder")]
        public IActionResult PatchOrder(int userId, int orderId, int wayOfPayement, string deliveryAddress)
        {
            var order = _context.Order.FirstOrDefault(u => u.Id == orderId);
            if (order == null)
                return BadRequest($"Заказ {orderId} не существует");

            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var user = _context.User.FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));
            if (currentUser.Id != user!.Id && !currentUser.Admin)
                return BadRequest("У вас нет прав на эту операцию");
            
            user!.DeliveryAddress = deliveryAddress;
            _context.User.Update(user);

            var ifWayOfPaymentExists = OrderService.ListOfWaysOfPayment.Any(l => l == wayOfPayement);
            if (!ifWayOfPaymentExists)
                return BadRequest("Данный способ оплаты не существует");
            order.WayOfPayment = wayOfPayement;
            order.OrderStatus = OrderService.OrderedStatus;
            order.SellDate = OrderService.OrderedSellDate();
            order.ReceiveDate = OrderService.DefaultReceiveDate();

            _context.Order.Update(order);
            _context.SaveChanges();

            return Ok();
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