using System;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserControllerAsync : Controller
    {
        private readonly UserServiceAsync _userServiceAsync;
        
        public UserControllerAsync(MarketplaceContext context, IMapper mapper)
        {
            _userServiceAsync = new UserServiceAsync(context, mapper);
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