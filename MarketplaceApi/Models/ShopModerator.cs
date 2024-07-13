namespace MarketplaceApi.Models
{
    public class ShopModerator
    {
        public int ShopId { get; set; }
        public virtual Shop Shop { get; set; } = null!;
        
        public int ModeratorId { get; set; }
        public virtual User Moderator { get; set; } = null!;
    }
}