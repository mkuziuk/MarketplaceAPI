using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var user = _context.User.AsNoTracking().FirstOrDefault(o => o.Id == id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest($"Пользователь {id} не существует");
            }
        }

        [HttpPatch]
        public IActionResult Patch(int currentUserId, int id, string phone, string email, bool seller)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Admin == true);

            if (currentUser == null || currentUserId != id) 
                return BadRequest("У вас нет прав на данную операцию");

            var user = _context.User.FirstOrDefault(u => u.Id == id);

            if (user == null) 
                return BadRequest($"Пользователь {id} не существует");

            user.Phone = phone;
            user.Email = email;
            user.Seller = seller;

            _context.User.Update(user);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPatch("toadmin")]
        public IActionResult PatchToAdmin(int currentUserId, int id)
        {
            var currentUser = _context.User.FirstOrDefault(u => u.Admin == true);
            
            if (currentUser == null) 
                return BadRequest("У вас нет прав на данную операцию");
            
            var user = _context.User.FirstOrDefault(u => u.Id == id);

            if (user == null) 
                return BadRequest($"Пользователь {id} не существует");

            user.Admin = true;

            _context.User.Update(user);
            _context.SaveChanges();

            return Ok();
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            user.Admin = false;
            user.Seller = false;
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