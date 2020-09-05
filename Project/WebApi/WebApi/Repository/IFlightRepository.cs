using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<IEnumerable<Flight>> GetAirlineFlights(int airlineId);
        Task<IEnumerable<Flight>> GetAllFlightsWithAllProp(Expression<Func<Flight, bool>> filter = null);
        Task<IEnumerable<Flight>> GetFlights(List<string> ids);


    }
}
