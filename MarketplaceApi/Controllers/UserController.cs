using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public UserController(MarketplaceContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Get([FromBody] int id)
        {
            var user = _context.User.FirstOrDefault(o => o.Id == id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest($"Пользователь {id} не существует");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            _context.Add(user);
            _context.SaveChanges(); 
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var user = _context.User.FirstOrDefault(o => o.Id == id);

            if (user != null) 
            { 
                _context.Remove(user); 
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest($"Пользователь {user.Id} не найден");
            }
        }
    }
}