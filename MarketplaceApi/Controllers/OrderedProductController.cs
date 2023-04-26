using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class OrderedProductController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public OrderedProductController(MarketplaceContext context)
        {
            _context = context;
        }
        
        [HttpGet("{orderId}")]
        public IActionResult Get([FromRoute] int orderId)
        {
            var orderedProduct = _context.OrderedProduct.Where(o => o.OrderId == orderId);

            if (orderedProduct == null)
            {
                return BadRequest($"Заказ {orderId} не существует");
            }

            return Ok(orderedProduct);
        }
        
        [HttpPatch("{orderId}/{productId}/{newQuantity}")]
        public IActionResult Patch([FromRoute]int orderId, int productId, int newQuantity)
        {
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId
                                                                             && o.ProductId == productId);
            
            if (orderedProduct == null)
            {
                return BadRequest($"Продукта {productId} из заказа {orderId} не существует");
            }

            if (newQuantity <= 0)
            {
                _context.OrderedProduct.Remove(orderedProduct);
                _context.SaveChanges();
                
                return Ok($"Продукт {productId} был удалён из заказа {orderId}");
            }
            
            orderedProduct.Quantity = newQuantity;
                
            _context.OrderedProduct.Update(orderedProduct); 
            _context.SaveChanges();
            
            return Ok();
        }
        
        [HttpPost("{orderId}/{productId}/{quantity}")]
        public IActionResult Post([FromRoute]int orderId, int productId, int quantity)
        {
            var existingOrderedProduct = _context.OrderedProduct.FirstOrDefault(o =>
                o.OrderId == orderId && o.ProductId == productId);

            if (existingOrderedProduct == null)
            {
                var orderedProduct = new OrderedProduct()
                {
                    OrderId = orderId,
                    ProductId = productId,
                    Quantity = quantity
                };
                
                _context.Add(orderedProduct); 
                _context.SaveChanges();
            
                return Ok();
            }
            
            existingOrderedProduct.Quantity += quantity;

            _context.Update(existingOrderedProduct);
            _context.SaveChanges();

            return Ok();
        }
            

        
        [HttpDelete("{orderId}/{productId}")]
        public IActionResult Delete([FromRoute] int orderId, int productId)
        {
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId
                                                                             && o.ProductId == productId);
            
            if (orderedProduct != null) 
            { 
                _context.Remove(orderedProduct); 
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest($"Продукт {productId} в заказе {orderId} не найден");            
            }
        }
        
    }
}