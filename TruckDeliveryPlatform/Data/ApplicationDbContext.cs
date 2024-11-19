using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TruckDeliveryPlatform.Models;

namespace TruckDeliveryPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TruckType> TruckTypes { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<TruckOwnerProfile> TruckOwnerProfiles { get; set; }
        public DbSet<TruckOwnerRating> TruckOwnerRatings { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SystemWallet> SystemWallets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TruckOwnerProfile>()
                .Property(p => p.WaitingHourPrice)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Job>()
                .Property(j => j.EstimatedWaitingHours)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Bid>()
                .Property(b => b.WaitingHourPrice)
                .HasColumnType("decimal(18,2)");

            // Configure Job relationships
            builder.Entity<Job>()
                .HasOne(j => j.Customer)
                .WithMany()
                .HasForeignKey(j => j.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.PickupLocationNavigation)
                .WithMany()
                .HasForeignKey(j => j.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.DropoffLocationNavigation)
                .WithMany()
                .HasForeignKey(j => j.DropoffLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Job>()
                .HasOne(j => j.TruckType)
                .WithMany()
                .HasForeignKey(j => j.TruckTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Job-AcceptedBid relationship
            builder.Entity<Job>()
                .HasOne(j => j.AcceptedBid)
                .WithOne()
                .HasForeignKey<Job>(j => j.AcceptedBidId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Job-Bids relationship
            builder.Entity<Job>()
                .HasMany(j => j.Bids)
                .WithOne(b => b.Job)
                .HasForeignKey(b => b.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Bid relationships
            builder.Entity<Bid>()
                .HasOne(b => b.TruckOwner)
                .WithMany()
                .HasForeignKey(b => b.TruckOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TruckOwnerProfile relationships
            builder.Entity<TruckOwnerProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<TruckOwnerProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TruckOwnerProfile>()
                .HasOne(p => p.TruckType)
                .WithMany()
                .HasForeignKey(p => p.TruckTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TruckOwnerRating relationships
            builder.Entity<TruckOwnerRating>()
                .HasOne(r => r.TruckOwnerProfile)
                .WithMany(p => p.Ratings)
                .HasForeignKey(r => r.TruckOwnerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TruckOwnerRating>()
                .HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TruckOwnerRating>()
                .HasOne(r => r.Job)
                .WithMany()
                .HasForeignKey(r => r.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TruckOwnerProfile-Location many-to-many relationship
            builder.Entity<TruckOwnerProfile>()
                .HasMany(p => p.ServiceAreas)
                .WithMany()
                .UsingEntity(j => j.ToTable("LocationTruckOwnerProfile"));

            // Seed SystemConfig
            builder.Entity<SystemConfig>().HasData(
                new SystemConfig 
                { 
                    Id = 1, 
                    PricePerKilometer = 2.5M, // 2.5 EGP per kilometer
                    BaseFee = 50M // 50 EGP base fee
                }
            );

            // Seed Locations with coordinates
            builder.Entity<Location>().HasData(
                // Cairo
                new Location { 
                    Id = 1, 
                    Name = "Maadi", 
                    City = "Cairo", 
                    State = "Cairo",
                    Latitude = 29.9602,
                    Longitude = 31.2569
                },
                new Location { 
                    Id = 2, 
                    Name = "Nasr City", 
                    City = "Cairo", 
                    State = "Cairo",
                    Latitude = 30.0511,
                    Longitude = 31.3656
                },
                new Location { 
                    Id = 3, 
                    Name = "Heliopolis", 
                    City = "Cairo", 
                    State = "Cairo",
                    Latitude = 30.0875,
                    Longitude = 31.3284
                },
                // Alexandria
                new Location { 
                    Id = 4, 
                    Name = "Miami", 
                    City = "Alexandria", 
                    State = "Alexandria",
                    Latitude = 31.2001,
                    Longitude = 29.9187
                },
                new Location { 
                    Id = 5, 
                    Name = "Montazah", 
                    City = "Alexandria", 
                    State = "Alexandria",
                    Latitude = 31.2833,
                    Longitude = 30.0167
                },
                // Giza
                new Location { 
                    Id = 6, 
                    Name = "Dokki", 
                    City = "Giza", 
                    State = "Giza",
                    Latitude = 30.0392,
                    Longitude = 31.2122
                },
                new Location { 
                    Id = 7, 
                    Name = "6th of October", 
                    City = "Giza", 
                    State = "Giza",
                    Latitude = 29.9285,
                    Longitude = 30.9188
                },
                // Delta Cities
                new Location { 
                    Id = 8, 
                    Name = "Tanta", 
                    City = "Tanta", 
                    State = "Gharbia",
                    Latitude = 30.7865,
                    Longitude = 31.0004
                },
                new Location { 
                    Id = 9, 
                    Name = "Mansoura", 
                    City = "Mansoura", 
                    State = "Dakahlia",
                    Latitude = 31.0409,
                    Longitude = 31.3785
                },
                // Canal Cities
                new Location { 
                    Id = 10, 
                    Name = "Port Said", 
                    City = "Port Said", 
                    State = "Port Said",
                    Latitude = 31.2567,
                    Longitude = 32.2840
                },
                new Location { 
                    Id = 11, 
                    Name = "Suez", 
                    City = "Suez", 
                    State = "Suez",
                    Latitude = 29.9668,
                    Longitude = 32.5498
                },
                new Location { 
                    Id = 12, 
                    Name = "Ismailia", 
                    City = "Ismailia", 
                    State = "Ismailia",
                    Latitude = 30.5965,
                    Longitude = 32.2715
                },
                // Upper Egypt
                new Location { 
                    Id = 13, 
                    Name = "Luxor", 
                    City = "Luxor", 
                    State = "Luxor",
                    Latitude = 25.6872,
                    Longitude = 32.6396
                },
                new Location { 
                    Id = 14, 
                    Name = "Aswan", 
                    City = "Aswan", 
                    State = "Aswan",
                    Latitude = 24.0889,
                    Longitude = 32.8998
                },
                // Red Sea
                new Location { 
                    Id = 15, 
                    Name = "Hurghada", 
                    City = "Hurghada", 
                    State = "Red Sea",
                    Latitude = 27.2579,
                    Longitude = 33.8116
                }
            );

            // Seed TruckTypes
            builder.Entity<TruckType>().HasData(
                new TruckType 
                { 
                    Id = 1, 
                    Name = "Pickup Truck", 
                    Icon = "fa-pickup-truck",
                    MinimumWeight = 0,
                    MaximumWeight = 1000,
                    Description = "Small pickup truck suitable for light cargo and furniture"
                },
                new TruckType 
                { 
                    Id = 2, 
                    Name = "Box Truck", 
                    Icon = "fa-truck-box",
                    MinimumWeight = 1000,
                    MaximumWeight = 5000,
                    Description = "Medium-sized truck with enclosed cargo area"
                },
                new TruckType 
                { 
                    Id = 3, 
                    Name = "Flatbed Truck", 
                    Icon = "fa-truck-flatbed",
                    MinimumWeight = 2000,
                    MaximumWeight = 8000,
                    Description = "Open bed truck suitable for construction materials"
                },
                new TruckType 
                { 
                    Id = 4, 
                    Name = "Refrigerated Truck", 
                    Icon = "fa-truck-container",
                    MinimumWeight = 1000,
                    MaximumWeight = 5000,
                    Description = "Temperature-controlled truck for perishable goods"
                },
                new TruckType 
                { 
                    Id = 5, 
                    Name = "Semi-Trailer", 
                    Icon = "fa-truck-moving",
                    MinimumWeight = 5000,
                    MaximumWeight = 25000,
                    Description = "Large truck for heavy cargo and long-distance transport"
                }
            );

            // Configure Transaction relationships
            builder.Entity<Transaction>()
                .HasOne(t => t.Job)
                .WithMany()
                .HasForeignKey(t => t.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.Customer)
                .WithMany()
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Transaction>()
                .HasOne(t => t.TruckOwner)
                .WithMany()
                .HasForeignKey(t => t.TruckOwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure SystemWallet
            builder.Entity<SystemWallet>()
                .HasMany(w => w.Transactions)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 