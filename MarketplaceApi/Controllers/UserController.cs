using System;
using System.Linq;
using MarketplaceApi.Models;
using MarketplaceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MarketplaceContext _context;
        private readonly UserService _userService;
        
        public UserController(MarketplaceContext context)
        {
            _context = context;
            _userService = new UserService(context);
        }
        
        [HttpGet]
        public IActionResult Get(int userId)
        {
            var result = _userService.GetUser(userId);

            return DoSwitch(result);
        }

        [HttpPatch("changdata")]
        public IActionResult Patch(int userId, int id, string phone, string email, bool seller)
        {
            var result = _userService.ChangInfo(userId, id, phone, email, seller);

            return DoSwitch(result);
        }

        [HttpPatch("toadmin")]
        public IActionResult PatchToAdmin(int userId, int id)
        {
            var result = _userService.ToAdmin(userId, id);

            return DoSwitch(result);
        }
        
        [HttpPost]
        public IActionResult Post(string phone, string email, string firstName, string secondName, 
            string deliveryAddress)
        {
            var result = _userService
                .CreateUser(phone, email, firstName, secondName, deliveryAddress);

            return DoSwitch(result);
        }

        [HttpDelete]
        public IActionResult Delete(int userId, int id)
        {
            var result = _userService.DeleteUser(userId, id);

            return DoSwitch(result);
        }
    }
}