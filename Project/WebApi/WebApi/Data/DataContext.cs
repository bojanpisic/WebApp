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
            modelBuilder.Entity<CarRent>()
                .HasOne(a => a.FlightReservation)
                .WithOne(b => b.CarRent)
                .HasForeignKey<FlightReservation>(b => b.CarRentId);

            //airline-address
            modelBuilder.Entity<Airline>()
                .HasOne(a => a.Address)
                .WithOne(b => b.Airline)
                .HasForeignKey<Address>(b => b.AirlineId);
            // racs - address
            modelBuilder.Entity<RentACarService>()
               .HasOne(a => a.Address)
               .WithOne(b => b.RentACarService)
               .HasForeignKey<Address2>(b => b.RentACarServiceId);
            //airline-admin
            modelBuilder.Entity<AirlineAdmin>()
                .HasOne(a => a.Airline)
                .WithOne(b => b.Admin)
                .HasForeignKey<Airline>(b => b.AdminId);
            //ticket-seat
            modelBuilder.Entity<Seat>()
                .HasOne(a => a.Ticket)
                .WithOne(b => b.Seat)
                .HasForeignKey<Ticket>(b => b.SeatId);
            modelBuilder.Entity<Seat>()
                .HasOne(a => a.Ticket2)
                .WithOne(b => b.Seat)
                .HasForeignKey<Ticket2>(b => b.SeatId);
            modelBuilder.Entity<Seat>()
            .HasOne(a => a.Invitation)
            .WithOne(b => b.Seat)
            .HasForeignKey<Invitation>(b => b.SeatId);

            //one to many

            modelBuilder.Entity<User>()
               .HasMany(c => c.TripRequests)
               .WithOne(e => e.Receiver);
            modelBuilder.Entity<User>()
               .HasMany(c => c.TripInvitations)
               .WithOne(e => e.Sender);

            modelBuilder.Entity<Airline>()
               .HasMany(c => c.SpecialOffers)
               .WithOne(e => e.Airline);
            //specoff-seat
            modelBuilder.Entity<SpecialOffer>()
               .HasMany(c => c.Seats)
               .WithOne(e => e.SpecialOffer);
            //airline-flights
            modelBuilder.Entity<Airline>()
               .HasMany(c => c.Flights)
               .WithOne(e => e.Airline);
            ////airline-rate
            //modelBuilder.Entity<Airline>()
            //  .HasMany(c => c.Rates)
            //  .WithOne(e => e.Airline);
            //flight-from-address
            modelBuilder.Entity<Destination>()
               .HasMany(c => c.From)
               .WithOne(e => e.From);
            //flight-from-address
            modelBuilder.Entity<Destination>()
               .HasMany(c => c.To)
               .WithOne(e => e.To);
            //flight-seats
            modelBuilder.Entity<Flight>()
               .HasMany(c => c.Seats)
               .WithOne(e => e.Flight);
            //user-flight res
            modelBuilder.Entity<User>()
               .HasMany(c => c.FlightReservations)
               .WithOne(e => e.User);
            //racservice-branch
            modelBuilder.Entity<RentACarService>()
               .HasMany(c => c.Branches)
               .WithOne(e => e.RentACarService);
            //racservice-car
            modelBuilder.Entity<RentACarService>()
               .HasMany(c => c.Cars)
               .WithOne(e => e.RentACarService);
            //branch-car
            modelBuilder.Entity<Branch>()
               .HasMany(c => c.Cars)
               .WithOne(e => e.Branch);
            //car - specoff
            modelBuilder.Entity<Car>()
               .HasMany(c => c.SpecialOffers)
               .WithOne(e => e.Car);
            modelBuilder.Entity<Car>()
               .HasMany(c => c.Rents)
               .WithOne(e => e.RentedCar);
            modelBuilder.Entity<User>()
               .HasMany(c => c.CarRents)
               .WithOne(e => e.User);

            modelBuilder.Entity<FlightReservation>()
                .HasMany(c => c.Tickets)
                .WithOne(e => e.Reservation);
            modelBuilder.Entity<FlightReservation>()
                .HasMany(c => c.UnregistredFriendsTickets)
                .WithOne(e => e.Reservation);


            //many to many
            //flight-address
            modelBuilder.Entity<FlightDestination>()
                .HasKey(bc => new { bc.DestinationId, bc.FlightId });
            modelBuilder.Entity<FlightDestination>()
                .HasOne(bc => bc.Flight)
                .WithMany(b => b.Stops)
                .HasForeignKey(bc => bc.FlightId);
            modelBuilder.Entity<FlightDestination>()
                .HasOne(bc => bc.Destination)
                .WithMany(c => c.Flights)
                .HasForeignKey(bc => bc.DestinationId);
            ////friends
            modelBuilder.Entity<Friendship>()
                .HasKey(bc => new { bc.User1Id, bc.User2Id });
            modelBuilder.Entity<Friendship>()
                .HasOne(bc => bc.User1)
                .WithMany(b => b.FriendshipInvitations)
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Friendship>()
                .HasOne(bc => bc.User2)
                .WithMany(c => c.FriendshipRequests)
                .HasForeignKey(bc => bc.User2Id)
                .OnDelete(DeleteBehavior.NoAction);
            
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
            ////airline-rate-user
            //modelBuilder.Entity<AirlineRate>()
            //    .HasKey(bc => new { bc.UserId, bc.AirlineId });
            //modelBuilder.Entity<AirlineRate>()
            //    .HasOne(bc => bc.User)
            //    .WithMany(b => b.RateAirline)
            //    .HasForeignKey(bc => bc.UserId);
            //modelBuilder.Entity<AirlineRate>()
            //    .HasOne(bc => bc.Airline)
            //    .WithMany(c => c.Rates)
            //    .HasForeignKey(bc => bc.AirlineId);
            ////racservice-rate-user
            //modelBuilder.Entity<RentCarServiceRates>()
            //    .HasKey(bc => new { bc.RentACarServiceId, bc.UserId });
            //modelBuilder.Entity<RentCarServiceRates>()
            //    .HasOne(bc => bc.User)
            //    .WithMany(b => b.RateRACService)
            //    .HasForeignKey(bc => bc.UserId);
            //modelBuilder.Entity<RentCarServiceRates>()
            //    .HasOne(bc => bc.RentACarService)
            //    .WithMany(c => c.Rates)
            //    .HasForeignKey(bc => bc.RentACarServiceId);


            //rates -> one to many
            modelBuilder.Entity<Airline>()
               .HasMany(c => c.Rates)
               .WithOne(e => e.Airline);
            modelBuilder.Entity<Flight>()
               .HasMany(c => c.Rates)
               .WithOne(e => e.Flight);
            modelBuilder.Entity<RentACarService>()
               .HasMany(c => c.Rates)
               .WithOne(e => e.RentACarService);
            modelBuilder.Entity<Car>()
               .HasMany(c => c.Rates)
               .WithOne(e => e.Car);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<AirlineAdmin> AirlineAdmins{ get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightDestination> FlightsAddresses { get; set; }
        public DbSet<FlightReservation> FlightReservations { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Ticket2> Tickets2 { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public DbSet<RentACarService> RentACarServices { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<AirlineRate> AirlineRates { get; set; }
        public DbSet<AirlineDestionation> AirlineDestination { get; set; }

        public DbSet<RentCarServiceRates> RentCarServiceRates { get; set; }
        public DbSet<SpecialOffer> SpecialOffers { get; set; }
        public DbSet<CarSpecialOffer> CarSpecialOffers { get; set; }
        public DbSet<CarRent> CarRents { get; set; }
        public DbSet<CarRate> CarRates { get; set; }
        public DbSet<FlightRate> FlightRates { get; set; }
        public DbSet<Bonus> Bonus { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

    }
}
