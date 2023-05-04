namespace MarketplaceApi
{
    public class ProductEntity
    {
        public int UserId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public bool IsPublic { get; set; }
    }
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
}