using System;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAsyncController : Controller
    {
        private readonly IUserServiceAsync _userServiceAsync;
        
        public UserAsyncController(IUserServiceAsync userServiceAsync)
        {
            _userServiceAsync = userServiceAsync;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _userServiceAsync.GetUserAsync(userId);
                return Ok(user);
            }
            catch (ArgumentNullException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return NotFound($"{e.GetType()} says {e.Message}");
            }
        }
    }
}