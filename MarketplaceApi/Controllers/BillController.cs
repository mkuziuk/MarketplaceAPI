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
        public IActionResult Get()
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var bill = _context.Find<Order>(Guid.Parse(id));
            return Ok(bill);
        }
        
        [HttpPost]
        public IActionResult Post([FromBody] Bill bill)
        {
            var existingBill = _context.User.FirstOrDefault(o => o.Id == bill.Id);
            
            if (existingBill == null)
            {
                _context.Add(bill);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest($"Чек {bill.Id} уже существует"); 
            }
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
                return BadRequest($"Чек {bill.Id} не найден");            }
        }
    }
}