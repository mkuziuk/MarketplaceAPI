using System;

namespace MarketplaceApi.ViewModels
{
    public class OrderView
    {
        public int Id { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime? SellDate { get; set; }
        public int WayOfPayment { get; set; }
        
        public string DeliveryAddress { get; set; }
        public int UserId { get; set; }
    }
}