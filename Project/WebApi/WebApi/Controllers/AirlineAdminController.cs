using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
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

        [HttpGet]
        [Route("get-airline")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetAdminsAirline() 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAdminAirline(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                //return Ok(airline);
                object obj = new 
                {
                    airline.Name,
                    airline.PromoDescription,
                    airline.Address,
                    airline.LogoUrl
                };

                return Ok(obj);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet]
        [Route("get-airline-destinations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAirlineDestinations()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400" , Description = "User not found"});
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Airline not found" });
                }

                var result = await _airlineRepository.GetAirlineDestinations(airline);

                List<object> obj = new List<object>();

                foreach (var item in result)
                {
                    obj.Add(new
                    {
                        city = item.City,
                        state = item.State,
                        destinationId = item.DestinationId,
                        imageUrl = item.ImageUrl
                    });
                }
                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("change-airline-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeAirlineInfo([FromBody]ChangeAirlineInfoDto airlineInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Description = "Airline doesnt exist" });
                }

                airline.Name = airlineInfoDto.Name;
                airline.PromoDescription = airlineInfoDto.PromoDescription;

                bool addressChanged = false;

                if (!airline.Address.City.Equals(airlineInfoDto.Address.City) || !airline.Address.City.Equals(airlineInfoDto.Address.State)
                    || !airline.Address.Lat.Equals(airlineInfoDto.Address.Lat) || !airline.Address.Lon.Equals(airlineInfoDto.Address.Lon))
                {
                    airline.Address.City = airlineInfoDto.Address.City;
                    airline.Address.State = airlineInfoDto.Address.State;
                    airline.Address.Lon = airlineInfoDto.Address.Lon;
                    airline.Address.Lat = airlineInfoDto.Address.Lat;
                    addressChanged = true;
                }

                var result = IdentityResult.Success;
                if (addressChanged)
                {
                    using (var transaction = _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var res = await _airlineRepository.UpdateArline(airline);
                            var res2 = await _airlineRepository.UpdateAddress(airline.Address);

                            await transaction.Result.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.Result.RollbackAsync();
                            throw;
                        }
                    }
                }
                else
                {
                    result = await _airlineRepository.UpdateArline(airline);
                }

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

        [HttpPut]
        [Route("change-airline-logo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeAirlineLogo(IFormFile img)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Airline doesnt exist" });
                }

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    airline.LogoUrl = stream.ToArray();
                }

                var res = await _airlineRepository.UpdateArline(airline);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize(Roles = "AirlineAdmin")]

        [HttpPost]
        [Route("add-flight")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddFlight(FlightDto flightDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var exists = await _airlineRepository.GetFlightByNumber(airline, flightDto.FlightNumber);
                //proveri da li postoji vec takav naziv leta
                if (exists != null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Flight num already exist" });
                }

                var day = Convert.ToDateTime(flightDto.TakeOffDateTime).Day;

                if (Convert.ToDateTime(flightDto.TakeOffDateTime) > Convert.ToDateTime(flightDto.LandingDateTime) || Convert.ToDateTime(flightDto.TakeOffDateTime) < DateTime.Now)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Take of time has to be lower then landing time" });
                }

                var fromIdDest = await _airlineRepository.GetDestination(flightDto.FromId);
                var toIdDest = await _airlineRepository.GetDestination(flightDto.ToId);

                if (fromIdDest == null || toIdDest == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Destination is not on airline" });
                }

                var stops = (await _airlineRepository.GetAirlineDestinations(airline)).Where(s => flightDto.StopIds.Contains(s.DestinationId));

                var flight = new Flight()
                {
                    FlightNumber = flightDto.FlightNumber,
                    TakeOffDateTime = Convert.ToDateTime(flightDto.TakeOffDateTime),
                    LandingDateTime = Convert.ToDateTime(flightDto.LandingDateTime),
                    From = fromIdDest,
                    To = toIdDest,
                    Airline = airline,
                    tripLength = flightDto.TripLength
                };

                flight.TripTime = this.GetLocalDateByCityName(flight.From.City, flight.To.City, flight.TakeOffDateTime, flight.LandingDateTime).ToString();

                flight.Seats = new HashSet<Seat>();

                foreach (var seat in flightDto.Seats)
                {

                    flight.Seats.Add(new Seat() { Column = seat.Column, 
                        Row = seat.Row, Class = seat.Class, Price = seat.Price,
                        Flight = flight, Available = true, Reserved = false, Ticket = null});
                }

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
                    var flights = await _airlineRepository.GetFlightsOfAirline(airline.AirlineId);

                    return Ok(flights);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        private string GetLocalDateByCityName(string departureCity, string arrivalCity, DateTime departureDate, DateTime arrivalDate)
        {
            var timeZoneInfoDeparture = TimeZoneInfo.GetSystemTimeZones()
                        .Where(k => k.DisplayName.Substring(k.DisplayName.IndexOf(')') + 2).ToLower().IndexOf("tokyo") >= 0)
                        .ToList();
            var timeZoneInfoLanding = TimeZoneInfo.GetSystemTimeZones()
            .Where(k => k.DisplayName.Substring(k.DisplayName.IndexOf(')') + 2).ToLower().IndexOf("belgrade") >= 0)
            .ToList();

            if (timeZoneInfoDeparture.Count == 0 || timeZoneInfoLanding.Count == 0)
            {
                return "";
            }

            int departureZone = (int)timeZoneInfoDeparture[0].BaseUtcOffset.TotalHours;
            int landingZone = (int)timeZoneInfoLanding[0].BaseUtcOffset.TotalHours;
            //int year, int month, int day, int hour, int minute, int second

            var day = (arrivalDate.Day - departureDate.Day) * 12;
            int hour;
            if (day > 0)
            {
                hour = Math.Abs(departureDate.Hour + arrivalDate.Hour) + day;
            }
            else 
            {
                hour = Math.Abs(departureDate.Hour - arrivalDate.Hour);
}

            var minutes = Math.Abs(departureDate.Minute - arrivalDate.Minute);

            var offset = departureZone - landingZone;

            string flightTime = hour + offset + "h " + minutes + "min";

            return flightTime;
        }


        [HttpPost]
        [Route("add-destination")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddDestination(DestinationDto destinationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }


                byte[] imageBytes;
                using (var webClient = new WebClient())
                {
                    imageBytes = webClient.DownloadData(destinationDto.ImgUrl);
                }

                var destination = new Destination()
                {
                    City = destinationDto.City,
                    State = destinationDto.State,
                    ImageUrl = imageBytes
                };

                destination.Airlines = new List<AirlineDestionation>
                {
                    new AirlineDestionation
                    {
                        Destination = destination,
                        Airline = airline,
                    }
                };

                var result = await _airlineRepository.AddDestination(destination);

                var allDestinations = await _airlineRepository.GetAirlineDestinations(airline);

                if (result.Succeeded)
                {
                    List<object> obj = new List<object>();

                    foreach (var item in allDestinations)
                    {
                        obj.Add(new
                        {
                            city = item.City,
                            state = item.State,
                            destinationId = item.DestinationId,
                            imageUrl = item.ImageUrl
                        });
                    }
                    return Ok(obj);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        //[Authorize(Roles = "AirlineAdmin")]

        //[HttpDelete]
        //[Route("delete-destination/{id}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        //public async Task<IActionResult> DeleteDestination(int id)
        //{
        //    try
        //    {
        //        string userId = User.Claims.First(c => c.Type == "UserID").Value;

        //        string userRole = User.Claims.First(c => c.Type == "Roles").Value;

        //        if (!userRole.Equals("AirlineAdmin"))
        //        {
        //            return Unauthorized();
        //        }

        //        var user = await _userManager.FindByIdAsync(userId);

        //        if (user == null)
        //        {
        //            return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
        //        }

        //        var destination = await _airlineRepository.GetDestination(id);

        //        if (destination == null)
        //        {
        //            return BadRequest(new IdentityError() { Code = "Destination not found" });
        //        }

        //        var airline = await _airlineRepository.GetAirlineByAdmin(userId);

        //        if (airline == null)
        //        {
        //            return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
        //        }

        //        airline.Destinations.Remove(destination);

        //        var result = await _airlineRepository.UpdateArline(airline);

        //        if (result.Succeeded)
        //        {
        //            var airline = await _airlineRepository.GetAirlineByAdmin(userId);
        //            var dest = await _airlineRepository.GetAirlineDestinations(airline);

        //            List<object> obj = new List<object>();

        //            foreach (var item in dest)
        //            {
        //                obj.Add(new 
        //                {
        //                    city = item.City,
        //                    state = item.State,
        //                    destinationId = item.DestinationId,
        //                    imageUrl = item.ImageUrl
        //                });
        //            }
        //            return Ok(obj);
        //        }

        //        return BadRequest(result.Errors);
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Internal Server error");
        //    }

        //}

        [HttpGet]
        [Route("get-flights")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<IEnumerable<Flight>>> GetFlightsOfAirline()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var flights = await _airlineRepository.GetFlightsOfAirline(airline.AirlineId);

                ICollection<object> flightsObject = new List<object>();

                if (flights != null)
                {

                    foreach (var flight in flights)
                    {
                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                var s = await _airlineRepository.GetDestination(stop.DestinationId);
                                stops.Add(new { s.City });
                            }
                        }


                        flightsObject.Add(new  { 
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = airline.LogoUrl,
                            airlineName = airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops
                        });
                    }
                }
                return Ok(flightsObject);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("get-toprated-airlines")]
        //public async Task<ActionResult<IEnumerable<Airline>>> GetTopRated() 
        public async Task<IActionResult> GetTopRated() 
        {
            var airlines = await _airlineRepository.GetTopRated();
            return Ok(airlines); 
        }


        [HttpDelete]
        [Route("delete-seat/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteSeat(int id)
        {
            try
            {
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var seat = await _airlineRepository.GetSeat(id);

                if (seat == null)
                {
                    return BadRequest(new IdentityError() { Code = "seat not found" });
                }

                var result = await _airlineRepository.DeleteSeat(seat);

                if (result.Succeeded)
                {
                    var seats = await _airlineRepository.GetAllSeats(seat.Flight);

                    List<object> obj = new List<object>();

                    foreach (var item in seats)
                    {
                        obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                    }
                    return Ok(obj);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        [HttpPost]
        [Route("add-seat")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddSeat(SeatDto seatDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var flight = await _airlineRepository.GetAirlineFlightById(seatDto.FlightId);

                if (flight == null)
                {
                    return NotFound(new IdentityError() { Code="404", Description = "Flight not found" });
                }
                var seat = new Seat()
                {
                    Column = seatDto.Column,
                    Row = seatDto.Row,
                    Price = seatDto.Price,
                    Class = seatDto.Class,
                    Reserved = false,
                    Available = true,
                    Flight = flight
                };

                var result = await _airlineRepository.AddSeat(seat);

                if (result.Succeeded)
                {
                    var allSeats = await _airlineRepository.GetAllSeats(flight);
                    List<object> obj = new List<object>();
                    foreach (var item in allSeats)
                    {
                        obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                    }
                    return Ok(obj);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

            throw new NotImplementedException();

        }

        //[Authorize(Roles = "AirlineAdmin")]
        [HttpPut]
        [Route("change-seat/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeSeatPrice(int id, ChangeSeatDto seatDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Seat not found" });
                }

                var seat = await _airlineRepository.GetSeat(id);

                if (seat == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Seat doesnt exist" });
                }

                seat.Price = seatDto.Price;

                var result = await _airlineRepository.ChangeSeat(seat);

                if (result.Succeeded)
                {
                    var seats = await _airlineRepository.GetAllSeats(seat.Flight);

                    List<object> obj = new List<object>();
                    foreach (var item in seats)
                    {
                        obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                    }
                    return Ok(obj);
                }

                return BadRequest(result.Errors);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error!");
            }
        }

        [HttpGet]
        [Route("get-seats/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetSeats(int id) 
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest();
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Seat not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId); //getairlinebyadmin

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var flight = await _airlineRepository.GetAirlineFlightById(id);

                if (flight == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "flight not found" });
                }

                var seats = await _airlineRepository.GetAllSeats(flight);

                List<object> obj = new List<object>();
                foreach (var item in seats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }


        [HttpGet]
        [Route("get-specialoffer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetSpecialOffers() 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Seat not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var specOffers = await _airlineRepository.GetSpecialOffersOfAirline(airline);

                //var airlineName = specOffers.First().Airline.Name;
                //var airlineLogo = specOffers.First().Airline.LogoUrl;
                

                List<object> objs = new List<object>();
                List<object> flights = new List<object>();
                List<object> fstops = new List<object>();


                foreach (var item in specOffers)
                {
                    flights = new List<object>();

                    foreach (var seat in item.Seats)
                    {
                        fstops = new List<object>();

                        foreach (var stop in seat.Flight.Stops)
                        {
                            fstops.Add(new
                            {
                                city = stop.Destination.City
                            });
                        }

                        flights.Add(new { 
                            flightId = seat.Flight.FlightId,
                            flightNumber = seat.Flight.FlightNumber,
                            to = seat.Flight.To.City,
                            from = seat.Flight.From.City,
                            tripLength = seat.Flight.tripLength, 
                            tripTime = seat.Flight.TripTime, 
                            stops = fstops, 
                            landingDate = seat.Flight.LandingDateTime.Date,
                            landingTime = seat.Flight.LandingDateTime.TimeOfDay,
                            takeOffDate = seat.Flight.TakeOffDateTime.Date,
                            takeOffTime = seat.Flight.TakeOffDateTime.TimeOfDay,
                            seat.Class, seat.Column, seat.Row, seat.Price, seat.Reserved, seat.SeatId}
                        );
                    }

                    objs.Add(new { airline.LogoUrl, airline.Name, item.NewPrice, item.OldPrice, item.SpecialOfferId, flights});
                }

                return Ok(objs);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }

        [HttpPost]
        [Route("add-specialoffer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddSpecialOffer([FromBody]SpecialOfferDto specialOfferDto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (specialOfferDto.SeatsIds.Count == 0)
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                List<Seat> seats = new List<Seat>();
                float oldPrice = 0;
                foreach (var seatId in specialOfferDto.SeatsIds)
                {
                    var seatt = await _airlineRepository.GetSeat(seatId);
                    oldPrice += seatt.Price;
                    seats.Add(seatt);
                }

                var specialOffer = new SpecialOffer()
                {
                    Airline = airline,
                    OldPrice = oldPrice,
                    NewPrice = specialOfferDto.NewPrice
                };

                specialOffer.Seats = seats;

                airline.SpecialOffers.Add(specialOffer);

                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _airlineRepository.AddSpecOffer(specialOffer);
                        await _airlineRepository.UpdateArline(airline);

                        foreach (var s in seats)
                        {
                            s.Available = false;
                            await _airlineRepository.UpdateSeat(s);
                        }
                        await transaction.Result.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.Result.RollbackAsync();
                        return StatusCode(500, "Internal server error");
                    }
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("delete-specialoffer/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> DeleteSpecialOffer(int id) 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var airline = await _airlineRepository.GetAirlineByAdmin(userId);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var specOffer = await _airlineRepository.GetSpecialOfferById(id);

                if (specOffer == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Special offer not found" });

                }

                var res = await _airlineRepository.DeleteSpecOffer(specOffer);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // user methods

        [HttpGet]
        [Route("all-airlines")]
        public async Task<IActionResult> AllAirlinesForUser()
        {
            try
            {
                var queryString = Request.Query;
                var sortType = queryString["typename"].ToString();
                var sortByName = queryString["sortbyname"].ToString();
                var sortTypeCity = queryString["typecity"].ToString();
                var sortByCity = queryString["sortbycity"].ToString();

                var airlines = await _airlineRepository.GetAllAirlines();

              
                if (!String.IsNullOrEmpty(sortByCity) && !String.IsNullOrEmpty(sortTypeCity))
                {
                    if (sortType.Equals("ascending"))
                    {
                        airlines.OrderBy(a => a.Address.City);

                    }
                    else
                    {
                        airlines.OrderByDescending(a => a.Address.City);
                    }
                }
                if (!String.IsNullOrEmpty(sortByName) && !String.IsNullOrEmpty(sortType) )
                {
                    if (sortType.Equals("ascending"))
                    {
                        airlines.OrderBy(a => a.Name);
                    }
                    else
                    {
                        airlines.OrderByDescending(a => a.Name);
                    }
                }

                List<object> all = new List<object>();
                List<object> allDest = new List<object>();

                foreach (var item in airlines)
                {
                    allDest = new List<object>();

                    foreach (var dest in item.Destinations)
                    {
                        allDest.Add(new
                        {
                            dest.Destination.City,
                            dest.Destination.State
                        });
                    }
                    all.Add(new
                    {
                        AirlineId = item.AirlineId,
                        City = item.Address.City,
                        State = item.Address.State,
                        //Lat = item.Address.Lat,
                        //Lon = item.Address.Lon,
                        Name = item.Name,
                        Logo = item.LogoUrl,
                        About = item.PromoDescription,
                        Destinations = allDest
                    });
                }

                return Ok(all);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("airline/{id}")]
        public async Task<IActionResult> Airline(int id) 
        {
            try
            {
                var airline = await _airlineRepository.GetAirlineById(id);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                List<object> allDest = new List<object>();


                allDest = new List<object>();

                foreach (var dest in airline.Destinations)
                {
                    allDest.Add(new
                    {
                        dest.Destination.City,
                        dest.Destination.State,
                        dest.Destination.ImageUrl
                    });
                }

                object obj = new
                {
                    AirlineId = airline.AirlineId,
                    City = airline.Address.City,
                    State = airline.Address.State,
                    Lat = airline.Address.Lat,
                    Lon = airline.Address.Lon,
                    Name = airline.Name,
                    Logo = airline.LogoUrl,
                    About = airline.PromoDescription,
                    Destinations = allDest
                };

                return Ok(obj);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("airline-special-offers/{id}")]
        public async Task<IActionResult> AirlineSpecialOffers(int id) 
        {
            try
            {
                var airline = await _airlineRepository.GetAirlineById(id);

                if (airline == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var offers = await _airlineRepository.GetSpecialOffersOfAirline(airline);

                List<object> objs = new List<object>();
                List<object> flights = new List<object>();
                List<object> fstops = new List<object>();


                foreach (var item in offers)
                {
                    flights = new List<object>();

                    foreach (var seat in item.Seats)
                    {
                        fstops = new List<object>();

                        foreach (var stop in seat.Flight.Stops)
                        {
                            fstops.Add(new
                            {
                                city = stop.Destination.City
                            });
                        }

                        flights.Add(new
                        {
                            flightId = seat.Flight.FlightId,
                            flightNumber = seat.Flight.FlightNumber,
                            to = seat.Flight.To.City,
                            from = seat.Flight.From.City,
                            tripLength = seat.Flight.tripLength,
                            tripTime = seat.Flight.TripTime,
                            stops = fstops,
                            landingDate = seat.Flight.LandingDateTime.Date,
                            landingTime = seat.Flight.LandingDateTime.TimeOfDay,
                            takeOffDate = seat.Flight.TakeOffDateTime.Date,
                            takeOffTime = seat.Flight.TakeOffDateTime.TimeOfDay,
                            seat.Class,
                            seat.Column,
                            seat.Row,
                            seat.Price,
                            seat.Reserved,
                            seat.SeatId
                        }
                        );
                    }

                    objs.Add(new { airline.LogoUrl, airline.Name, item.NewPrice, item.OldPrice, item.SpecialOfferId, flights });
                }

                return Ok(objs);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("flights")]
        public async Task<IActionResult> Flights()
        {
            try
            {
                var queryString = Request.Query;
                var tripType = queryString["type"].ToString();
                var y = queryString["dep"].ToString();
                var t = queryString["dep"].ToString().Split(',');

                List<DateTime> departures = new List<DateTime>();
                if (!String.IsNullOrEmpty(queryString["dep"].ToString()))
                {
                    foreach (var item in queryString["dep"].ToString().Split(','))
                    {
                        departures.Add(Convert.ToDateTime(item));
                    }
                }

                List<string> fromList = new List<string>();
                List<string> toList = new List<string>();
                string from = "", to = "";

                if (tripType == "multi")
                {
                    foreach (var item in queryString["from"].ToString().Split(','))
                    {
                        fromList.Add(item);
                    }
                    foreach (var item in queryString["to"].ToString().Split(','))
                    {
                        toList.Add(item);
                    }
                }
                else 
                {
                    from = queryString["from"].ToString();
                    to = queryString["to"].ToString();
                }


                DateTime ret = DateTime.MinValue;
                if (!String.IsNullOrEmpty(queryString["ret"].ToString()))
                {
                    ret = Convert.ToDateTime(queryString["ret"].ToString());
                }
                float minPrice = 0;
                float maxPrice = 3000;

                float.TryParse(queryString["minprice"], out minPrice);
                float.TryParse(queryString["maxprice"], out maxPrice);

                List<int> ids = new List<int>();
                if (!String.IsNullOrEmpty(queryString["air"].ToString()))
                {
                    foreach (var item in queryString["air"].ToString().Split(','))
                    {
                        ids.Add(int.Parse(item));
                    }
                }

                var minDuration = queryString["mind"].ToString();
                var maxDuration = queryString["maxd"].ToString();

                

                var flights = await _airlineRepository.GetAllFlights();

                ICollection<object> flightsObject = new List<object>();
                ICollection<object> twoWayFlights = new List<object>();
                ICollection<object> oneWayFlights = new List<object>();

                ICollection<object> multiWayFlights = new List<object>();



                if (tripType == "one")
                {
                    foreach (var flight in flights)
                    {

                        if (!FilterFromPassed(flight, to, from, ids, minDuration, maxDuration, departures, minPrice, maxPrice))
                        {
                            continue;
                        }

                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                //var s = await _airlineRepository.GetDestination(stop.DestinationId);
                                //stops.Add(new { s.City });
                                stops.Add(new { stop.Destination.City, stop.Destination.State });

                            }
                        }


                        flightsObject.Add(new
                        {
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = flight.Airline.LogoUrl,
                            airlineName = flight.Airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops,
                            minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                        });
                        oneWayFlights.Add(new { flightsObject });
                    }
                    return Ok(oneWayFlights);
                }

                else if (tripType == "two")
                {
                    foreach (var flight in flights)
                    {

                        if (!FilterFromPassed(flight, to, from, ids, minDuration, maxDuration, departures, minPrice, maxPrice))
                        {
                            continue;
                        }

                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                //var s = await _airlineRepository.GetDestination(stop.DestinationId);
                                //stops.Add(new { s.City });
                                stops.Add(new { stop.Destination.City, stop.Destination.State });

                            }
                        }


                        flightsObject.Add(new
                        {
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = flight.Airline.LogoUrl,
                            airlineName = flight.Airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops,
                            minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                        });

                        foreach (var returnFlights in flights)
                        {
                            if (!FilterReturnPassed(flight, from, to, ids, minDuration, maxDuration, ret, minPrice, maxPrice))
                            {
                                continue;
                            }

                            stops = new List<object>();

                            if (flight.Stops != null)
                            {
                                foreach (var stop in flight.Stops)
                                {
                                    //var s = await _airlineRepository.GetDestination(stop.DestinationId);
                                    //stops.Add(new { s.City });
                                    stops.Add(new { stop.Destination.City, stop.Destination.State });

                                }
                            }


                            flightsObject.Add(new
                            {
                                takeOffDate = flight.TakeOffDateTime.Date,
                                landingDate = flight.LandingDateTime.Date,
                                airlineLogo = flight.Airline.LogoUrl,
                                airlineName = flight.Airline.Name,
                                from = flight.From.City,
                                to = flight.To.City,
                                takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                                landingTime = flight.TakeOffDateTime.TimeOfDay,
                                flightTime = flight.TripTime,
                                flightLength = flight.tripLength,
                                flightNumber = flight.FlightNumber,
                                flightId = flight.FlightId,
                                stops = stops,
                                minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                            });

                            twoWayFlights.Add(new { flightsObject });
                            flightsObject = new List<object>();
                            flightsObject.Add(new { flight });

                        }

                    }
                    return Ok(twoWayFlights);
                }
                else 
                {
                    float currentPrice = 0;

                    foreach (var flight in flights)
                    {
                        List<string> tempFrom = fromList;
                        List<string> tempTo = toList;
                        List<DateTime> tempDepartures = departures;
                        currentPrice = 0;

                        if (!FilterMultiPassed(flight, fromList, toList, ids, minDuration, maxDuration, departures, minPrice, maxPrice, currentPrice))
                        {
                            continue;
                        }

                        currentPrice += flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price;

                        List<object> stops = new List<object>();

                        if (flight.Stops != null)
                        {
                            foreach (var stop in flight.Stops)
                            {
                                //var s = await _airlineRepository.GetDestination(stop.DestinationId);
                                //stops.Add(new { s.City });
                                stops.Add(new { stop.Destination.City, stop.Destination.State });

                            }
                        }


                        flightsObject.Add(new
                        {
                            takeOffDate = flight.TakeOffDateTime.Date,
                            landingDate = flight.LandingDateTime.Date,
                            airlineLogo = flight.Airline.LogoUrl,
                            airlineName = flight.Airline.Name,
                            from = flight.From.City,
                            to = flight.To.City,
                            takeOffTime = flight.TakeOffDateTime.TimeOfDay,
                            landingTime = flight.TakeOffDateTime.TimeOfDay,
                            flightTime = flight.TripTime,
                            flightLength = flight.tripLength,
                            flightNumber = flight.FlightNumber,
                            flightId = flight.FlightId,
                            stops = stops,
                            minPrice = flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                        });

                        tempFrom.Remove(flight.From.City);
                        tempTo.Remove(flight.To.City);
                        tempDepartures.Remove(flight.TakeOffDateTime.Date);

                        foreach (var returnFlights in flights)
                        {
                            if (tempFrom.Count == 0)
                            {
                                break;
                            }
                            if (!FilterMultiPassed(returnFlights, tempFrom, tempTo, ids, minDuration, maxDuration, tempDepartures, minPrice, maxPrice, currentPrice))
                            {


                                continue;
                            }
                            currentPrice += returnFlights.Seats.OrderBy(s => s.Price).FirstOrDefault().Price;

                            tempFrom.Remove(returnFlights.From.City);
                            tempTo.Remove(returnFlights.To.City);
                            tempDepartures.Remove(returnFlights.TakeOffDateTime.Date);

                            stops = new List<object>();

                            if (returnFlights.Stops != null)
                            {
                                foreach (var stop in returnFlights.Stops)
                                {
                                    //var s = await _airlineRepository.GetDestination(stop.DestinationId);
                                    //stops.Add(new { s.City });
                                    stops.Add(new { stop.Destination.City, stop.Destination.State });

                                }
                            }


                            flightsObject.Add(new
                            {
                                takeOffDate = returnFlights.TakeOffDateTime.Date,
                                landingDate = returnFlights.LandingDateTime.Date,
                                airlineLogo = returnFlights.Airline.LogoUrl,
                                airlineName = returnFlights.Airline.Name,
                                from = returnFlights.From.City,
                                to = returnFlights.To.City,
                                takeOffTime = returnFlights.TakeOffDateTime.TimeOfDay,
                                landingTime = returnFlights.TakeOffDateTime.TimeOfDay,
                                flightTime = returnFlights.TripTime,
                                flightLength = returnFlights.tripLength,
                                flightNumber = returnFlights.FlightNumber,
                                flightId = returnFlights.FlightId,
                                stops = stops,
                                minPrice = returnFlights.Seats.OrderBy(s => s.Price).FirstOrDefault().Price
                            });

                            multiWayFlights.Add(new { flightsObject });
                            flightsObject = new List<object>();
                            flightsObject.Add(new { flight });

                        }

                    }
                    if (multiWayFlights.Count == fromList.Count)
                    {
                        return Ok(multiWayFlights);
                    }
                    return Ok();
                }
                

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("flight-seats/{id}")]
        public async Task<IActionResult> FlightSeats(int id) 
        {
            try
            {
                var flight = await _airlineRepository.GetAirlineFlightById(id);

                if (flight == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "flight not found" });
                }

                var seats = await _airlineRepository.GetAllSeats(flight);

                List<object> obj = new List<object>();
                foreach (var item in seats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("all-airlines-specialoffers")]
        public async Task<IActionResult> AllSpecOffers() 
        {
            try
            {
                var specOffers = await _airlineRepository.GetAllSpecOffers();

                List<object> objs = new List<object>();
                List<object> flights = new List<object>();
                List<object> fstops = new List<object>();


                foreach (var item in specOffers)
                {
                    flights = new List<object>();

                    foreach (var seat in item.Seats)
                    {
                        fstops = new List<object>();

                        foreach (var stop in seat.Flight.Stops)
                        {
                            fstops.Add(new
                            {
                                city = stop.Destination.City
                            });
                        }

                        flights.Add(new
                        {
                            flightId = seat.Flight.FlightId,
                            flightNumber = seat.Flight.FlightNumber,
                            to = seat.Flight.To.City,
                            from = seat.Flight.From.City,
                            airlineName = seat.Flight.Airline.Name,
                            airlineLogo = seat.Flight.Airline.LogoUrl,
                            tripLength = seat.Flight.tripLength,
                            tripTime = seat.Flight.TripTime,
                            stops = fstops,
                            landingDate = seat.Flight.LandingDateTime.Date,
                            landingTime = seat.Flight.LandingDateTime.TimeOfDay,
                            takeOffDate = seat.Flight.TakeOffDateTime.Date,
                            takeOffTime = seat.Flight.TakeOffDateTime.TimeOfDay,
                            seat.Class,
                            seat.Column,
                            seat.Row,
                            seat.Price,
                            seat.Reserved,
                            seat.SeatId
                        }
                        );
                    }

                    objs.Add(new { item.Airline.Name, item.Airline.LogoUrl, item.NewPrice, item.OldPrice, item.SpecialOfferId, flights });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        private bool FilterPassed(Flight flight,string tripType, string to, string from,
            List<string> fromMulti, List<string> toMulti, List<int> airlines, DateTime returnDate,
            string minDuration , string maxDuration, List<DateTime> departures, float minPrice, float maxPrice) 
        {
            switch (tripType)
            {
                case "one":
                    if (!flight.From.City.Equals(from) || !flight.To.City.Equals(to))
                    {
                        return false;
                    }
                    if (airlines.Count > 0)
                    {
                        if (!airlines.Contains(flight.AirlineId))
                        {
                            return false;
                        }
                    }
                    if (departures[0].Date != flight.TakeOffDateTime.Date)
                    {
                        return false;
                    }
                    if ( minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
                    {
                        return false;
                    }
                    if (!String.IsNullOrEmpty(minDuration) || !String.IsNullOrEmpty(maxDuration))
                    {
                        if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                            || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) < int.Parse(minDuration.Split('h', ' ', 'm')[1])
                            || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                            || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) > int.Parse(maxDuration.Split('h', ' ', 'm')[1]))
                        {
                            return false;
                        }
                    }
                    return true;
                    break;

                case "two":
                    bool passed = false;
                    if (flight.From.City.Equals(from) && flight.To.City.Equals(to) && departures[0].Date == flight.TakeOffDateTime.Date)
                    {
                        passed = true;
                    }
                    if (flight.To.City.Equals(from) && flight.From.City.Equals(to) && returnDate.Date == flight.TakeOffDateTime.Date)
                    {
                        passed = true;
                    }

                    if (!passed)
                    {
                        return false;
                    }

                    if (airlines.Count > 0)
                    {
                        if (!airlines.Contains(flight.AirlineId))
                        {
                            return false;
                        }
                    }

                    if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
                    {
                        return false;
                    }
                    if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) < int.Parse(minDuration.Split('h', ' ', 'm')[1])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) > int.Parse(maxDuration.Split('h', ' ', 'm')[1]))
                    {
                        return false;
                    }

                    return true;
                    break;

                case "multi":

                    bool pass = false;

                    for (int i = 0; i < fromMulti.Count; i++)
                    {
                        if (flight.From.City.Equals(fromMulti[i]) && flight.To.City.Equals(toMulti[i]) && departures[i].Date == flight.TakeOffDateTime.Date)
                        {
                            pass = true;
                            break;
                        }
                    }
                    if (!pass)
                    {
                        return false;
                    }
                    if (airlines.Count > 0)
                    {
                        if (!airlines.Contains(flight.AirlineId))
                        {
                            return false;
                        }
                    }
                    if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
                    {
                        return false;
                    }
                    if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) < int.Parse(minDuration.Split('h', ' ', 'm')[1])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                        || int.Parse(flight.TripTime.Split('h', ' ', 'm')[1]) > int.Parse(maxDuration.Split('h', ' ', 'm')[1]))
                    {
                        return false;
                    }

                    return true;

                    break;
                default:
                    break;
            }

            return false;
        }


        private bool FilterFromPassed(Flight flight, string to, string from, List<int> airlines,
            string minDuration, string maxDuration, List<DateTime> departures, float minPrice, float maxPrice) 
        {
            if (!flight.From.City.Equals(from) || !flight.To.City.Equals(to))
            {
                return false;
            }
            if (airlines.Count > 0)
            {
                if (!airlines.Contains(flight.AirlineId))
                {
                    return false;
                }
            }
            if (departures[0].Date != flight.TakeOffDateTime.Date)
            {
                return false;
            }
            if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(minDuration) || !String.IsNullOrEmpty(maxDuration))
            {
                if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) < int.Parse(minDuration.Split('h', ' ', 'm')[2])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) > int.Parse(maxDuration.Split('h', ' ', 'm')[2]))
                {
                    return false;
                }
            }
            return true;
        }

        private bool FilterReturnPassed(Flight flight, string from, string to, List<int> airlines,
          string minDuration, string maxDuration, DateTime departure, float minPrice, float maxPrice)
        {
            if (!flight.From.City.Equals(from) || !flight.To.City.Equals(to))
            {
                return false;
            }
            if (airlines.Count > 0)
            {
                if (!airlines.Contains(flight.AirlineId))
                {
                    return false;
                }
            }
            if (departure.Date != flight.TakeOffDateTime.Date)
            {
                return false;
            }
            if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price > maxPrice)
            {
                return false;
            }
            if (!String.IsNullOrEmpty(minDuration) || !String.IsNullOrEmpty(maxDuration))
            {
                if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) < int.Parse(minDuration.Split('h', ' ', 'm')[2])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                    || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) > int.Parse(maxDuration.Split('h', ' ', 'm')[2]))
                {
                    return false;
                }
            }
            return true;
        }


        private bool FilterMultiPassed(Flight flight, List<string> fromMulti, List<string> toMulti, List<int> airlines,
           string minDuration, string maxDuration, List<DateTime> departures, float minPrice, float maxPrice, float currentFlightsPrice)
        {
            bool pass = false;



            if (fromMulti.Contains(flight.From.City) && toMulti.Contains(flight.To.City))
            {
            }
            else 
            {
                return false;
            }

            if ( !departures.Contains(flight.TakeOffDateTime.Date))
            {
                return false;
            }

            if (airlines.Count > 0)
            {
                if (!airlines.Contains(flight.AirlineId))
                {
                    return false;
                }
            }
            if (minPrice > flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price + currentFlightsPrice 
                || flight.Seats.OrderBy(s => s.Price).FirstOrDefault().Price + currentFlightsPrice > maxPrice)
            {
                return false;
            }
            if (int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) < int.Parse(minDuration.Split('h', ' ', 'm')[0])
                || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) < int.Parse(minDuration.Split('h', ' ', 'm')[2])
                || int.Parse(flight.TripTime.Split('h', ' ', 'm')[0]) > int.Parse(maxDuration.Split('h', ' ', 'm')[0])
                || int.Parse(flight.TripTime.Split('h', ' ', 'm')[2]) > int.Parse(maxDuration.Split('h', ' ', 'm')[2]))
            {
                return false;
            }

            return true;
        }
    }
}
