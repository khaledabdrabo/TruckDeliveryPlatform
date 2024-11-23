using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TruckDeliveryPlatform.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public int PickupLocationId { get; set; }

        [Required]
        public int DropoffLocationId { get; set; }

        [Required]
        [StringLength(200)]
        public string PickupLocation { get; set; }

        [Required]
        [StringLength(200)]
        public string DropoffLocation { get; set; }

        [Required]
        [StringLength(50)]
        public string GoodsType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.1, double.MaxValue)]
        public double Weight { get; set; }

        [Required]
        [StringLength(50)]
        public string Size { get; set; }

        [Required]
        public DateTime PreferredPickupDate { get; set; }

        [StringLength(500)]
        public string? SpecialInstructions { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal EstimatedCost { get; set; }

        [Required]
        public JobStatus Status { get; set; } = JobStatus.Active;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? AcceptedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }

        [StringLength(500)]
        public string? CancellationReason { get; set; }

        // Navigation properties
        public virtual ApplicationUser Customer { get; set; }
        public virtual Location PickupLocationNavigation { get; set; }
        public virtual Location DropoffLocationNavigation { get; set; }
        public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public virtual ICollection<TripFeedback> TripFeedbacks { get; set; }

        // Accepted bid reference
        public int? AcceptedBidId { get; set; }
        [ForeignKey("AcceptedBidId")]
        public virtual Bid? AcceptedBid { get; set; }

        [Required]
        public int TruckTypeId { get; set; }
        public virtual TruckType TruckType { get; set; }

        // Helper properties
        [NotMapped]
        public int BidsCount => Bids?.Count(b => !b.IsDeleted) ?? 0;

        [NotMapped]
        public decimal LowestBidAmount => Bids?.Where(b => !b.IsDeleted).Min(b => b.BidAmount) ?? 0;

        [NotMapped]
        public bool HasAcceptedBid => AcceptedBidId.HasValue;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? AcceptedBidAmount { get; set; }

        // Helper methods
        public bool CanBeCancelled()
        {
            return Status == JobStatus.Active || Status == JobStatus.InProgress;
        }

        public bool CanBeCompleted()
        {
            return Status == JobStatus.InProgress;
        }

        public bool CanAcceptBids()
        {
            return Status == JobStatus.Active && !HasAcceptedBid && !CancelledAt.HasValue;
        }

        [Display(Name = "Estimated Waiting Hours")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Waiting hours must be 0 or greater")]
        public decimal EstimatedWaitingHours { get; set; }

        public decimal CalculateEstimatedWaitingCost(decimal hourlyRate)
        {
            return EstimatedWaitingHours * hourlyRate;
        }

        public PaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public decimal? PaidAmount { get; set; }
        public DateTime? PaidAt { get; set; }
        public virtual Transaction Payment { get; set; }

        public DateTime? StartedAt { get; set; }
    }

    public enum JobStatus
    {
        Active,      // Job is posted and accepting bids
        Selected,    // Bid has been selected, waiting for truck owner confirmation
        Accepted,    // Truck owner has accepted the job
        Started,     // Trip has started
        InProgress,  // Delivery is in progress
        Completed,   // Delivery completed
        Canceled     // Job canceled by either party
    }

    public enum PaymentStatus
    {
        Pending,    // Payment is awaiting customer action
        Paid,       // Payment has been received but not yet confirmed
        Completed,  // Payment has been confirmed and completed
        Overdue,    // Payment is past due date
        Refunded,   // Payment has been refunded
        Failed      // Payment attempt failed
    }
} 