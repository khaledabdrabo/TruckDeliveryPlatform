using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class RatingViewModel
    {
        public int JobId { get; set; }
        public int TruckOwnerProfileId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [StringLength(500)]
        public string? Comment { get; set; }
    }
} 