using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class OrderedProduct
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        
        public int Quantity { get; set; }
    }
}   