public class Job
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public string PickupLocation { get; set; }
    public string DropoffLocation { get; set; }
    public double PickupLatitude { get; set; }
    public double PickupLongitude { get; set; }
    public double DropoffLatitude { get; set; }
    public double DropoffLongitude { get; set; }
    public string GoodsType { get; set; }
    public int Quantity { get; set; }
    public double Weight { get; set; }
    public string Size { get; set; }
    public DateTime PreferredPickupDate { get; set; }
    public string SpecialInstructions { get; set; }
    public double EstimatedDistance { get; set; }
    public decimal EstimatedCost { get; set; }
    public JobStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public virtual ApplicationUser Customer { get; set; }
    public virtual ICollection<Bid> Bids { get; set; }
}

public enum JobStatus
{
    Active,
    InProgress,
    Completed,
    Canceled
} 