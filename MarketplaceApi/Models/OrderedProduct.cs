namespace MarketplaceApi.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        
        public int ProductsId { get; set; }
        public Product Product { get; set; } = null!;
        
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}   