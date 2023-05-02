namespace MarketplaceApi.Models
{
    public class ShopModerator
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; } = null!;
        
        public int ModeratorId { get; set; }
        public User Moderator { get; set; } = null!;
    }
}