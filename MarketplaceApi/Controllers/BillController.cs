using System;
using System.Linq;
using MarketplaceApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get([FromBody] int id)
        {
            var bill = _context.Bill.FirstOrDefault(o => o.Id == id);

            if (bill != null)
            {
                return Ok(bill);
            }
            else
            {
                return BadRequest($"Чек {id} не существует");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Bill bill)
        {
            _context.Add(bill);
            _context.SaveChanges(); 
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            var bill = _context.Bill.FirstOrDefault(o => o.Id == id);
            
            if (bill != null) 
            { 
                _context.Remove(bill); 
                _context.SaveChanges(); 
                return Ok();
            }
            else
            {
                return BadRequest($"Чек {bill.Id} не найден");
            }
        }
    }
}