using System;
using System.Collections.Generic;

namespace TruckDeliveryPlatform.Models
{
    public class SystemWallet
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }

    public class Transaction
    {
        public int Id { get; set; }
        public string ReferenceNumber { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string PaymentDetails { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        
        // Foreign keys
        public int? JobId { get; set; }
        public string? CustomerId { get; set; }
        public string? TruckOwnerId { get; set; }
        
        // Navigation properties
        public virtual Job Job { get; set; }
        public virtual ApplicationUser Customer { get; set; }
        public virtual ApplicationUser TruckOwner { get; set; }
    }

    public enum TransactionType
    {
        CustomerPayment,
        TruckOwnerPayout
    }

    public enum PaymentMethod
    {
        BankTransfer,
        VodafoneCash
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
} 