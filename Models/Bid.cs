public class Bid
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string TruckOwnerId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTime EstimatedDeliveryTime { get; set; }
    public string Notes { get; set; }
    public BidStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual Job Job { get; set; }
    public virtual ApplicationUser TruckOwner { get; set; }
}

public enum BidStatus
{
    Pending,
    Accepted,
    Rejected,
    Withdrawn
} 