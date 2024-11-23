using System.Collections.Generic;

namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public AdminDashboardViewModel()
        {
            // Initialize collections
            TotalCustomers = new List<ApplicationUser>();
            TotalTruckOwners = new List<ApplicationUser>();
            RecentJobs = new List<Job>();
            RecentTruckOwners = new List<TruckOwnerProfile>();
            JobsByStatus = new Dictionary<JobStatus, int>();
            RatingDistribution = new Dictionary<int, int>();
            RecentRatings = new List<TruckOwnerRating>();
            SystemConfig = new SystemConfig 
            { 
                PricePerKilometer = 2.5M, // Default value
                BaseFee = 50M // Default value
            };
        }

        public ICollection<ApplicationUser> TotalCustomers { get; set; }
        public ICollection<ApplicationUser> TotalTruckOwners { get; set; }
        public int ActiveJobs { get; set; }
        public int CompletedJobs { get; set; }
        public int TotalJobs { get; set; }
        public int TotalBids { get; set; }
        public double CompletionRate { get; set; }
        public ICollection<Job> RecentJobs { get; set; }
        public ICollection<TruckOwnerProfile> RecentTruckOwners { get; set; }
        public SystemConfig SystemConfig { get; set; }
        public Dictionary<JobStatus, int> JobsByStatus { get; set; }
        public double AverageBidsPerJob { get; set; }
        public double AverageResponseTime { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; }
        public ICollection<TruckOwnerRating> RecentRatings { get; set; }
    }
} 