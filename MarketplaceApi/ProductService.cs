using System.Collections.Generic;
using MarketplaceApi.Models;

namespace MarketplaceApi
{
    public class ProductService
    {
        public static bool CheckIfDefault(int value)
        {
            return value == 0;
        }
        
        public static bool CheckIfDefault(string value)
        {
            return value == null;
        }
    }
}