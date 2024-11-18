namespace TruckDeliveryPlatform.Models.ViewModels
{
    public class TruckOwnerProfileDetailsViewModel
    {
        public TruckOwnerProfile Profile { get; set; }
        public IEnumerable<TruckOwnerRating> RecentRatings { get; set; }
        public IEnumerable<Job> CompletedJobs { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; }
        public int TotalCompletedJobs { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal AverageResponseTime { get; set; }
    }
} 