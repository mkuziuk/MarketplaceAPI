using System;

namespace MarketplaceApi
{
    public static class OrderService
    {
        public static readonly string DefaultOrderStatus = "Basket";
        
        public static DateTime DefaultOrderDate()
        {
            var defaultOrderDate = DateTime.Now;

            return defaultOrderDate;
        }
        
        public static DateTime DefaultReceiveDate()
        {
            var defaultReceiveDate = DefaultOrderDate().AddDays(3);

            return defaultReceiveDate;
        }
    }
}