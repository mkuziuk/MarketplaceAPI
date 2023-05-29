using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class OrderedProduct
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        
        public int Quantity { get; set; }
    }
}   