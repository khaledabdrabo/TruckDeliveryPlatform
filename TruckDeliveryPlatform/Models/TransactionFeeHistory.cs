public class TransactionFeeHistory
{
    public int Id { get; set; }
    public decimal PreviousFee { get; set; }
    public decimal NewFee { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
} 