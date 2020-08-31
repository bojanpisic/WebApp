using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class FlightReservationRepository : GenericRepository<FlightReservation>, IFlightReservationRepository
    {
        public FlightReservationRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FlightReservation>> GetTrips(User user)
        {
            return await context.FlightReservations
                .Include(f => f.Tickets)
                    .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.From)
                .Include(f => f.Tickets)
                    .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.To)
                .Include(f => f.Tickets)
                    .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Flight)
                    .ThenInclude(ff => ff.Airline)
                    .ThenInclude(a => a.Rates)
                    .ThenInclude(r => r.User)
                .Include(f => f.Tickets)
                    .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.Stops)
                    .ThenInclude(d => d.Destination)
                .Include(f => f.Tickets)
                    .ThenInclude(t => t.Seat)
                    .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.Rates)
                    .ThenInclude(r => r.User)
                .Where(t => t.User == user)
                .ToListAsync();
        }

        public async Task<FlightReservation> GetReservationById(int flightReservationId)
        {
            return await context.FlightReservations
                .Include(f => f.Tickets)
                .ThenInclude(t => t.Seat)
                .ThenInclude(s => s.Flight)
                .Include(t => t.CarRent)
                .ThenInclude(c => c.RentedCar)
                .FirstOrDefaultAsync(t => t.FlightReservationId == flightReservationId);
        }
    }
}
