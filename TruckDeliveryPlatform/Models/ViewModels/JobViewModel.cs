using System;
using System.ComponentModel.DataAnnotations;
using TruckDeliveryPlatform.Models.Validation;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class JobViewModel
    {
        [Required]
        [Display(Name = "Pickup Location")]
        public int PickupLocationId { get; set; }

        [Required]
        [Display(Name = "Drop-off Location")]
        public int DropoffLocationId { get; set; }

        [Required]
        [Display(Name = "Type of Goods")]
        public string GoodsType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Weight must be greater than 0")]
        [Display(Name = "Weight (kg)")]
        public double Weight { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        [Display(Name = "Preferred Pickup Date")]
        [FutureDate]
        public DateTime PreferredPickupDate { get; set; }

        [Display(Name = "Special Instructions")]
        public string? SpecialInstructions { get; set; }

        [Required]
        [Display(Name = "Truck Type")]
        public int TruckTypeId { get; set; }

        [Display(Name = "Estimated Waiting Hours")]
        [Required(ErrorMessage = "Please enter estimated waiting hours")]
        [Range(0, 24, ErrorMessage = "Waiting hours must be between 0 and 24")]
        public decimal EstimatedWaitingHours { get; set; }
    }
} 