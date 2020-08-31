using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Flight>> GetAirlineFlights(int airlineId)
        {
            return await context.Flights.Where(f => f.AirlineId == airlineId)
                .Include(f => f.Seats)
                .Include(f => f.From)
                .Include(f => f.To)
                .Include(f => f.Airline)
                .Include(f => f.Stops)
                .ThenInclude(d => d.Destination)
                .ToListAsync();
        }

        public async Task<IEnumerable<Flight>> GetAllFlightsWithAllProp()
        {
            return await context.Flights
                .Include(f => f.Seats)
                .Include(f => f.From)
                .Include(f => f.To)
                .Include(f => f.Airline)
                .Include(f => f.Stops)
                .ThenInclude(d => d.Destination)
                .Where(f => f.TakeOffDateTime >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<IEnumerable<Flight>> GetFlights(List<string> ids)
        {
            return await context.Flights
              .Include(f => f.Seats)
              .Include(f => f.From)
              .Include(f => f.To)
              .Include(f => f.Airline)
              .Include(f => f.Stops)
              .ThenInclude(d => d.Destination)
              .Where(f => ids.Contains(f.FlightId.ToString()) && f.TakeOffDateTime >= DateTime.Now)
              .ToListAsync();
        }
    }
}
