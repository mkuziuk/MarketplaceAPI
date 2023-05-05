using System;
using System.Collections.Generic;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly MarketplaceContext _context;

        public ProductController(MarketplaceContext context)
        {
            _context = context;
        }
        
        [HttpGet("getproduct")]
        public IActionResult Get(int productId)
        {
            var product = _context.Product.AsNoTracking().FirstOrDefault(o => o.Id == productId);
            if (product == null)
                return BadRequest($"Товар {productId} не существует");
            
            return Ok(product);
        }

        [HttpGet("getsimilarproducts")]
        public IActionResult GetSimilar(int productId)
        {
            var product = _context.Product.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return BadRequest($"Товар {productId} не существует");

            double priceFluctuation = 0.1;
            double sizeFluctuation = 0.1;
            double volumeFluctuation = 0.1;
            var similarProducts = _context.Product
                .Where(p => 
                            p.Id != product.Id &&
                            (
                                p.IsPublic && 
                                (
                                    p.Name == product.Name ||
                                    p.Material == product.Material ||
                                    p.Type == product.Type ||
                                    p.UseCase == product.UseCase ||
                                    p.WhereCanBeUsed == product.WhereCanBeUsed ||
                                    p.ShopId == product.ShopId ||
                                    p.Price >= product.Price * (1 - priceFluctuation) ||
                                    p.Price <= product.Price * (1 + priceFluctuation) && 
                                    (
                                        p.Length >= product.Length * (1 - sizeFluctuation) ||
                                        p.Length <= product.Length * (1 + sizeFluctuation) ||
                                        p.Width >= product.Width * (1 - sizeFluctuation) ||
                                        p.Width <= product.Width * (1 + sizeFluctuation) ||
                                        p.Height >= product.Height * (1 - sizeFluctuation) ||
                                        p.Height <= product.Height * (1 + sizeFluctuation)
                                    ) && 
                                    (
                                        p.Length * p.Width * p.Height >= 
                                        product.Length * product.Width * product.Height * (1 - volumeFluctuation) ||
                                        p.Length * p.Width * p.Height >= 
                                        product.Length * product.Width * product.Height * (1 + volumeFluctuation)
                                    )
                                )    
                            )    
                );
            
            return Ok(similarProducts);
        }

        [HttpGet("new")]
        public IActionResult GetNew(int limit)
        {
            var newProducts = _context.Product
                .Where(p => p.IsPublic)
                .OrderByDescending(p => p.PublicationDate)
                .Take(limit);

            return Ok(newProducts);
        }

        [HttpGet("listofattributs")]
        public IActionResult GetAttributs()
        {
            
            return Ok();
        }

        [HttpGet("search")]
        public IActionResult GetByAttributes(string name,int? type, int? useCase, int? whereCanBeUsed,
            int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth, 
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            var resultingProducts = _context.Product.AsNoTracking().Where(p => 
                (name == null || p.Name.StartsWith(name))
                && (type == null || p.Type == type)
                && (useCase == null || p.UseCase == useCase)
                && (whereCanBeUsed == null || p.WhereCanBeUsed == whereCanBeUsed)
                && (material == null || p.Material == material)
                
                && (minLength == null || p.Length >= minLength)
                && (maxLength == null || p.Length <= maxLength)
                && (minWidth == null || p.Width >= minWidth)
                && (maxWidth == null || p.Width <= maxWidth)
                && (minHeight == null || p.Height >= minHeight)
                && (maxHeight == null || p.Height <= maxHeight)
                && (minPrice == null || p.Price >= minPrice)
                && (maxPrice == null || p.Price <= maxPrice)
            );
            
            return Ok(resultingProducts);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] ProductEntity productEntity)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Id == productEntity.UserId);
            if (currentUser == null)
                return BadRequest($"Пользователь {productEntity.UserId} не существует");

            var shop = _context.Shop.FirstOrDefault(s => s.Id == productEntity.ShopId);
            if (shop == null)
                return BadRequest($"Магазин {productEntity.ShopId} не существует");

            var areModeratorsShop = _context.Shop.FirstOrDefault(s => s.Moderators
                .FirstOrDefault(m => m.Id == productEntity.UserId) == currentUser);
            if (!currentUser.Admin && areModeratorsShop == null)
                return BadRequest("У вас нет прав на добавление товаров в это магазин");

            var ifTypeExists = ProductEntity.ListOfTypes.Any(lt => lt.Equals(productEntity.Type));
            if (!ifTypeExists)
                return BadRequest($"Тип {productEntity.Type} не существует");

            var ifUseCaseExists = ProductEntity.ListOfUseCases.Any(uc => uc.Equals(productEntity.UseCase));
            if(!ifUseCaseExists)
                    return BadRequest($"Способ применения {productEntity.UseCase} не существует");
            
            var ifWhereCanBeUsedExists = ProductEntity.ListOfWhereCanBeUsed
                .Any(wc => wc.Equals(productEntity.WhereCanBeUsed));
            if (!ifWhereCanBeUsedExists)
                    return BadRequest($"Место применения {productEntity.WhereCanBeUsed} не существует");
            
            var ifMaterialExists = ProductEntity.ListOfMaterials.Any(lm => lm.Equals(productEntity.Material));
            if (!ifMaterialExists)
                    return BadRequest($"Материал {productEntity.Material} не существует");

            var product = new Product()
            {
                UserId = productEntity.UserId,
                Name = productEntity.Name,
                Type = productEntity.Type,
                UseCase = productEntity.UseCase,
                WhereCanBeUsed = productEntity.WhereCanBeUsed,
                Material = productEntity.Material,
                Length = productEntity.Length,
                Width = productEntity.Width,
                Height = productEntity.Height,
                Price = productEntity.Price,
                InStockQuantity = productEntity.Quantity,
                CreationDate = DateTime.Now,
                ShopId = productEntity.ShopId,
                IsPublic = productEntity.IsPublic
            };

            if (productEntity.IsPublic) product.PublicationDate = DateTime.Now;

            _context.Add(product);
            _context.SaveChanges(); 
            return Ok();
        }

        [HttpPatch("changecharacteristics")]
        public IActionResult PatchProduct(int userId, int productId, int? price, int? quantity, bool isPublic)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null) 
                return BadRequest($"Продукт {productId} не найден");
            
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) 
                return BadRequest($"Пользователь {userId} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.Moderators.Any(mu => mu.Id == userId));
            if (!currentUser.Admin && moderator == null)
                return BadRequest("У вас нет прав на редактирование товара");

            if (price != null) product.Price = price.Value;
            if (quantity != null) product.InStockQuantity = quantity.Value;
            if (product.IsPublic != isPublic) product.IsPublic = isPublic;

            _context.Update(product);
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpPatch("changepublic")]
        public IActionResult PatchPublic(int userId, int productId, bool isPublic)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null) 
                return BadRequest($"Продукт {productId} не найден");
            
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) 
                return BadRequest($"Пользователь {userId} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.Moderators.Any(mu => mu.Id == userId));
            if (!currentUser.Admin && moderator == null)
                return BadRequest("У вас нет прав на редактирование товара");

            if (product.IsPublic == isPublic)
                return BadRequest("Товар уже находится в этом статусе");
            
            product.IsPublic = isPublic;
            if (isPublic) product.PublicationDate = DateTime.Now;
            else product.PublicationDate = null;

            _context.Update(product);
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int productId)
        {
            var product = _context.Product.FirstOrDefault(o => o.Id == productId);
            if (product == null) 
                return BadRequest($"Продукт {productId} не найден");
            
            var currentUser = _context.User.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null) 
                return BadRequest($"Пользователь {userId} не найден");
            
            var moderator = _context.Shop.FirstOrDefault(s => s.Moderators.Any(mu => mu.Id == userId));
            if (!currentUser.Admin && moderator == null)
                return BadRequest("У вас нет прав на удаление товара");
            
            _context.Remove(product); 
            _context.SaveChanges(); 
            return Ok();
        }
    }
}