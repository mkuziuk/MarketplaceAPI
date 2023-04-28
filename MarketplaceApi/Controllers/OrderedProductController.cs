using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var orderedProduct = _context.OrderedProduct.AsNoTracking().Where(o => o.OrderId == orderId);

            return Ok(orderedProduct);
        }
        
        [HttpPatch("patchquantity")]
        public IActionResult PatchQuantity(int orderId, int productId, int newQuantity)
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
        
        [HttpPost]
        public IActionResult Post(int orderId, int productId, int quantity)
        {
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId 
                                                                             && o.ProductId == productId);
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);
            var order = _context.Order.FirstOrDefault(o => o.Id == orderId);
            
            if (orderedProduct == null && product != null && order != null)
            {
                order.Products = new List<Product>() { product };

                _context.Order.Update(order);
                _context.SaveChanges();
                
                var newOrderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId 
                    && o.ProductId == productId);

                newOrderedProduct.Quantity = quantity;
                _context.OrderedProduct.Update(newOrderedProduct);
                _context.SaveChanges();

                return Ok();
            }
            
            if (product.Quantity < orderedProduct.Quantity + quantity) 
                return BadRequest($"В наличие {product.Quantity} товаров");

            orderedProduct.Quantity += quantity;
            _context.OrderedProduct.Update(orderedProduct);
            _context.SaveChanges();
            
            return Ok();
        }
            

        
        [HttpDelete]
        public IActionResult Delete(int orderId, int productId)
        {
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => 
                o.OrderId == orderId && o.ProductId == productId);
            
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