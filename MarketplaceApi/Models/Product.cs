using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Name { get; set; }
        public string Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PublicationDate { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
        
        public int ShopId { get; set; }
        public Shop Shop { get; set; } = null!;
    }
}