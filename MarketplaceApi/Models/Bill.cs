using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketplaceApi.Models
{
    public class Bill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int Id { get; set; }
        public DateTime SellDate { get; set; }
        public string WayOfPayement { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}