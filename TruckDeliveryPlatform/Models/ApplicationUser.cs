using Microsoft.AspNetCore.Identity;

namespace TruckDeliveryPlatform.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserType UserType { get; set; }
    }

    public enum UserType
    {
        Customer,
        TruckOwner
    }
} 