using System;
using System.Security.Policy;

namespace MarketplaceApi
{
    public static class OrderService
    {
        public static DateTime DefaultOrderDate = DateTime.Now;
        public static DateTime DefaultReceiveDate = DefaultOrderDate.AddDays(3);
        public static string DefaultOrderStatus = "Basket";
        
    }
}