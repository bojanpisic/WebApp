using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
                                                : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //one to one
            //airline-address
            modelBuilder.Entity<Airline>()
                .HasOne(a => a.Address)
                .WithOne(b => b.Airline)
                .HasForeignKey<Address>(b => b.AddressId);
            //airline-admin
            modelBuilder.Entity<Airline>()
                .HasOne(a => a.Admin)
                .WithOne(b => b.Airline)
                .HasForeignKey<AirlineAdmin>(b => b.AirlineId);
            //destination-address
            modelBuilder.Entity<CityStateAddress>()
                .HasOne(a => a.Destination)
                .WithOne(b => b.Address)
                .HasForeignKey<Destination>(b => b.AddressId);
            //ticket-seat
            modelBuilder.Entity<Seat>()
                .HasOne(a => a.Ticket)
                .WithOne(b => b.Seat)
                .HasForeignKey<Ticket>(b => b.SeatId);



            //one to many
            //airline-flights
            modelBuilder.Entity<Airline>()
               .HasMany(c => c.Flights)
               .WithOne(e => e.Airline);
            //airline-rate
            modelBuilder.Entity<Airline>()
              .HasMany(c => c.Rates)
              .WithOne(e => e.Airline);
            //flight-from-address
            modelBuilder.Entity<CityStateAddress>()
               .HasMany(c => c.From)
               .WithOne(e => e.From);
            //flight-from-address
            modelBuilder.Entity<CityStateAddress>()
               .HasMany(c => c.To)
               .WithOne(e => e.To);
            //flight-seats
            modelBuilder.Entity<Flight>()
               .HasMany(c => c.Seats)
               .WithOne(e => e.Flight);
            //user-ticket
            modelBuilder.Entity<User>()
               .HasMany(c => c.FlightReservations)
               .WithOne(e => e.User);


            //many to many
            //flight-address
            modelBuilder.Entity<FlightAddress>()
                .HasKey(bc => new { bc.CityStateAddressId, bc.FlightId });
            modelBuilder.Entity<FlightAddress>()
                .HasOne(bc => bc.Flight)
                .WithMany(b => b.ChangeOvers)
                .HasForeignKey(bc => bc.FlightId);
            modelBuilder.Entity<FlightAddress>()
                .HasOne(bc => bc.CityStateAddress)
                .WithMany(c => c.Flights)
                .HasForeignKey(bc => bc.CityStateAddressId);
            //airline-destinations
            modelBuilder.Entity<AirlineDestionation>()
               .HasKey(bc => new { bc.AirlineId, bc.DestinationId });
            modelBuilder.Entity<AirlineDestionation>()
                .HasOne(bc => bc.Airline)
                .WithMany(b => b.Destinations)
                .HasForeignKey(bc => bc.AirlineId);
            modelBuilder.Entity<AirlineDestionation>()
                .HasOne(bc => bc.Destination)
                .WithMany(c => c.Airlines)
                .HasForeignKey(bc => bc.DestinationId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CityStateAddress> CityStateAddresses { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightAddress> FlightsAddresses { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
