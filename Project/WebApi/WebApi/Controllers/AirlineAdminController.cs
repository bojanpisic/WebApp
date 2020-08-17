using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        private IUnitOfWork unitOfWork;

        public AirlineAdminController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
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
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId, null, "Flights,Address");

                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

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
                return StatusCode(500, "Failed to return airline.");
            }
        }



        #region Change airline info methods

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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var findRes = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId, null, "Address");
                var airline = findRes.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found.");
                }

                airline.Name = airlineInfoDto.Name;
                airline.PromoDescription = airlineInfoDto.PromoDescription;

                bool addressChanged = false;

                if (!airline.Address.City.Equals(airlineInfoDto.Address.City) || 
                    !airline.Address.State.Equals(airlineInfoDto.Address.State)
                    || !airline.Address.Lat.Equals(airlineInfoDto.Address.Lat) || 
                    !airline.Address.Lon.Equals(airlineInfoDto.Address.Lon))
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
                    //using (var transaction = new TransactionScope())
                    //{
                        try
                        {
                            unitOfWork.AirlineRepository.Update(airline);
                            //unitOfWork.Commit();

                            unitOfWork.AirlineRepository.UpdateAddress(airline.Address);
                            await unitOfWork.Commit();

                            //transaction.Complete();
                        }
                        catch (Exception)
                        {
                        //unitOfWork.Rollback();
                        //transaction.Dispose();
                        return StatusCode(500, "Failed to change airline info. One of transactions failed.");
                        }
                    //}
                }
                else
                {
                    try
                    {
                        unitOfWork.AirlineRepository.Update(airline);
                        await unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        //unitOfWork.Rollback();
                        return StatusCode(500, "Failed to change airline info. Transaction failed.");
                    }
        }

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to change airline info");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var findResult = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId);
                var airline = findResult.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline doesnt exist");
                }

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    airline.LogoUrl = stream.ToArray();
                }

                try
                {
                    unitOfWork.AirlineRepository.Update(airline);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to change. Transaction failed.");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to change.");
            }
        }

        #endregion

        #region Flight methods

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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId);

                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                var exists = await unitOfWork.FlightRepository.Get(f => f.Airline == airline && 
                                                                    f.FlightNumber == flightDto.FlightNumber);

                //var exists = await unitOfWork.AirlineRepository.GetFlightByNumber(airline, flightDto.FlightNumber);
                //proveri da li postoji vec takav naziv leta
                if (exists.ToList().Count > 0)
                {
                    return BadRequest("Flight num already exist");
                }

                var day = Convert.ToDateTime(flightDto.TakeOffDateTime).Day;

                if (Convert.ToDateTime(flightDto.TakeOffDateTime) > Convert.ToDateTime(flightDto.LandingDateTime) 
                    || Convert.ToDateTime(flightDto.TakeOffDateTime) < DateTime.Now)
                {
                    return BadRequest( "Take of time has to be lower then landing time");
                }

                var fromIdDest = await unitOfWork.DestinationRepository.GetByID(flightDto.FromId);
                var toIdDest = await unitOfWork.DestinationRepository.GetByID(flightDto.ToId);

                if (fromIdDest == null || toIdDest == null)
                {
                    return BadRequest("Destination is not on airline");
                }

                var stops = (await unitOfWork.DestinationRepository.GetAirlineDestinations(airline))
                                   .Where(s => flightDto.StopIds.Contains(s.DestinationId));

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

                flight.TripTime = this.GetLocalDateByCityName(flight.From.City, flight.To.City, 
                                                              flight.TakeOffDateTime, flight.LandingDateTime)
                                                              .ToString();

                flight.Seats = new HashSet<Seat>();

                foreach (var seat in flightDto.Seats)
                {
                    flight.Seats.Add(new Seat()
                        { Column = seat.Column, 
                        Row = seat.Row, Class = seat.Class, Price = seat.Price,
                        Flight = flight, Available = true, Reserved = false,
                        Ticket = null});
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
                try
                {
                    await unitOfWork.FlightRepository.Insert(flight);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to add flight.");
                }

                var flights = await unitOfWork.FlightRepository.Get(f => f.AirlineId == airline.AirlineId);

                return Ok(flights);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to add flight.");
            }

        }

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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var findRes = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId);

                var airline = findRes.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                var flights = await unitOfWork.FlightRepository.GetAirlineFlights(airline.AirlineId); // ne moze get jer treba uvesti i stops -> destinations

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
                                //var s = await unitOfWork.AirlineRepository.GetDestination(stop.DestinationId);
                                stops.Add(new { stop.Destination.City });
                            }
                        }


                        flightsObject.Add(new
                        {
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
                return StatusCode(500, "Failed to return flights.");
                throw;
            }
        }

        #endregion

        #region Destination methods
        [HttpGet]
        [Route("get-airline-destinations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAirlineDestinations()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("AirlineAdmin"))
                {
                    return Unauthorized();
                }

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId, null, "Flights,Address");
                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                var result = await unitOfWork.DestinationRepository.GetAirlineDestinations(airline);

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
                return StatusCode(500, "Failed to return destinations.");
            }
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId);
                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
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
                try
                {
                    await unitOfWork.DestinationRepository.Insert(destination);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to add destination.");
                }

                var allDestinations = await unitOfWork.DestinationRepository.GetAirlineDestinations(airline);

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
            catch (Exception)
            {
                return StatusCode(500, "Failed to add destination.");
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

        //        var user = await unitOfWork.UserManager.FindByIdAsync(userId);

        //        if (user == null)
        //        {
        //            return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
        //        }

        //        var destination = await unitOfWork.AirlineRepository.GetDestination(id);

        //        if (destination == null)
        //        {
        //            return BadRequest(new IdentityError() { Code = "Destination not found" });
        //        }

        //        var airline = await unitOfWork.AirlineRepository.GetAirlineByAdmin(userId);

        //        if (airline == null)
        //        {
        //            return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
        //        }

        //        airline.Destinations.Remove(destination);

        //        var result = await unitOfWork.AirlineRepository.UpdateArline(airline);

        //        if (result.Succeeded)
        //        {
        //            var airline = await unitOfWork.AirlineRepository.GetAirlineByAdmin(userId);
        //            var dest = await unitOfWork.AirlineRepository.GetAirlineDestinations(airline);

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

        #endregion

        #region Seat methods

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

                var res = await unitOfWork.SeatRepository.Get(s => s.SeatId == id, null, "Flight");

                var seat = res.FirstOrDefault();

                if (seat == null)
                {
                    return NotFound("Seat not found");
                }
                try
                {
                    unitOfWork.SeatRepository.Delete(seat);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to delete seat. Transaction failed.");
                }

                var seats = await unitOfWork.SeatRepository.Get(s => s.Flight == seat.Flight);

                List<object> obj = new List<object>();

                foreach (var item in seats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }
                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to delete seat.");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.FlightRepository.Get(f => f.FlightId == seatDto.FlightId);
                var flight = res.FirstOrDefault();

                if (flight == null)
                {
                    return NotFound("Flight not found");
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

                try
                {
                   await unitOfWork.SeatRepository.Insert(seat);
                   await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to add seat. Transaction failed.");
                }

                var allSeats = await unitOfWork.SeatRepository.Get(s => s.Flight == flight);
                List<object> obj = new List<object>();

                foreach (var item in allSeats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to add seat.");
            }
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var seat = await unitOfWork.SeatRepository.GetByID(id);

                if (seat == null)
                {
                    return NotFound("Seat not found.");
                }

                seat.Price = seatDto.Price;
                try
                {
                    unitOfWork.SeatRepository.Update(seat);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to update seat. Transaction failed.");
                }

                var seats = await unitOfWork.SeatRepository.Get(s => s.Flight == seat.Flight);

                List<object> obj = new List<object>();

                foreach (var item in seats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to update seat.");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId); //getairlinebyadmin
                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found.");
                }

                var flight = await unitOfWork.FlightRepository.GetByID(id);

                if (flight == null)
                {
                    return NotFound("Flight not found.");
                }

                var seats = await unitOfWork.SeatRepository.Get(s => s.Flight == flight);

                List<object> obj = new List<object>();

                foreach (var item in seats)
                {
                    obj.Add(new { item.Column, item.Row, item.Flight.FlightId, item.Class, item.SeatId, item.Price });
                }

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return seats.");
            }
           
        }

        #endregion

        #region Special offer methods

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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId);
                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found.");
                }

                var specOffers = await unitOfWork.SpecialOfferRepository.GetSpecialOffersOfAirline(airline);

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
                return StatusCode(500, "Failed to return special offers.");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.AirlineRepository.Get(a=>a.AdminId == userId);
                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                List<Seat> seats = new List<Seat>();
                float oldPrice = 0;

                foreach (var seatId in specialOfferDto.SeatsIds)
                {
                    var seatt = await unitOfWork.SeatRepository.GetByID(seatId);
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

                //using (var transaction = new TransactionScope()) {
                    try
                    {
                        await unitOfWork.SpecialOfferRepository.Insert(specialOffer);
                        //unitOfWork.Commit();

                        unitOfWork.AirlineRepository.Update(airline);
                        //unitOfWork.Commit();

                        foreach (var seat in seats)
                        {
                            seat.Available = false;
                            unitOfWork.SeatRepository.Update(seat);
                            //unitOfWork.Commit();
                        }
                        await unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        //unitOfWork.Rollback();
                        return StatusCode(500, "Failed to add special offer. One of transactions failed.");
                    }
                //}
                
                return StatusCode(201, "Special offer created.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to add special offer");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.AirlineRepository.Get(a => a.AdminId == userId);
                var airline = res.FirstOrDefault();

                if (airline == null)
                {
                    return NotFound("Airline not found");
                }

                var specOffer = await unitOfWork.SpecialOfferRepository.GetByID(id);

                if (specOffer == null)
                {
                    return NotFound("Special offer not found");
                }

                try
                {
                    unitOfWork.SpecialOfferRepository.Delete(specOffer);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to delete special offer. Transaction failed.");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to delete special offer");
            }
        }

        #endregion

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

    }
}
