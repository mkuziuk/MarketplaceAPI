using System;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MarketplaceApi
{
    public static class OrderService
    {
        public static string DefaultOrderStatus = "Basket";
        
        public static DateTime DefaultOrderDate()
        {
            DateTime defaultOrderDate = DateTime.Now;

            return defaultOrderDate;
        }
        
        public static DateTime DefaultreceiveDate()
        {
            DateTime defaultReceiveDate = DefaultOrderDate().AddDays(3);

            return defaultReceiveDate;
        }
    }
}