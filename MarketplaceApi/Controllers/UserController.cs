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
            var student = _context.Find<User>(Guid.Parse(id));
            return Ok(student);
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
            IQueryable<User> user = _context.User.Where(o => o.Id == id);

            if (user.Count() != 0)
            {
                int userId = user.First().Id;
                
                if (id == userId)
                {
                    _context.Remove(user.First());
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Ok();
                }
            }
            else
            {
                return Ok();
            }
        }
    }
}