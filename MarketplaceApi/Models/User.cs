using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string DeliveryAddress { get; set; }
        public bool Seller { get; set; }
        public bool Admin { get; set; }
        
        public ICollection<Order> Orders { get; } = new List<Order>();
        public ICollection<Bill> Bills { get; } = new List<Bill>();
        public ICollection<Product> Product { get; } = new List<Product>();

        public ICollection<Shop> ShopsOwned { get; set; } = new List<Shop>();
        public ICollection<Shop> ShopsWhereModerator { get; set; } = new List<Shop>();
        public ICollection<ShopModerator> ShopModerators { get; set; } = new List<ShopModerator>();
    }
}