using System;
using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class JobViewModel
    {
        [Required]
        public string PickupLocation { get; set; }

        [Required]
        public string DropoffLocation { get; set; }

        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }

        [Required]
        public string GoodsType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        public double Weight { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        [FutureDate]
        public DateTime PreferredPickupDate { get; set; }

        public string SpecialInstructions { get; set; }

        public double EstimatedDistance { get; set; }
        public decimal EstimatedCost { get; set; }
    }
} 