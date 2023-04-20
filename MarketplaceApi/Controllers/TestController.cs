using System;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public TestController(MarketplaceContext context)
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
    }
}