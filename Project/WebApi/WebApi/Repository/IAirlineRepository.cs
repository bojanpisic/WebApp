using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        //Task<IdentityResult> AddAirlinetoAdmin(AirlineAdmin admin, Airline airline);
        Task<Airline> GetAirlineByAdmin(string adminId);
        Task<Airline> GetAdminAirline(string id);
        Task<ICollection<Airline>> GetAllAirlines();
        Task<IdentityResult> UpdateArline(Airline airline);
        Task<IdentityResult> UpdateAddress(Address addr);
        Task<Airline> GetAirlineById(int airlineId);

        Task<Flight> GetFlightByNumber(Airline airline, string flightNum);
        Task<IdentityResult> AddFlight(Flight flight);
        Task<IEnumerable<Flight>> GetFlightsOfAirline(int airlineId);
        Task<Flight> GetAirlineFlightById(int flightId);
        Task<ICollection<Destination>> GetAirlineDestinations(Airline airline);
        Task<Destination> GetDestination(int destId);
        Task<IdentityResult> AddDestination(Destination destination);
        //Task<IdentityResult> RemoveDestination(Destination destination);

        Task<IEnumerable<Airline>> GetTopRated();

        Task<IdentityResult> ChangeSeat(Seat seat);
        Task<IdentityResult> AddSeat(Seat seat); // nije odradjeno
        Task<IdentityResult> DeleteSeat(Seat seat);
        Task<Seat> GetSeat(int id);
        Task<IEnumerable<Seat>> GetAllSeats(Flight flight);

        Task<IdentityResult> UpdateSeat(Seat seat);
        Task<IEnumerable<SpecialOffer>> GetSpecialOffersOfAirline(Airline airline);
        Task<IdentityResult> AddSpecOffer(SpecialOffer specOffer);
        Task<IdentityResult> DeleteSpecOffer(SpecialOffer specOffer);
        Task<SpecialOffer> GetSpecialOfferById(int id);

        Task<IEnumerable<Flight>> GetAllFlights();
        Task<IEnumerable<SpecialOffer>> GetAllSpecOffers();
        //pregled statistike
    }
}
