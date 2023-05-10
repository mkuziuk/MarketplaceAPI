using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? SellDate { get; set; }
        public int WayOfPayment { get; set; }
        
        public string DeliveryAddress { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
    }
}