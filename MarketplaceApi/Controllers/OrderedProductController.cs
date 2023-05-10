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
        
        [HttpGet]
        public IActionResult Get(int orderId)
        {
            var orderedProduct = _context.Product
                .Where(p => p.Orders
                    .Any(o => o.Id == orderId));
            
            return Ok(orderedProduct);
        }
        
        [HttpPatch("patchquantity")]
        public IActionResult PatchQuantity(int userId, int orderId, int productId, int newQuantity)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
                return BadRequest($"Пользователь {userId} не существует");

            var areUserOrders = _context.Order
                .Any(o => o.UserId == userId);
            if (!currentUser.Admin && !areUserOrders)
                return BadRequest("У вас нет прав на редактироване данного заказа");
            
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return BadRequest($"Товар {productId} не существует");
            
            var orderedProduct = _context.OrderedProduct.FirstOrDefault(o => o.OrderId == orderId
                                                                             && o.ProductId == productId);
            if (orderedProduct == null)
                return BadRequest($"Продукта {productId} в заказе {orderId} не существует");

            if (newQuantity <= 0)
            {
                _context.OrderedProduct.Remove(orderedProduct);
                _context.SaveChanges();
                
                return Ok($"Продукт {productId} был удалён из заказа {orderId}");
            }

            if (newQuantity > product.InStockQuantity)
                return BadRequest($"В наличие {product.InStockQuantity} товаров");
            
            orderedProduct.Quantity = newQuantity;
                
            _context.OrderedProduct.Update(orderedProduct); 
            _context.SaveChanges();
            
            return Ok();

        }

        [HttpPost("addproducttoorder")]
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
            if (!product.IsPublic)
                return BadRequest($"Товар {productId} не опубликован");

            var orderedProduct = _context.OrderedProduct
                .FirstOrDefault(op => op.ProductId == productId && op.OrderId == orderId);
            if (orderedProduct == null)
            {
                if (product.InStockQuantity < quantity) 
                    return BadRequest($"В наличие {product.InStockQuantity} товаров");
                
                order.Products.Add(product);

                _context.Order.Update(order);
                _context.SaveChanges();
                
                var newOrderedProduct = _context.OrderedProduct.FirstOrDefault(o => 
                    o.OrderId == orderId && o.ProductId == productId);
                
                newOrderedProduct!.Quantity = quantity;
                _context.OrderedProduct.Update(newOrderedProduct);
                _context.SaveChanges();

                return Ok();
            }
            
            if (product.InStockQuantity < orderedProduct.Quantity + quantity) 
                return BadRequest($"В наличие {product.InStockQuantity} товаров");

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

            var order = _context.Order.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return BadRequest($"Заказ {orderId} не существует");

            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null)
                return BadRequest($"Продукт {productId} не существует");

            var ifIsOwner = _context.Order.Any(o => o.UserId == userId);
            if (!ifIsOwner && !currentUser.Admin)
                return BadRequest($"У вас не прав на редактирование заказа {orderId}");

            var orderProduct = _context.Order
                .Include(o => o.Products)
                .ThenInclude(p => p.Orders)
                .FirstOrDefault(o => o.Id == orderId && o.Products
                    .Any(p => p.Id == productId));
            if (orderProduct == null)
                return BadRequest($"Товара {productId} нет в заказе {orderId}");
            
            product.Orders.Add(order);
            _context.Product.Attach(product);

            orderProduct.Products.Remove(product);
            _context.SaveChanges();
            return Ok();
        }
    }
}