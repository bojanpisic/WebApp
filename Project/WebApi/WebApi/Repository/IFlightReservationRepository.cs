using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IFlightReservationRepository: IGenericRepository<FlightReservation>
    {
        Task<IEnumerable<FlightReservation>> GetTrips(User user);
        Task<FlightReservation> GetReservationById(int flightReservationId);
    }
}
