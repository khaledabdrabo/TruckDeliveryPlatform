using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TruckDeliveryPlatform.Models
{
    public class Bid
    {
        public int Id { get; set; }
        [Required]
        public int JobId { get; set; }
        [Required]
        public string TruckOwnerId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BidAmount { get; set; }
        [Required]
        public DateTime EstimatedDeliveryTime { get; set; }
        public string? Notes { get; set; }
        [Required]
        public BidStatus Status { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public decimal WaitingHourPrice { get; set; }
        public decimal TotalWaitingPrice => WaitingHourPrice * Job.EstimatedWaitingHours;
        public decimal TotalBidAmount => BidAmount + TotalWaitingPrice;

        public virtual Job Job { get; set; }
        public virtual ApplicationUser TruckOwner { get; set; }

        public void InitializeFromTruckOwnerProfile(TruckOwnerProfile profile)
        {
            WaitingHourPrice = profile.WaitingHourPrice;
        }
    }

    public enum BidStatus
    {
        Pending,
        Selected,
        Accepted,
        Rejected,
        Withdrawn
    }
} 