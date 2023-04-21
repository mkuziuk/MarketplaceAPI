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
        public IActionResult Get()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var user = _context.Find<User>(Guid.Parse(id));
            return Ok(user);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            var existingUsers = _context.User.FirstOrDefault(o => o.Id == user.Id);
            
            if (existingUsers == null)
            {
                _context.Add(user);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest($"Пользователь {user.Id} уже существует"); 
            }
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
                //return BadRequest(String.Format("Пользователь {0} не существует", id));
                return BadRequest($"Пользователь {user.Id} не найден");
            }
        }
    }
}