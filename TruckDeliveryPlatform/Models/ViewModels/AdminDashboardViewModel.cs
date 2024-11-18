namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IList<ApplicationUser> TotalCustomers { get; set; }
        public IList<ApplicationUser> TotalTruckOwners { get; set; }
        public int TotalJobs { get; set; }
        public int TotalBids { get; set; }
        public int ActiveJobs { get; set; }
        public int CompletedJobs { get; set; }
        public SystemConfig SystemConfig { get; set; }
        public IList<Job> RecentJobs { get; set; }
        public IList<TruckOwnerProfile> RecentTruckOwners { get; set; }
        public Dictionary<JobStatus, int> JobsByStatus { get; set; }
        public double AverageBidsPerJob { get; set; }
    }
} 