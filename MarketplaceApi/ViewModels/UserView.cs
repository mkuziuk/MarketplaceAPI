using System;

namespace MarketplaceApi.ViewModels
{
    public class UserView
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string DeliveryAddress { get; set; }
        public bool Seller { get; set; }
    }
}