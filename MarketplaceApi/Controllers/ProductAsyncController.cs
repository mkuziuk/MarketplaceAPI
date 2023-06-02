using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IProductServiceAsync _productServiceAsync;

        public ProductAsyncController(IProductServiceAsync productServiceAsync)
        {
            _productServiceAsync = productServiceAsync;
        }

        [HttpPost("addproduct")]
        public async Task<IActionResult> PostProduct(ProductEntity productEntity)
        {
            try
            {
                await _productServiceAsync.AddProductAsync(productEntity);
                return Ok("товар успешно добавлен");
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return NotFound($"{e.GetType()} says {e.Message}");
            }
        }
    }
}