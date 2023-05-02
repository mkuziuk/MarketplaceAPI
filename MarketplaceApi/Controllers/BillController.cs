using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketplaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : Controller
    {
        private readonly MarketplaceContext _context;
        
        public BillController(MarketplaceContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Get(int id)
        {
            var bill = _context.Bill.AsNoTracking().FirstOrDefault(o => o.Id == id);
            if (bill == null)
                return BadRequest($"Чек {id} не существует");

            return Ok(bill);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Bill bill)
        {
            _context.Add(bill);
            _context.SaveChanges(); 
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var bill = _context.Bill.FirstOrDefault(o => o.Id == id);
            
            if (bill == null) 
                return BadRequest($"Чек {id} не найден");

            _context.Remove(bill); 
            _context.SaveChanges(); 
            return Ok();
        }
    }
}