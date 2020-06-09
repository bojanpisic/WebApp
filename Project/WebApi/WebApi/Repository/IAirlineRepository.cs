using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IAirlineRepository : IDisposable
    {
        //menja info slika ime, adresa, opis
        Task<IdentityResult> ChangeAirlineInfo(int id, Airline airline);
        Task<IdentityResult> ChangeAirlineLogo(int id, Airline airline);
        Task<Airline> GetAirlineById(int airlineId);
        Task<Flight> GetFlightByNumber(int airlineId, string flightNum);
        Task<ICollection<Destination>> GetAirlineDestinations(int airlineId);
        Task<Destination> GetDestination(int destId);

        //dodaje nove letove
        Task<IdentityResult> AddFlight(Flight flight);
        Task<IdentityResult> AddDestination(Destination destination);
        Task<IdentityResult> RemoveDestination(Destination destination);
        Task<IEnumerable<Flight>> GetFlightsOfAirline(int airlineId);



        //menja sedista letova
        //Task<IdentityResult> ChangeSeat();
        //dodaje spec ponude i brise ih
        //Task<IdentityResult> AddSpecOffer();
        //Task<IdentityResult> DeleteSpecOffer();
        //pregled statistike

        //[regled spec ponuda i letova


    }
}
