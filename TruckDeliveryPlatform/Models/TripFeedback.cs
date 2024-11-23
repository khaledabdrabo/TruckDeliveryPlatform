using TruckDeliveryPlatform.Models;

public class TripFeedback
{
    public int Id { get; set; }
    public int JobId { get; set; }
    public string TruckOwnerId { get; set; }
    public int CustomerRating { get; set; }
    public string CustomerCooperation { get; set; }
    public string Comments { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual Job Job { get; set; }
    public virtual ApplicationUser TruckOwner { get; set; }
} 