using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        public OrderController(MarketplaceContext context, IMapper mapper)
        {
            _orderService = new OrderService(context, mapper);
        }
        
        [HttpGet]
        public IActionResult Get(int userId, int orderId)
        {
            var result = _orderService.GetOrder(userId, orderId);

            return DoSwitch(result);
        }

        [HttpGet("ordersperuser")]
        public IActionResult GetOrdersPerUser(int userId)
        {
            var result = _orderService.GetUserOrders(userId);
            
            return DoSwitch(result);
        }
        
        [HttpPatch("setuporder")]
        public IActionResult PatchOrder(int userId, int orderId, int wayOfPayment, string deliveryAddress)
        {
            var result = _orderService.SetupOrder(userId, orderId, wayOfPayment, deliveryAddress);
            
            return DoSwitch(result);
        }
        
        [HttpPost]
        public IActionResult Post(int userId)
        {
            var result = _orderService.CreateOrder(userId);
            
            return DoSwitch(result);
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int orderId)
        {
            var result = _orderService.DeleteOrder(userId, orderId);
            
            return DoSwitch(result);
        }
    }
}