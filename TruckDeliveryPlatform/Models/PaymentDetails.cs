using TruckDeliveryPlatform.Models;

public class PaymentDetails
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public PaymentMethod Method { get; set; }
    public string AccountNumber { get; set; }
    public string AccountName { get; set; }
    public string BankName { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public virtual ApplicationUser User { get; set; }
} 