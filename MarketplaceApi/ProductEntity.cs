using System.Collections.Generic;

namespace MarketplaceApi
{
    public class ProductEntity
    {
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } 
        public int UseCase { get; set; }
        public int WhereUsed { get; set; }
        public int Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsPublic { get; set; }

        public static List<int> ListOfTypes = new List<int>() { 1, 2, 3, 4, 5 };
        public static List<int> ListOfUseCases = new List<int>() { 1, 2, 3, 4, 5 };
        public static List<int> ListOfWhereUsed = new List<int>() { 1, 2, 3, 4, 5 };
        public static List<int> ListOfMaterials = new List<int>() { 1, 2, 3, 4, 5 };
    }
}