using System.ComponentModel.DataAnnotations;

namespace TruckDeliveryPlatform.Models
{
    public class TruckType
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Icon { get; set; }
        
        [Required]
        public decimal MinimumWeight { get; set; }
        
        [Required]
        public decimal MaximumWeight { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
} 