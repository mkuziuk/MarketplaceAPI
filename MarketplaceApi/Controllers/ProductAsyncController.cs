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
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class ProductAsyncController : Controller
    {
        private readonly IProductService _productService;

        public ProductAsyncController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> PostProduct(ProductEntity productEntity)
        {
            try
            {
                await _productService.AddProductAsync(productEntity);
                return Ok("товар успешно добавлен");
            }
            catch (EntityNullException e)
            {
                return NotFound(e.Message);
            }
            catch (AccessException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest($"{e.GetType()} says {e.Message}");
            }
        }
    }
}