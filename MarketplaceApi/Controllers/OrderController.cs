using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MarketplaceContext _context;
        private readonly OrderService _orderService;

        public OrderController(MarketplaceContext context)
        {
            _context = context;
            _orderService = new OrderService(context);
        }
        
        [HttpGet]
        public IActionResult Get(int userId, int orderId)
        {
            var result = _orderService.GetOrder(userId, orderId);

            DoSwitch(result);

            return Ok();
        }

        [HttpGet("ordersperuser")]
        public IActionResult GetOrdersPerUser(int userId)
        {
            var result = _orderService.GetUserOrders(userId);
            DoSwitch(result, $"Пользователь {userId} не существует");

            return Ok("Операция завершена");
        }
        
        [HttpPatch("setuporder")]
        public IActionResult PatchOrder(int userId, int orderId, int wayOfPayment, string deliveryAddress)
        {
            var result = _orderService.SetupOrder(userId, orderId, wayOfPayment, deliveryAddress);
            DoSwitch(result);
            return Ok("Операция завершена");
        }
        
        [HttpPost]
        public IActionResult Post(int userId)
        {
            var result = _orderService.CreateOrder(userId);
            DoSwitch(result);
            return Ok("Операция завершена");
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int orderId)
        {
            var result = _orderService.DeleteOrder(userId, orderId);
            DoSwitch(result);
            return Ok("Операция завершена");
        }
    }
}