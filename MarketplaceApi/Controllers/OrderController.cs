using System;
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
        public IActionResult PatchOrder(int userId, int orderId, int wayOfPayment, string deliveryAddress)
        {
            var order = _context.Order.FirstOrDefault(u => u.Id == orderId);
            if (order == null)
                return BadRequest($"Заказ {orderId} не существует");

            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var user = _context.User.FirstOrDefault(u => u.Orders.Any(o => o.Id == orderId));
            if (user!.Id != userId && !currentUser.Admin)
                return BadRequest("У вас нет прав на эту операцию");
            
            var products = _context.Product
                .Where(p => p.Orders
                    .Any(o => o.Id == orderId));
            var orderedProducts = _context.OrderedProduct
                .Where(o => o.OrderId == orderId)
                .ToList();
            
            var i = 0;
            foreach (var product in products)
            {
                product.InStockQuantity -= orderedProducts[i].Quantity;

                _context.Product.Update(product);
                
                i++;
            }
            
            if (deliveryAddress != null)
            {
                user!.DeliveryAddress = deliveryAddress;
                _context.User.Update(user);
            }

            var ifWayOfPaymentExists = OrderService.ListOfWaysOfPayment.Any(l => l == wayOfPayment);
            if (!ifWayOfPaymentExists)
                return BadRequest("Данный способ оплаты не существует");
            order.WayOfPayment = wayOfPayment;
            order.OrderStatus = OrderService.OrderedStatus;
            order.SellDate = OrderService.OrderedSellDate();
            order.DeliveryDate = OrderService.OrderedReceiveDate();
            order.DeliveryAddress = user.DeliveryAddress;

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
                OrderDate = OrderService.OrderedOrderDate(),
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