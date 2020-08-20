﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Data;

namespace WebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200820184056_newfieldMig")]
    partial class newfieldMig
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

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
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WebApi.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AirlineId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lon")
                        .HasColumnType("float");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.HasIndex("AirlineId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("WebApi.Models.Address2", b =>
                {
                    b.Property<int>("Address2Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Lat")
                        .HasColumnType("float");

                    b.Property<double>("Lon")
                        .HasColumnType("float");

                    b.Property<int>("RentACarServiceId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Address2Id");

                    b.HasIndex("RentACarServiceId")
                        .IsUnique();

                    b.ToTable("Address2");
                });

            modelBuilder.Entity("WebApi.Models.Airline", b =>
                {
                    b.Property<int>("AirlineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("LogoUrl")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PromoDescription")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AirlineId");

                    b.HasIndex("AdminId")
                        .IsUnique()
                        .HasFilter("[AdminId] IS NOT NULL");

                    b.ToTable("Airlines");
                });

            modelBuilder.Entity("WebApi.Models.AirlineDestionation", b =>
                {
                    b.Property<int>("AirlineId")
                        .HasColumnType("int");

                    b.Property<int>("DestinationId")
                        .HasColumnType("int");

                    b.HasKey("AirlineId", "DestinationId");

                    b.HasIndex("DestinationId");

                    b.ToTable("AirlineDestination");
                });

            modelBuilder.Entity("WebApi.Models.AirlineRate", b =>
                {
                    b.Property<int>("AirlineRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AirlineId")
                        .HasColumnType("int");

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AirlineRateId");

                    b.HasIndex("AirlineId");

                    b.HasIndex("UserId");

                    b.ToTable("AirlineRates");
                });

            modelBuilder.Entity("WebApi.Models.Branch", b =>
                {
                    b.Property<int>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RentACarServiceId")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BranchId");

                    b.HasIndex("RentACarServiceId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("WebApi.Models.Car", b =>
                {
                    b.Property<int>("CarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImageUrl")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("PricePerDay")
                        .HasColumnType("real");

                    b.Property<int?>("RentACarServiceId")
                        .HasColumnType("int");

                    b.Property<int>("SeatsNumber")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("CarId");

                    b.HasIndex("BranchId");

                    b.HasIndex("RentACarServiceId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("WebApi.Models.CarRate", b =>
                {
                    b.Property<int>("CarRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarId")
                        .HasColumnType("int");

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CarRateId");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.ToTable("CarRates");
                });

            modelBuilder.Entity("WebApi.Models.CarRent", b =>
                {
                    b.Property<int>("CarRentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsCarRated")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRACSRated")
                        .HasColumnType("bit");

                    b.Property<int?>("RentedCarCarId")
                        .HasColumnType("int");

                    b.Property<string>("ReturnCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TakeOverCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TakeOverDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("TotalPrice")
                        .HasColumnType("real");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CarRentId");

                    b.HasIndex("RentedCarCarId");

                    b.HasIndex("UserId");

                    b.ToTable("CarRents");
                });

            modelBuilder.Entity("WebApi.Models.CarSpecialOffer", b =>
                {
                    b.Property<int>("CarSpecialOfferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("NewPrice")
                        .HasColumnType("real");

                    b.Property<float>("OldPrice")
                        .HasColumnType("real");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("CarSpecialOfferId");

                    b.HasIndex("CarId");

                    b.ToTable("CarSpecialOffers");
                });

            modelBuilder.Entity("WebApi.Models.Destination", b =>
                {
                    b.Property<int>("DestinationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImageUrl")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DestinationId");

                    b.ToTable("Destinations");
                });

            modelBuilder.Entity("WebApi.Models.Flight", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AirlineId")
                        .HasColumnType("int");

                    b.Property<string>("FlightNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FromDestinationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LandingDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TakeOffDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ToDestinationId")
                        .HasColumnType("int");

                    b.Property<string>("TripTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("tripLength")
                        .HasColumnType("real");

                    b.HasKey("FlightId");

                    b.HasIndex("AirlineId");

                    b.HasIndex("FromDestinationId");

                    b.HasIndex("ToDestinationId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("WebApi.Models.FlightDestination", b =>
                {
                    b.Property<int>("DestinationId")
                        .HasColumnType("int");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.HasKey("DestinationId", "FlightId");

                    b.HasIndex("FlightId");

                    b.ToTable("FlightsAddresses");
                });

            modelBuilder.Entity("WebApi.Models.Friendship", b =>
                {
                    b.Property<string>("User1Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("User2Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Accepted")
                        .HasColumnType("bit");

                    b.Property<bool>("Rejacted")
                        .HasColumnType("bit");

                    b.HasKey("User1Id", "User2Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("WebApi.Models.RentACarService", b =>
                {
                    b.Property<int>("RentACarServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("About")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("LogoUrl")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RentACarServiceId");

                    b.HasIndex("AdminId")
                        .IsUnique()
                        .HasFilter("[AdminId] IS NOT NULL");

                    b.ToTable("RentACarServices");
                });

            modelBuilder.Entity("WebApi.Models.RentCarServiceRates", b =>
                {
                    b.Property<int>("RentCarServiceRatesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Rate")
                        .HasColumnType("real");

                    b.Property<int?>("RentACarServiceId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RentCarServiceRatesId");

                    b.HasIndex("RentACarServiceId");

                    b.HasIndex("UserId");

                    b.ToTable("RentCarServiceRates");
                });

            modelBuilder.Entity("WebApi.Models.Seat", b =>
                {
                    b.Property<int>("SeatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<string>("Class")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Column")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FlightId")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<bool>("Reserved")
                        .HasColumnType("bit");

                    b.Property<string>("Row")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SpecialOfferId")
                        .HasColumnType("int");

                    b.HasKey("SeatId");

                    b.HasIndex("FlightId");

                    b.HasIndex("SpecialOfferId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("WebApi.Models.SpecialOffer", b =>
                {
                    b.Property<int>("SpecialOfferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AirlineId")
                        .HasColumnType("int");

                    b.Property<float>("NewPrice")
                        .HasColumnType("real");

                    b.Property<float>("OldPrice")
                        .HasColumnType("real");

                    b.HasKey("SpecialOfferId");

                    b.HasIndex("AirlineId");

                    b.ToTable("SpecialOffers");
                });

            modelBuilder.Entity("WebApi.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Passport")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("SeatId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TicketId");

                    b.HasIndex("SeatId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("WebApi.Models.Person", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImageUrl")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PasswordChanged")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Person");
                });

            modelBuilder.Entity("WebApi.Models.AirlineAdmin", b =>
                {
                    b.HasBaseType("WebApi.Models.Person");

                    b.HasDiscriminator().HasValue("AirlineAdmin");
                });

            modelBuilder.Entity("WebApi.Models.RentACarServiceAdmin", b =>
                {
                    b.HasBaseType("WebApi.Models.Person");

                    b.HasDiscriminator().HasValue("RentACarServiceAdmin");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.HasBaseType("WebApi.Models.Person");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("UserId");

                    b.HasDiscriminator().HasValue("User");
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
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
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

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.Address", b =>
                {
                    b.HasOne("WebApi.Models.Airline", "Airline")
                        .WithOne("Address")
                        .HasForeignKey("WebApi.Models.Address", "AirlineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.Address2", b =>
                {
                    b.HasOne("WebApi.Models.RentACarService", "RentACarService")
                        .WithOne("Address")
                        .HasForeignKey("WebApi.Models.Address2", "RentACarServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.Airline", b =>
                {
                    b.HasOne("WebApi.Models.AirlineAdmin", "Admin")
                        .WithOne("Airline")
                        .HasForeignKey("WebApi.Models.Airline", "AdminId");
                });

            modelBuilder.Entity("WebApi.Models.AirlineDestionation", b =>
                {
                    b.HasOne("WebApi.Models.Airline", "Airline")
                        .WithMany("Destinations")
                        .HasForeignKey("AirlineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.Destination", "Destination")
                        .WithMany("Airlines")
                        .HasForeignKey("DestinationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.AirlineRate", b =>
                {
                    b.HasOne("WebApi.Models.Airline", "Airline")
                        .WithMany("Rates")
                        .HasForeignKey("AirlineId");

                    b.HasOne("WebApi.Models.User", "User")
                        .WithMany("RateAirline")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApi.Models.Branch", b =>
                {
                    b.HasOne("WebApi.Models.RentACarService", "RentACarService")
                        .WithMany("Branches")
                        .HasForeignKey("RentACarServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.Car", b =>
                {
                    b.HasOne("WebApi.Models.Branch", "Branch")
                        .WithMany("Cars")
                        .HasForeignKey("BranchId");

                    b.HasOne("WebApi.Models.RentACarService", "RentACarService")
                        .WithMany("Cars")
                        .HasForeignKey("RentACarServiceId");
                });

            modelBuilder.Entity("WebApi.Models.CarRate", b =>
                {
                    b.HasOne("WebApi.Models.Car", "Car")
                        .WithMany("Rates")
                        .HasForeignKey("CarId");

                    b.HasOne("WebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApi.Models.CarRent", b =>
                {
                    b.HasOne("WebApi.Models.Car", "RentedCar")
                        .WithMany("Rents")
                        .HasForeignKey("RentedCarCarId");

                    b.HasOne("WebApi.Models.User", "User")
                        .WithMany("CarRents")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApi.Models.CarSpecialOffer", b =>
                {
                    b.HasOne("WebApi.Models.Car", "Car")
                        .WithMany("SpecialOffers")
                        .HasForeignKey("CarId");
                });

            modelBuilder.Entity("WebApi.Models.Flight", b =>
                {
                    b.HasOne("WebApi.Models.Airline", "Airline")
                        .WithMany("Flights")
                        .HasForeignKey("AirlineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.Destination", "From")
                        .WithMany("From")
                        .HasForeignKey("FromDestinationId");

                    b.HasOne("WebApi.Models.Destination", "To")
                        .WithMany("To")
                        .HasForeignKey("ToDestinationId");
                });

            modelBuilder.Entity("WebApi.Models.FlightDestination", b =>
                {
                    b.HasOne("WebApi.Models.Destination", "Destination")
                        .WithMany("Flights")
                        .HasForeignKey("DestinationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.Flight", "Flight")
                        .WithMany("Stops")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.Friendship", b =>
                {
                    b.HasOne("WebApi.Models.User", "User1")
                        .WithMany("FriendshipInvitations")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("WebApi.Models.User", "User2")
                        .WithMany("FriendshipRequests")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Models.RentACarService", b =>
                {
                    b.HasOne("WebApi.Models.RentACarServiceAdmin", "Admin")
                        .WithOne("RentACarService")
                        .HasForeignKey("WebApi.Models.RentACarService", "AdminId");
                });

            modelBuilder.Entity("WebApi.Models.RentCarServiceRates", b =>
                {
                    b.HasOne("WebApi.Models.RentACarService", "RentACarService")
                        .WithMany("Rates")
                        .HasForeignKey("RentACarServiceId");

                    b.HasOne("WebApi.Models.User", "User")
                        .WithMany("RateRACService")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApi.Models.Seat", b =>
                {
                    b.HasOne("WebApi.Models.Flight", "Flight")
                        .WithMany("Seats")
                        .HasForeignKey("FlightId");

                    b.HasOne("WebApi.Models.SpecialOffer", "SpecialOffer")
                        .WithMany("Seats")
                        .HasForeignKey("SpecialOfferId");
                });

            modelBuilder.Entity("WebApi.Models.SpecialOffer", b =>
                {
                    b.HasOne("WebApi.Models.Airline", "Airline")
                        .WithMany("SpecialOffers")
                        .HasForeignKey("AirlineId");
                });

            modelBuilder.Entity("WebApi.Models.Ticket", b =>
                {
                    b.HasOne("WebApi.Models.Seat", "Seat")
                        .WithOne("Ticket")
                        .HasForeignKey("WebApi.Models.Ticket", "SeatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Models.User", "User")
                        .WithMany("FlightReservations")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("WebApi.Models.User", b =>
                {
                    b.HasOne("WebApi.Models.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
