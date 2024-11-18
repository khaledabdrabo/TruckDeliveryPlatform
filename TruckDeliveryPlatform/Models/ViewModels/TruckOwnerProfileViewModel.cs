using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class TruckOwnerProfileViewModel
    {
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        
        [Required]
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; }
        
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [Display(Name = "Truck Type")]
        public int TruckTypeId { get; set; }
        
        [Display(Name = "Description")]
        public string? Description { get; set; }
        
        [Display(Name = "Profile Image")]
        public IFormFile? ProfileImage { get; set; }
        
        [Display(Name = "Truck Image")]
        public IFormFile? TruckImage { get; set; }
        
        [Required]
        [Display(Name = "Service Areas")]
        public List<int> ServiceAreaIds { get; set; } = new List<int>();
        
        [Required]
        [Display(Name = "Price per Kilometer (EGP)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal PricePerKilometer { get; set; }
    }
} 