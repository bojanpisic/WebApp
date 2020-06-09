using Microsoft.AspNetCore.Identity;
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

            return IdentityResult.Failed(new IdentityError() { Code = "Error when trying to add flight"});
        }

        public async Task<Flight> GetFlightByNumber(int airlineId, string flightNum)
        {
            return await context.Flights.FirstAsync(f => f.AirlineId == airlineId && f.FlightNumber == flightNum);
        }

        public async Task<IdentityResult> ChangeAirlineInfo(int id, Airline airline)
        {
            context.Entry(airline).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = "Update error" });
        }

        public async Task<IdentityResult> ChangeAirlineLogo(int id, Airline airline)
        {
            context.Entry(airline).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = "Update error"});
        }

        public async Task<ICollection<Destination>> GetAirlineDestinations(int airlineId)
        {
            var airline = await this.GetAirlineById(airlineId);
            return await context.Destinations.Where(dest => dest.Airlines == airline).ToListAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<Airline> GetAirlineById(int airlineId)
        {
            return await context.Airlines.FirstAsync(airline => airline.AirlineId == airlineId);
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

            return IdentityResult.Failed(new IdentityError() { Code = "Add error"});
        }

        public async Task<IdentityResult> RemoveDestination(Destination destination)
        {
            context.Destinations.Remove(destination);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Code = "Add error" });
        }

        public async Task<IEnumerable<Flight>> GetFlightsOfAirline(int airlineId)
        {
            return await context.Flights.Where( f => f.AirlineId == airlineId).ToListAsync();
        }


        //public async Task<ICollection<Destination>> GetDestinations(DataContext context = null)
        //{
        //    return await context.Destinations.ToListAsync();
        //}
    }
}
