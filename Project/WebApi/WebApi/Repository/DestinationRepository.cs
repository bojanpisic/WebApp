using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class DestinationRepository : GenericRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Destination>> GetAirlineDestinations(Airline airline)
        {
            //return await context.Destinations.Where(d => d.Airlines.Any( a=> a.Airline == airline)).ToListAsync(); 
            return await context.AirlineDestination.Where(ad => ad.AirlineId == airline.AirlineId).Select(ad => ad.Destination).ToListAsync();
        }
    }
}
