using TruckDeliveryPlatform.Models;

public class SystemWalletViewModel
{
    public SystemWallet Wallet { get; set; }
    public List<Transaction> RecentTransactions { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalPayout { get; set; }
    public Dictionary<TransactionType, decimal> TransactionSummary { get; set; }
    public Dictionary<PaymentMethod, int> PaymentMethodStats { get; set; }
    public Dictionary<DateTime, decimal> DailyBalance { get; set; }

    // Statistics
    public int PendingPayments { get; set; }
    public int CompletedPayments { get; set; }
    public int FailedPayments { get; set; }
    public decimal PendingPayoutsAmount { get; set; }

    // Pagination
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    // Revenue Analytics
    public Dictionary<string, decimal> RevenueAnalytics { get; set; }
    public Dictionary<PaymentMethod, decimal> RevenueByPaymentMethod { get; set; }
    public Dictionary<DateTime, decimal> DailyRevenue { get; set; }
    public decimal TransactionFee { get; set; }

    // Helper methods for revenue analytics
    public decimal GetRevenueGrowth()
    {
        var lastMonth = DailyRevenue
            .Where(x => x.Key < DateTime.UtcNow.AddMonths(-1))
            .Sum(x => x.Value);
        var thisMonth = RevenueAnalytics["MonthlyRevenue"];
        
        return lastMonth > 0 
            ? ((thisMonth - lastMonth) / lastMonth) * 100 
            : 0;
    }

    public string FormatGrowth(decimal growth)
    {
        var sign = growth >= 0 ? "+" : "";
        return $"{sign}{growth:F1}%";
    }

    public IEnumerable<TransactionFeeHistory> FeeHistory { get; set; }
} 