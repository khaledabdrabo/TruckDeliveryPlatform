﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TruckDeliveryPlatform.Data;

#nullable disable

namespace TruckDeliveryPlatform.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241118233013_test")]
    partial class test
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LocationTruckOwnerProfile", b =>
                {
                    b.Property<int>("ServiceAreasId")
                        .HasColumnType("int");

                    b.Property<int>("TruckOwnerProfileId")
                        .HasColumnType("int");

                    b.HasKey("ServiceAreasId", "TruckOwnerProfileId");

                    b.HasIndex("TruckOwnerProfileId");

                    b.ToTable("LocationTruckOwnerProfile", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.Bid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BidAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EstimatedDeliveryTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TruckOwnerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("TruckOwnerId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("AcceptedAt")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("AcceptedBidAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("AcceptedBidId")
                        .HasColumnType("int");

                    b.Property<int?>("AcceptedBidId1")
                        .HasColumnType("int");

                    b.Property<string>("CancellationReason")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime?>("CancelledAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CompletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DropoffLocation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("DropoffLocationId")
                        .HasColumnType("int");

                    b.Property<decimal>("EstimatedCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("GoodsType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PickupLocation")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("PickupLocationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PreferredPickupDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SpecialInstructions")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TruckTypeId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AcceptedBidId1");

                    b.HasIndex("CustomerId");

                    b.HasIndex("DropoffLocationId");

                    b.HasIndex("PickupLocationId");

                    b.HasIndex("TruckTypeId");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Cairo",
                            Latitude = 29.9602,
                            Longitude = 31.256900000000002,
                            Name = "Maadi",
                            State = "Cairo"
                        },
                        new
                        {
                            Id = 2,
                            City = "Cairo",
                            Latitude = 30.051100000000002,
                            Longitude = 31.365600000000001,
                            Name = "Nasr City",
                            State = "Cairo"
                        },
                        new
                        {
                            Id = 3,
                            City = "Cairo",
                            Latitude = 30.087499999999999,
                            Longitude = 31.328399999999998,
                            Name = "Heliopolis",
                            State = "Cairo"
                        },
                        new
                        {
                            Id = 4,
                            City = "Alexandria",
                            Latitude = 31.200099999999999,
                            Longitude = 29.918700000000001,
                            Name = "Miami",
                            State = "Alexandria"
                        },
                        new
                        {
                            Id = 5,
                            City = "Alexandria",
                            Latitude = 31.283300000000001,
                            Longitude = 30.0167,
                            Name = "Montazah",
                            State = "Alexandria"
                        },
                        new
                        {
                            Id = 6,
                            City = "Giza",
                            Latitude = 30.039200000000001,
                            Longitude = 31.212199999999999,
                            Name = "Dokki",
                            State = "Giza"
                        },
                        new
                        {
                            Id = 7,
                            City = "Giza",
                            Latitude = 29.9285,
                            Longitude = 30.918800000000001,
                            Name = "6th of October",
                            State = "Giza"
                        },
                        new
                        {
                            Id = 8,
                            City = "Tanta",
                            Latitude = 30.7865,
                            Longitude = 31.000399999999999,
                            Name = "Tanta",
                            State = "Gharbia"
                        },
                        new
                        {
                            Id = 9,
                            City = "Mansoura",
                            Latitude = 31.040900000000001,
                            Longitude = 31.378499999999999,
                            Name = "Mansoura",
                            State = "Dakahlia"
                        },
                        new
                        {
                            Id = 10,
                            City = "Port Said",
                            Latitude = 31.256699999999999,
                            Longitude = 32.283999999999999,
                            Name = "Port Said",
                            State = "Port Said"
                        },
                        new
                        {
                            Id = 11,
                            City = "Suez",
                            Latitude = 29.966799999999999,
                            Longitude = 32.549799999999998,
                            Name = "Suez",
                            State = "Suez"
                        },
                        new
                        {
                            Id = 12,
                            City = "Ismailia",
                            Latitude = 30.596499999999999,
                            Longitude = 32.271500000000003,
                            Name = "Ismailia",
                            State = "Ismailia"
                        },
                        new
                        {
                            Id = 13,
                            City = "Luxor",
                            Latitude = 25.687200000000001,
                            Longitude = 32.639600000000002,
                            Name = "Luxor",
                            State = "Luxor"
                        },
                        new
                        {
                            Id = 14,
                            City = "Aswan",
                            Latitude = 24.088899999999999,
                            Longitude = 32.899799999999999,
                            Name = "Aswan",
                            State = "Aswan"
                        },
                        new
                        {
                            Id = 15,
                            City = "Hurghada",
                            Latitude = 27.257899999999999,
                            Longitude = 33.811599999999999,
                            Name = "Hurghada",
                            State = "Red Sea"
                        });
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.SystemConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("BaseFee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PricePerKilometer")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("SystemConfigs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BaseFee = 50m,
                            PricePerKilometer = 2.5m
                        });
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.TruckOwnerProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AverageRating")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PricePerKilometer")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ProfileImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalRatings")
                        .HasColumnType("int");

                    b.Property<string>("TruckImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TruckTypeId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("TruckTypeId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("TruckOwnerProfiles");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.TruckOwnerRating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("JobId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("TruckOwnerProfileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("JobId");

                    b.HasIndex("TruckOwnerProfileId");

                    b.ToTable("TruckOwnerRatings");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.TruckType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("MaximumWeight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MinimumWeight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TruckTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Small pickup truck suitable for light cargo and furniture",
                            Icon = "fa-pickup-truck",
                            MaximumWeight = 1000m,
                            MinimumWeight = 0m,
                            Name = "Pickup Truck"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Medium-sized truck with enclosed cargo area",
                            Icon = "fa-truck-box",
                            MaximumWeight = 5000m,
                            MinimumWeight = 1000m,
                            Name = "Box Truck"
                        },
                        new
                        {
                            Id = 3,
                            Description = "Open bed truck suitable for construction materials",
                            Icon = "fa-truck-flatbed",
                            MaximumWeight = 8000m,
                            MinimumWeight = 2000m,
                            Name = "Flatbed Truck"
                        },
                        new
                        {
                            Id = 4,
                            Description = "Temperature-controlled truck for perishable goods",
                            Icon = "fa-truck-container",
                            MaximumWeight = 5000m,
                            MinimumWeight = 1000m,
                            Name = "Refrigerated Truck"
                        },
                        new
                        {
                            Id = 5,
                            Description = "Large truck for heavy cargo and long-distance transport",
                            Icon = "fa-truck-moving",
                            MaximumWeight = 25000m,
                            MinimumWeight = 5000m,
                            Name = "Semi-Trailer"
                        });
                });

            modelBuilder.Entity("LocationTruckOwnerProfile", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.Location", null)
                        .WithMany()
                        .HasForeignKey("ServiceAreasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.TruckOwnerProfile", null)
                        .WithMany()
                        .HasForeignKey("TruckOwnerProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.Bid", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.Job", "Job")
                        .WithMany("Bids")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", "TruckOwner")
                        .WithMany()
                        .HasForeignKey("TruckOwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("TruckOwner");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.Job", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.Bid", "AcceptedBid")
                        .WithMany()
                        .HasForeignKey("AcceptedBidId1");

                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.Location", "DropoffLocationNavigation")
                        .WithMany()
                        .HasForeignKey("DropoffLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.Location", "PickupLocationNavigation")
                        .WithMany()
                        .HasForeignKey("PickupLocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.TruckType", "TruckType")
                        .WithMany()
                        .HasForeignKey("TruckTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AcceptedBid");

                    b.Navigation("Customer");

                    b.Navigation("DropoffLocationNavigation");

                    b.Navigation("PickupLocationNavigation");

                    b.Navigation("TruckType");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.TruckOwnerProfile", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.TruckType", "TruckType")
                        .WithMany()
                        .HasForeignKey("TruckTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", "User")
                        .WithOne()
                        .HasForeignKey("TruckDeliveryPlatform.Models.TruckOwnerProfile", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TruckType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.TruckOwnerRating", b =>
                {
                    b.HasOne("TruckDeliveryPlatform.Models.ApplicationUser", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TruckDeliveryPlatform.Models.TruckOwnerProfile", "TruckOwnerProfile")
                        .WithMany("Ratings")
                        .HasForeignKey("TruckOwnerProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Job");

                    b.Navigation("TruckOwnerProfile");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.Job", b =>
                {
                    b.Navigation("Bids");
                });

            modelBuilder.Entity("TruckDeliveryPlatform.Models.TruckOwnerProfile", b =>
                {
                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
