using System;
using System.Collections.Generic;

namespace MarketplaceApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string OrderStatus { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
    }
}