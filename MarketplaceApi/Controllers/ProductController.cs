using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(MarketplaceContext context, IMapper mapper)
        {
            _productService = new ProductService(context, mapper);
        }
        
        [HttpGet("getproduct")]
        public IActionResult Get(int productId)
        {
            var result = _productService.GetProduct(productId);
            
            return DoSwitch(result);
        }

        [HttpGet("getsimilarproducts")]
        public IActionResult GetSimilar(int productId, int limit)
        {
            var result = _productService.GetSimilar(productId, limit);

            return DoSwitch(result);
        }

        [HttpGet("newOfTheWeek")]
        public IActionResult GetNew(int limit)
        {
            var result = _productService.GetNewOfTheWeek(limit);

            return DoSwitch(result);
        }

        [HttpGet("listofattributes")]
        public IActionResult GetAttributes()
        {
            var result = _productService.GetAttributes();

            return DoSwitch(result);
        }

        [HttpGet("search")]
        public IActionResult GetByAttributes(string name,int? type, int? useCase, int? whereUsed,
            int? material, int? minLength, int? maxLength, int? minWidth, int? maxWidth, 
            int? minHeight, int? maxHeight, int? minPrice, int? maxPrice)
        {
            var result = _productService.Search(name, type, useCase, whereUsed,
                material, minLength, maxLength, minWidth, maxWidth, minHeight, maxHeight, minPrice, maxPrice);

            return DoSwitch(result);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] ProductEntity productEntity)
        {
            var result = _productService.AddProduct(productEntity);

            return DoSwitch(result);
        }

        [HttpPatch("changecharacteristics")]
        public IActionResult PatchProduct(int userId, int productId, int? price, int? quantity, bool isPublic)
        {
            var result = _productService
                .ChangeCharacteristics(userId, productId, price, quantity, isPublic);

            return DoSwitch(result);
        }

        [HttpPatch("changepublicstate")]
        public IActionResult PatchPublic(int userId, int productId)
        {
            var result = _productService.ChangePublicState(userId, productId);

            return DoSwitch(result);
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int productId)
        {
            var result = _productService.RemoveProduct(userId, productId);

            return DoSwitch(result);
        }
    }
}
