using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckDeliveryPlatform.Models
{
    public class TruckOwnerProfile
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string CompanyName { get; set; }
        
        [Required]
        public string LicenseNumber { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
        
        [Required]
        public int TruckTypeId { get; set; }
        
        public string? Description { get; set; }
        
        // New properties for images
        public string? ProfileImagePath { get; set; }
        public string? TruckImagePath { get; set; }
        
        // Rating properties
        public decimal AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public virtual ICollection<TruckOwnerRating> Ratings { get; set; } = new List<TruckOwnerRating>();
        
        // Service Areas - Many-to-Many relationship
        public virtual ICollection<Location> ServiceAreas { get; set; } = new List<Location>();
        
        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual TruckType TruckType { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerKilometer { get; set; }
        
        // Helper method to calculate estimated price
        public decimal CalculateEstimatedPrice(double distance)
        {
            return (decimal)distance * PricePerKilometer;
        }
        
        public bool IsActive { get; set; } = true;
    }
} 