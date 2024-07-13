using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.Exceptions;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAsyncController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductAsyncController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpPost("addproduct")]
        public async Task<IActionResult> PostProductExp(ProductEntity productEntity)
        {
            var task = _productService.AddProductAsync(productEntity);

            return await DoTryCatch(task);
        }
    }
}