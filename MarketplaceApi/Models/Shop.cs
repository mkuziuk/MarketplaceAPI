using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class Shop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<User> Moderators { get; set; } = new List<User>();
        public ICollection<ShopModerator> ShopModerators { get; set; } = new List<ShopModerator>();
    }
}