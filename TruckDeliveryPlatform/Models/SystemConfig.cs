using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckDeliveryPlatform.Models
{
    public class SystemConfig
    {
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerKilometer { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseFee { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Transaction Fee")]
        public decimal TransactionFee { get; set; } = 10M; // Default to 10 EGP

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
} 