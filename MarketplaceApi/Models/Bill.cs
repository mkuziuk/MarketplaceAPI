using System;

namespace MarketplaceApi.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public DateTime SellDate { get; set; }
        public string WayOfPayement { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}