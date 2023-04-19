using System;
using System.Collections.Generic;

namespace MarketplaceApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string DeliveryAdress { get; set; }
        
        public ICollection<Order> Orders { get; } = new List<Order>();
        public ICollection<Bill> Bills { get; } = new List<Bill>();
        public ICollection<Product> Products { get; } = new List<Product>();
    }
}