using System;
using System.ComponentModel.DataAnnotations.Schema;
using Castle.Components.DictionaryAdapter;
using MarketplaceApi.Models;

namespace MarketplaceApi.Views
{
    public class ProductView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } // statue, moving
        public int UseCase { get; set; } // game, watch
        public int WhereUsed { get; set; } // in door, out door 
        public int Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public int InStockQuantity { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? PublicationDate { get; set; }
    }
}