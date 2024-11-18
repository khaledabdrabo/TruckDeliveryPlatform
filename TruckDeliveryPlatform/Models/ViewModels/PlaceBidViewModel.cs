using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class PlaceBidViewModel
    {
        public int JobId { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid amount must be greater than 0")]
        public decimal BidAmount { get; set; }
        
        [Required]
        public DateTime EstimatedDeliveryTime { get; set; }
        
        public string? Notes { get; set; }
    }
} 