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
        public int Type { get; set; } // statue, 
        public int UseCase { get; set; } // game, watch
        public int WhereUsed{ get; set; } // in door, out door 
        public int Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public int InStockQuantity { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? PublicationDate { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
        
        public int ShopId { get; set; }
        public Shop Shop { get; set; } = null!;
    }
}