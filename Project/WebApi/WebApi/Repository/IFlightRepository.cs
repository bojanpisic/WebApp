using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<IEnumerable<Flight>> GetAirlineFlights(int airlineId);
        Task<IEnumerable<Flight>> GetAllFlightsWithAllProp();
    }
}
