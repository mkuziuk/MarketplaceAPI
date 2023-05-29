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
        public int OrderStatusId { get; set; }
        public DateTime? SellDate { get; set; }
        public int WayOfPayment { get; set; }
        
        public string DeliveryAddress { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
    }
}