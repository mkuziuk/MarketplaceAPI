using System;
using System.Collections.Generic;

namespace MarketplaceApi
{
    public static class OrderService
    {
        public static readonly string DefaultOrderStatus = "Basket";
        public static readonly string OrderedStatus = "Ordered";
        public static List<int> ListOfWaysOfPayment = new List<int>() { 1, 2, 3 };
        
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
        
        public static DateTime OrderedSellDate()
        {
            var orderedOrderDate = DateTime.Now;

            return orderedOrderDate;
        }

        public static DateTime OrderedReceiveDate()
        {
            var orderedReceiveDate = OrderedSellDate().AddDays(3);

            return orderedReceiveDate;
        }
    }
}