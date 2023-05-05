using System.Collections.Generic;

namespace MarketplaceApi
{
    public class ProductEntity
    {
        /*
     Request body
     {
        "UserId" : 1,
        "ShopId" : 1,
        "Name" : "ball",
        "Material" : "wood",
        "Length" : 5,
        "Width" : 5,
        "Height" : 5,
        "Price" : 1500,
        "Quantity" : 10,
        "IsPublic" : "false"
    }
     */
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; } // statue, moving
        public int UseCase { get; set; } // game, watch
        public int WhereUsed { get; set; } // in door, out door 
        public int Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsPublic { get; set; }

        public static List<int> ListOfTypes = new List<int>() { 1, 2, 3, 4, 5};
        public static List<int> ListOfUseCases = new List<int>() { 1, 2, 3, 4, 5 };
        public static List<int> ListOfWhereUsed = new List<int>() { 1, 2, 3, 4, 5 };
        public static List<int> ListOfMaterials = new List<int>() { 1, 2, 3, 4, 5 };
    }
}