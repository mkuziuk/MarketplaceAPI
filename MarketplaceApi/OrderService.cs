using System;

namespace MarketplaceApi
{
    public static class OrderService
    {
        public static readonly string DefaultOrderStatus = "Basket";
        
        public static DateTime? DefaultOrderDate()
        {
            //var defaultOrderDate = DateTime.Now;

            return null;
        }
        
        public static DateTime? DefaultReceiveDate()
        {
            // var defaultReceiveDate = DefaultOrderDate().AddDays(3);

            return null;
        }

        public static DateTime OrderedOrderDate()
        {
            var orderedOrderDate = DateTime.Now;

            return orderedOrderDate;
        }

        public static DateTime OrderedReceiveDate()
        {
            var orderedReceiveDate = OrderedOrderDate().AddDays(3);

            return orderedReceiveDate;
        }
    }
}