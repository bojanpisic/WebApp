using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirlineAdminController : Controller
    {
        private readonly IAirlineRepository _airlineRepository;
        private readonly IAuthenticationRepository _authenticationRepository;

        private readonly DataContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AirlineAdminController(DataContext dbContext, UserManager<Person> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _airlineRepository = new AirlineRepository(dbContext, userManager);
            _authenticationRepository = new AuthenticationRepository(dbContext, userManager, roleManager);
        }

        [Authorize(Roles = "AirlineAdmin")]

        [HttpPut]
        [Route("change-airline-info/{id}")]

        public async Task<IActionResult> ChangeAirlineInfo(int id, ChangeAirlineInfoDto airlineInfoDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var airline = await _airlineRepository.GetAirlineById(id);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Code = "Airline doesnt exist" });
                }

                airline.Name = airlineInfoDto.Name;
                airline.PromoDescription = airlineInfoDto.PromoDescription;
                airline.Address = airlineInfoDto.Address;

                var result = await _airlineRepository.ChangeAirlineInfo(airline.AirlineId, airline);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error!");
            }
        }

        [Authorize(Roles = "AirlineAdmin")]
        [HttpPut]
        [Route("change-airline-logo/{id}")]

        public async Task<IActionResult> ChangeAirlineLogo(int id, ChangeAirlineLogoDto logoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var airline = await _airlineRepository.GetAirlineById(id);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Code = "Airline doesnt exist"});
                }

                airline.LogoUrl = logoDto.ImgUrl;

                var result = await _airlineRepository.ChangeAirlineLogo(id, airline);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error!");
            }
        }

        [Authorize(Roles = "AirlineAdmin")]

        [HttpPost]
        [Route("add-flight")]

        public async Task<IActionResult> AddFlight(FlightDto flightDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var admin = await _authenticationRepository.GetUserById(flightDto.AdminId.ToString());

                if (admin == null)
                {
                    return BadRequest(new IdentityError() { Code = "Admin doesnt exist" });
                }

                var airline = await _airlineRepository.GetAirlineById(flightDto.ArilineId);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Code = "Airline doesnt exist" });

                }

                //proveri da li postoji vec takav naziv leta
                if (await _airlineRepository.GetFlightByNumber(airline.AirlineId, flightDto.FlightNumber) != null)
                {
                    return BadRequest(new IdentityError() { Code = "Flight number already exist" });
                }

                var fromIdDest = await _airlineRepository.GetDestination(flightDto.FromId);
                var toIdDest = await _airlineRepository.GetDestination(flightDto.ToId);

                if (fromIdDest == null || toIdDest == null)
                {
                    return BadRequest(new IdentityError() { Code = "Destination doesnt exist" });
                }

                var stops = (await _airlineRepository.GetAirlineDestinations(airline.AirlineId)).Where(s => flightDto.StopIds.Contains(s.DestinationId));

                var flight = new Flight()
                {
                    FlightNumber = flightDto.FlightNumber,
                    Seats = flightDto.Seats,
                    TakeOffDateTime = flightDto.TakeOffDateTime,
                    LandingDateTime = flightDto.LandingDateTime,
                    From = fromIdDest,
                    To = toIdDest,
                    Airline = airline,
                };
                foreach (var stop in stops)
                {
                    //flight.Stops.Add(new FlightDestination() { Destination = stop, Flight = flight });
                    flight.Stops = new List<FlightDestination>
                    {
                        new FlightDestination{
                            Flight = flight,
                            Destination = stop
                        }
                    };
                }

                var result = await _airlineRepository.AddFlight(flight);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        [Authorize(Roles = "AirlineAdmin")]

        [HttpPost]
        [Route("add-destination")]

        public async Task<IActionResult> AddDestination(DestinationDto destinationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var airline = await _airlineRepository.GetAirlineById(destinationDto.AirlineId);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Code = "Airline not found"});
                }
                var destination = new Destination()
                {
                    City = destinationDto.City,
                    State = destinationDto.State,
                };



                destination.Airlines = new List<AirlineDestionation>
                {
                    new AirlineDestionation
                    {
                        Destination = destination,
                        Airline = airline
                    }
                };

                var result = await _airlineRepository.AddDestination(destination);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        [Authorize(Roles = "AirlineAdmin")]

        [HttpPut]
        [Route("delete-destination/{id}")]

        public async Task<IActionResult> AddDestination(int id)
        {
            try
            {
                var destination = await _airlineRepository.GetDestination(id);

                if (destination == null)
                {
                    return BadRequest(new IdentityError() { Code = "Destination not found" });
                }

                var result = await _airlineRepository.RemoveDestination(destination);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get-flights/{id}")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightsOfAirline(int id) 
        {
            try
            {
                var flights = await _airlineRepository.GetFlightsOfAirline(id);

                return Ok(flights);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
                throw;
            }
        }
    }
}
