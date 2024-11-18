using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models
{
    public class Location
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        
        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public string FullAddress => $"{Name}, {City}, {State}";
    }
} 