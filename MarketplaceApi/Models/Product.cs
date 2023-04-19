using System;
using System.Collections.Generic;

namespace MarketplaceApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        //public int ProductCode { get; set; }
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
        
        public ICollection<OrderedProduct> OrderedProducts { get; } = new List<OrderedProduct>();
    }
}