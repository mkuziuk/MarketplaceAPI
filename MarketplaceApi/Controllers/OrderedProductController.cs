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
        
        [HttpGet]
        public IActionResult Get([FromBody] int orderId)
        {
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId);
            var product = _context.Product.FirstOrDefault(o => o.Id == orderedProduct.ProductId);

            if (orderedProduct != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();   
            }
        }

        [HttpPatch("{orderId}/{productId}/{newQuantity}")]
        public IActionResult Patch([FromRoute]int orderId, int productId, int newQuantity)
        {
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId
                                                                             && o.ProductId == productId);
            
            if (newQuantity > 0)
            {
                if (orderedProduct != null)
                {
                    orderedProduct.Quantity = newQuantity;
                
                    _context.OrderedProduct.Update(orderedProduct);
                    _context.SaveChanges();
                
                    return Ok();
                }
                else
                {
                    return BadRequest($"Продукта {productId} из заказа {orderId} не существует");
                }
            }
            else
            {
                _context.OrderedProduct.Remove(orderedProduct);
                _context.SaveChanges();
                
                return Ok($"Продукт {productId} был удалён из заказа {orderId}");
            }
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] OrderedProduct orderedProduct)
        {
            var existingOrderedProduct = _context.OrderedProduct.FirstOrDefault(o =>
                o.OrderId == orderedProduct.OrderId && o.ProductId == orderedProduct.ProductId);

            if (existingOrderedProduct == null)
            {
                _context.Add(orderedProduct); 
                _context.SaveChanges();
            
                return Ok();
            }
            else
            {
                existingOrderedProduct.Quantity += orderedProduct.Quantity;

                _context.Update(existingOrderedProduct);
                _context.SaveChanges();

                return Ok();
            }
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