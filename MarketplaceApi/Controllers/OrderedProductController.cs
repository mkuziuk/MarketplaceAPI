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
        public IActionResult PatchQuantity(int userId, int orderId, int productId, int newQuantity)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");
            
            var areUserOrders = currentUser.Orders.Any(o => o.UserId == userId);
            if (!currentUser.Admin && !areUserOrders)
                return BadRequest("У вас нет прав на редактироване данного заказа");
            
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
        public IActionResult Post(int userId, int orderId, int productId, int quantity)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");

            var order = _context.Order.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return BadRequest($"Заказ {orderId} не существует");
            
            var userOrder = currentUser.Orders.FirstOrDefault(o=> o.Id == orderId);
            if (userOrder == null && !currentUser.Admin)
                return BadRequest("У вас нет прав на редактироване данного заказа");
            
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return BadRequest($"Товар {productId} не существует");

            var orderedProduct = order.OrderedProducts.FirstOrDefault(op =>
                op.ProductId == productId);
            //var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => 
                //o.OrderId == orderId && o.ProductId == productId);
            if (orderedProduct == null)
            {
                order.Products = new List<Product>() { product };

                _context.Order.Update(order);
                _context.SaveChanges();
                
                var newOrderedProduct = _context.OrderedProduct.FirstOrDefault(o => 
                    o.OrderId == orderId && o.ProductId == productId);

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
            

        
        [HttpDelete("productfromorder")]
        public IActionResult Delete(int userId, int orderId, int productId)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");

            var order = new Order() { Id = orderId };
            var product = new Product() { Id = productId };
            
            var ifOrderExists = _context.Order.Any(o => o.Id == orderId);
            if (!ifOrderExists)
                return BadRequest($"Заказ {orderId} не существует");

            var ifIsOwner = _context.Order.Any(o => o.UserId == userId);
            if (!ifIsOwner && !currentUser.Admin)
                return BadRequest($"У вас не прав на редактирование заказа {orderId}");
            
            order.Products.Add(product);
            _context.Attach(order);

            order.Products.Remove(product);
            _context.SaveChanges(); 
            return Ok();
        }
    }
}