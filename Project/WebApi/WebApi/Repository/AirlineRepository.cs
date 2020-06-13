using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public class AirlineRepository : IAirlineRepository
    {
        private readonly DataContext context;
        private readonly UserManager<Person> userManager;
        public AirlineRepository(DataContext _context, UserManager<Person> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }
        public async Task<IdentityResult> AddFlight(Flight flight)
        {
            context.Flights.Add(flight);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Error when trying to add flight"});
        }

        public async Task<Flight> GetFlightByNumber(Airline airline, string flightNum)
        {
            //return await context.Flights.FirstAsync(f => f.AirlineId == airlineId && f.FlightNumber == flightNum);
            return await context.Flights.FirstOrDefaultAsync(f => f.Airline == airline && f.FlightNumber == flightNum);
        }

        public async Task<ICollection<Destination>> GetAirlineDestinations(Airline airline)
        {
            //return await context.Destinations.Where(d => d.Airlines.Any( a=> a.Airline == airline)).ToListAsync(); 
            return await context.AirlineDestination.Where(ad => ad.AirlineId == airline.AirlineId).Select(ad => ad.Destination).ToListAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<Airline> GetAirlineById(int airlineId)
        {
            return await context.Airlines.Include(a => a.Address).Include(a => a.Destinations).ThenInclude(d => d.Destination).FirstAsync(airline => airline.AirlineId == airlineId);
        }

        public async Task<Destination> GetDestination(int destId)
        {
            return await context.Destinations.FirstAsync(d => d.DestinationId == destId);
        }

        public async Task<IdentityResult> AddDestination(Destination destination)
        {
            context.Destinations.Add(destination);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Add error"});
        }

        //public async Task<IdentityResult> RemoveDestination(Destination destination)
        //{
        //    //context.Destinations.Remove(destination);


        //    var res = await context.SaveChangesAsync();

        //    if (res > 0)
        //    {
        //        return IdentityResult.Success;
        //    }

        //    return IdentityResult.Failed(new IdentityError() { Code = "Add error" });
        //}

        public async Task<IEnumerable<Flight>> GetFlightsOfAirline(int airlineId)
        {
            return await context.Flights.Where( f => f.AirlineId == airlineId).Include(f => f.From).Include(f => f.To).Include(f=>f.Stops).ToListAsync();
        }

        public async Task<IEnumerable<Airline>> GetTopRated()
        {
            return await context.Airlines.OrderBy(a => a.Rates).Take(5).ToListAsync();
        }



        public async Task<IdentityResult> ChangeSeat(Seat seat)
        {
            context.Entry(seat).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Description = "Update error" });
        }

        public async Task<IdentityResult> AddSeat(Seat seat)
        {
            context.Seats.Add(seat);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Error when trying to add seat" });
        }

        public async Task<IdentityResult> DeleteSeat(Seat seat)
        {
            context.Seats.Remove(seat);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Add error" });
        }

        public async Task<Seat> GetSeat(int id)
        {
            return await context.Seats.Include(s => s.Flight)
                //.ThenInclude(f => f.From)
                //.ThenInclude(f=>f.To)
                //.ThenInclude(f => f.Stops.Select(d=>d.Destination.City))
                .FirstOrDefaultAsync(s => s.SeatId == id);
        }

        public async Task<IEnumerable<Seat>> GetAllSeats(Flight flight)
        {
            return await context.Seats.Where(s => s.Flight == flight).ToListAsync();
        }

        public async Task<ICollection<Airline>> GetAllAirlines()
        {
            return await context.Airlines.Include(a=>a.Destinations).ThenInclude(d=>d.Destination).Include(a => a.Address).ToListAsync();
        }

        public async Task<Flight> GetAirlineFlightById(int flightId)
        {
            return await context.Flights.FirstOrDefaultAsync(f => f.FlightId == flightId);
        }

        public async Task<Airline> GetAdminAirline(string id)
        {
            return await context.Airlines.Include(a => a.Address).FirstOrDefaultAsync(a => a.AdminId == id);
        }

        public async Task<IdentityResult> UpdateAddress(Address addr)
        {
            context.Entry(addr).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = "Update error" });
        }

        public async Task<IdentityResult> UpdateArline(Airline airline)
        {
            context.Entry(airline).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Description = "Update error" });
        }

        public async Task<Airline> GetAirlineByAdmin(string adminId)
        {
            return await context.Airlines.Include(a => a.Flights).Include(a => a.Address).FirstAsync(a => a.AdminId == adminId);
        }

        public async Task<Flight> GetFlightOfAirlineById(int airlineId)
        {
            return await context.Flights.Include(f => f.Seats).FirstOrDefaultAsync(f => f.AirlineId == airlineId);
        }

        public async Task<IEnumerable<SpecialOffer>> GetSpecialOffersOfAirline(Airline airline)
        {
            return await context.SpecialOffers.Include(s => s.Seats)
                .ThenInclude(seat => seat.Flight)
                .ThenInclude(to => to.To)
                .Include(s => s.Seats)
                .ThenInclude(seat => seat.Flight)
                .ThenInclude(from => from.From)
                .Include(s => s.Seats)
                .ThenInclude(seat => seat.Flight)
                .ThenInclude(h => h.Stops)
                .ThenInclude(st => st.Destination)
                .Where(s => s.Airline == airline).ToListAsync();
        }

        public async Task<IdentityResult> AddSpecOffer(SpecialOffer specOffer)
        {
            context.SpecialOffers.Add(specOffer);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Error when trying to add special offer" });
        }

        public async Task<IdentityResult> DeleteSpecOffer(SpecialOffer specOffer)
        {
            context.SpecialOffers.Remove(specOffer);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Add error" });
        }

        public async Task<SpecialOffer> GetSpecialOfferById(int id)
        {
            return await context.SpecialOffers.FirstOrDefaultAsync(s => s.SpecialOfferId == id);
        }

        public async Task<IdentityResult> UpdateSeat(Seat seat)
        {

            context.Entry(seat).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Update error" });
        }

        public async Task<IEnumerable<Flight>> GetAllFlights()
        {
            return await context.Flights
                .Include(f => f.Seats)
                .Include(f => f.From)
                .Include(f => f.To)
                .Include(f => f.Airline)
                .Include(f => f.Stops)
                .ThenInclude(d => d.Destination)
                .ToListAsync();
        }

        public async Task<IEnumerable<SpecialOffer>> GetAllSpecOffers()
        {
            return await context.SpecialOffers
              .Include(s => s.Seats)
              .ThenInclude(seat => seat.Flight)
              .ThenInclude(to => to.To)
              .Include(s => s.Seats)
              .ThenInclude(seat => seat.Flight)
              .ThenInclude(from => from.From)
              .Include(s => s.Seats)
              .ThenInclude(seat => seat.Flight)
              .ThenInclude(h => h.Stops)
              .ThenInclude(st => st.Destination)
              .Include(s => s.Seats)
              .ThenInclude(seat => seat.Flight)
              .ThenInclude(f => f.Airline)
              .ToListAsync();
        }
     
    }
}
