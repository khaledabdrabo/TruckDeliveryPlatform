using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckDeliveryPlatform.Models
{
    public class SystemConfig
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerKilometer { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseFee { get; set; }
    }
} 