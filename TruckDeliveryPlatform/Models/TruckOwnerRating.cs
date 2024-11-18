using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models
{
    public class TruckOwnerRating
    {
        public int Id { get; set; }
        
        [Required]
        public int TruckOwnerProfileId { get; set; }
        
        [Required]
        public string CustomerId { get; set; }
        
        [Required]
        public int JobId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [StringLength(500)]
        public string? Comment { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public virtual TruckOwnerProfile TruckOwnerProfile { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        public virtual Job Job { get; set; }
    }
} 