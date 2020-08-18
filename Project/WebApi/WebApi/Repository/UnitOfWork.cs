using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private DataContext context;
        private readonly UserManager<Person> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<Person> signInManager;

        private IAirlineRepository airlineRepository;
        private IAuthenticationRepository authenticationRepository;
        private IFlightRepository flightRepository;
        private IRentCarServiceRepository rentRepository;
        private IUserRepository userRepository;
        private IDestinationRepository destinationRepository;
        private ISeatRepository seatRepository;
        private ISpecialOfferRepository specialOfferRepository;
        private IProfileRepository profileRepository;
        private ICarRepository carRepository;
        private IBranchRepository branchRepository;
        private IRACSSpecialOffer racsSpecialOfferRepository;
        private ICarRentRepository carRentRepository;

        public UnitOfWork(DataContext _context, RoleManager<IdentityRole> _roleManager, 
            UserManager<Person> _userManager, SignInManager<Person> _signInManager)
        {
            this.context = _context;
            this.roleManager = _roleManager;
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }

        public IAirlineRepository AirlineRepository
        {
            get
            {
                return airlineRepository = airlineRepository ?? new AirlineRepository(this.context); 
            }
        }

        public IAuthenticationRepository AuthenticationRepository
        {
            get
            {
                return authenticationRepository = authenticationRepository ??
                    new AuthenticationRepository(this.userManager, this.roleManager);
            }
        }

        public IFlightRepository FlightRepository
        {
            get
            {
                return flightRepository = flightRepository ?? new FlightRepository(this.context);
            }
        }

        public ISeatRepository SeatRepository
        {
            get
            {

                return seatRepository = seatRepository ?? new SeatRepository(this.context);

            }
        }

        public UserManager<Person> UserManager 
        {
            get => this.userManager;
        }

        public RoleManager<IdentityRole> RoleManager 
        {
            get => this.roleManager;
        }

        public IDestinationRepository DestinationRepository
        {
            get
            {
                return destinationRepository = destinationRepository ?? new DestinationRepository(this.context);
            }
        }

        public IRentCarServiceRepository RentACarRepository
        {
            get
            {
                return rentRepository = rentRepository ?? new RentCarServiceRepository(this.context);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return userRepository = userRepository ?? new UserRepository(this.context);
            }
        }
        public ISpecialOfferRepository SpecialOfferRepository
        {
            get
            {
                return specialOfferRepository = specialOfferRepository ?? new SpecialOfferRepository(this.context);
            }
        }
        public IProfileRepository ProfileRepository
        {
            get
            {
                return profileRepository = profileRepository ?? new ProfileRepository(this.context, this.userManager, this.signInManager);
            }
        }
        public IBranchRepository BranchRepository
        {
            get
            {
                return branchRepository = branchRepository ?? new BranchRepository(this.context);
            }
        }
        public ICarRepository CarRepository
        {
            get
            {
                return carRepository = carRepository ?? new CarRepository(this.context);
            }
        }
        public IRACSSpecialOffer RACSSpecialOfferRepository
        {
            get
            {
                return racsSpecialOfferRepository = racsSpecialOfferRepository ?? new RACSSpecialOfferRepository(this.context);
            }
        }

        public ICarRentRepository CarRentRepository
        {
            get
            {
                return carRentRepository = carRentRepository ?? new CarRentRepository(this.context);
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Commit()
        {
            await context.SaveChangesAsync();
        }

        public void Rollback()
        {
            this.Dispose();
        }
    }
}
