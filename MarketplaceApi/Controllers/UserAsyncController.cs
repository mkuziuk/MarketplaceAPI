using System;
using System.Threading.Tasks;
using AutoMapper;
using MarketplaceApi.IServices;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using MarketplaceApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAsyncController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserAsyncController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserExp(int userId)
        {
            var task = _userService.GetUserAsync(userId);

            return await DoTryCatch(task);
        }
    }
}