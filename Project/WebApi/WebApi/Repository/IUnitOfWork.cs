using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IUnitOfWork
    {
        IAuthenticationRepository AuthenticationRepository { get; }
        IAirlineRepository AirlineRepository { get; }
        IDestinationRepository DestinationRepository { get; }
        IRentCarServiceRepository RentACarRepository { get; }
        IUserRepository UserRepository { get; }
        ISeatRepository SeatRepository { get; }
        ISpecialOfferRepository SpecialOfferRepository { get; }
        IFlightRepository FlightRepository { get; }
        IProfileRepository ProfileRepository { get; }
        IBranchRepository BranchRepository { get; }
        IRACSSpecialOffer RACSSpecialOfferRepository { get; }
        ICarRepository CarRepository { get; }
        UserManager<Person> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }

        static DataContext Context { get; }

        void Commit();
        void Rollback();
    }
}
