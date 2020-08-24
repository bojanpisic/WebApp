using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IUnitOfWork unitOfWork;
        public UserController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        #region Friendship methods
        [HttpPost]
        [Route("send-friendship-invitation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> SendFriendshipInvitation([FromBody]AddFriendDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var friend = await unitOfWork.UserManager.FindByIdAsync(dto.UserId);

                if (friend == null)
                {
                    return NotFound("Searched user not found");
                }

                //var result = await unitOfWork.UserRepository.CreateFriendshipInvitation(user, friend);
                try
                {
                    //flight.Stops = new List<FlightDestination>
                    //{
                    //    new FlightDestination{
                    //        Flight = flight,
                    //        Destination = stop
                    //    }
                    //};
                    User s = (User)user;
                    User r = (User)friend;
                    var f = new Friendship() { Rejacted = false, Accepted = false, User1 = s, User2 = r };

                    s.FriendshipInvitations.Add(f);
                    r.FriendshipRequests.Add(f);

                    unitOfWork.UserRepository.Update(s);
                    unitOfWork.UserRepository.Update(r);
                    //bilo update usera

                    //await transaction.Result.CommitAsync();
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to send invitation");
                }

                return Ok("Invitation sent");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to send invitation");
            }

        }

        [HttpGet]
        [Route("get-user-requests")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserRequests()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var requests = await unitOfWork.UserRepository.GetRequests(user);

                List<object> allrequ = new List<object>();

                foreach (var item in requests)
                {
                    allrequ.Add(new {
                        senderUserName = item.User1.UserName,
                        senderEmail = item.User1.Email,
                        accepted = item.Accepted,
                        senderFirstName = item.User1.FirstName,
                        senderLastName = item.User1.LastName,
                        senderId = item.User1.Id
                    });
                }

                return Ok(allrequ);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return requests");
            }

        }

        [HttpGet]
        [Route("get-all-users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUsers() 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var allUsers = await unitOfWork.UserRepository.GetAllUsers();
                var usersToReturn = new List<object>();

                foreach (var item in allUsers)
                {
                    var role = await unitOfWork.AuthenticationRepository.GetRoles(item);
                    if (!role.FirstOrDefault().Equals("RegularUser") || item.UserName.Equals(user.UserName))
                    {
                        continue;
                    }
                    if (user.Friends.Contains((User)item))
                    {
                        continue;
                    }

                    usersToReturn.Add(new {
                        username = item.UserName,
                        email = item.Email,
                        firstname = item.FirstName,
                        lastname = item.LastName,
                        id = item.Id
                    });
                }
                return Ok(usersToReturn);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return all users");
            }
        }

        [HttpGet]
        [Route("get-friends")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetFriends()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                //var friends = await unitOfWork.UserRepository.GetFriends(user);
                var friends = user.Friends;

                var usersToReturn = new List<object>();
                foreach (var item in friends)
                {
                    if (user.Friends.Contains((User)item))
                    {
                        continue;
                    }

                    usersToReturn.Add(new
                    {
                        username = item.UserName,
                        email = item.Email,
                        firstname = item.FirstName,
                        lastname = item.LastName,
                        id = item.Id,
                        //inviteSent = user.FriendshipInvitations.FirstOrDefault(i => i.User2Id == item.Id) ? true : false
                    });
                }
                return Ok(usersToReturn);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return user friends");
            }
        }

        [HttpPost]
        [Route("accept-friendship")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AcceptFriendship([FromBody] AddFriendDto dto) 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var friendship = await unitOfWork.UserRepository.GetRequestWhere(userId, dto.UserId);

                if (friendship == null)
                {
                    return BadRequest();
                }

                user.Friends.Add(friendship.User1);
                user.FriendshipRequests.FirstOrDefault(f => f.User1Id == friendship.User1.Id && f.User2Id == friendship.User2.Id).Accepted = true;
                friendship.User1.Friends.Add(user);

                //using (var transaction = _context.Database.BeginTransactionAsync()) 
                //{
                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.UserRepository.Update(friendship.User1);

                    //await transaction.Result.CommitAsync();
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    //await transaction.Result.RollbackAsync();
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to accept friendship. One of transactions failed");
                }
                //}

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to accept friendship");
            }
        }

        [HttpPost]
        [Route("reject-request")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RejectRequest([FromBody] AddFriendDto dto) 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var friendship = await unitOfWork.UserRepository.GetRequestWhere(userId, dto.UserId);

                if (friendship == null)
                {
                    return BadRequest();
                }



                //using (var transaction = _context.Database.BeginTransactionAsync())
                //{
                try
                {
                    unitOfWork.UserRepository.DeleteFriendship(friendship);
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.UserRepository.Update(friendship.User1);

                    await unitOfWork.Commit();
                    //await transaction.Result.CommitAsync();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    //await transaction.Result.RollbackAsync();
                    return StatusCode(500, "Failed to reject request. One of transactions failed");
                }
                //}

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to reject request");
            }
        }
        #endregion

        #region Rent methods
        [HttpPost]
        [Route("rent-car")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RentCar(CarRentDto dto) 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (dto.TakeOverDate > dto.ReturnDate)
                {
                    return BadRequest("Takeover date shoud be lower then return date.");
                }

                var res = await unitOfWork.CarRepository.Get(c => c.CarId == dto.CarRentId, null, "Branch,Rents,SpecialOffers");
                var car = res.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                //provera da li ima special offer za taj period
                var specialOffer = car.SpecialOffers.FirstOrDefault(so => 
                                    dto.ReturnDate >= so.FromDate && so.FromDate >= dto.TakeOverDate ||
                                    dto.ReturnDate >= so.ToDate && so.FromDate >= dto.TakeOverDate || 
                                    so.FromDate < dto.TakeOverDate && so.ToDate > dto.ReturnDate);

                if (specialOffer != null)
                {
                    return BadRequest("This car has special offer for selected period. Cant rent this car");
                }

                //provera da li je vec rezervisan u datom periodu
                foreach (var rent in car.Rents)
                {
                    if (!(rent.TakeOverDate < dto.TakeOverDate && rent.ReturnDate < dto.TakeOverDate ||
                        rent.TakeOverDate > dto.ReturnDate && rent.ReturnDate > dto.ReturnDate))
                    {
                        return BadRequest("The selected car is reserved for selected period");
                    }
                }

                var racsId = car.BranchId == null ? car.RentACarServiceId : car.Branch.RentACarServiceId;

                var result = await unitOfWork.RentACarRepository.Get(r => r.RentACarServiceId ==racsId, null, "Address,Branches");
                var racs = result.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
                }
                if (car.Branch != null)
                {
                    if (!car.Branch.City.Equals(dto.TakeOverCity))
                    {
                        return BadRequest("Takeover city and rent service/branch city dont match");

                    }
                }

                if (!racs.Address.City.Equals(dto.TakeOverCity))
                {
                    return BadRequest("Takeover city and rent service/branch city dont match");
                }

                //provera da li postoje branch gde moze da se vrati auto

                var citiesToReturn = new List<string>();

                foreach (var item in racs.Branches)
                {
                    citiesToReturn.Add(item.City);
                }

                citiesToReturn.Add(racs.Address.City);

                if (!citiesToReturn.Contains(dto.ReturnCity))
                {
                    return BadRequest("Cant return to selected city");
                }

                //using (var transaction = _context.Database.BeginTransactionAsync())
                //{
                var carRent = new CarRent()
                {
                    TakeOverCity = dto.TakeOverCity,
                    ReturnCity = dto.ReturnCity,
                    TakeOverDate = dto.TakeOverDate,
                    ReturnDate = dto.ReturnDate,
                    RentedCar = car,
                    User = user,
                    TotalPrice = await CalculateTotalPrice(dto.TakeOverDate, dto.ReturnDate, car.PricePerDay)
                };

                user.CarRents.Add(carRent);
                car.Rents.Add(carRent);

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.CarRepository.Update(car);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to rent car. One of transactions failed");
                }

                //slanje email-a
                try
                {
                    await unitOfWork.AuthenticationRepository.SendRentConfirmationMail(user, carRent);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Car is rented, but failed to send confirmation email.");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rent car");
            }
        }

        [HttpGet]
        [Route("rent-total-price")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetTotalPrice() 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var queryString = Request.Query;
                var carId = 0;
                DateTime retDate;
                DateTime takeDate;

                if (!Int32.TryParse(queryString["carId"].ToString(), out carId))
                {
                    return BadRequest();
                }
                if (!DateTime.TryParse(queryString["ret"].ToString(), out retDate))
                {
                    return BadRequest();
                }
                if (!DateTime.TryParse(queryString["dep"].ToString(), out takeDate))
                {
                    return BadRequest();
                }
                if (retDate < takeDate)
                {
                    return BadRequest();
                }

                var car = await unitOfWork.CarRepository.GetByID(carId);

                if (car == null)
                {
                    return NotFound("Car not found");
                }
                //ovde bi se trebali uracunati i bodovi korisnika, kako bi se uracunala snizenja
                var totalPrice = await CalculateTotalPrice(takeDate, retDate, car.PricePerDay);

                //var returnData = new {
                //    from = queryString["from"].ToString(),
                //    to = queryString["to"].ToString(),
                //    dep = takeDate,
                //    ret = retDate,
                //    brand = car.Brand,
                //    carId = car.CarId,
                //    model = car.Model, =
                //    name = car.
                //};

                return Ok(totalPrice);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return total price");
            }
        }
        [HttpDelete]
        [Route("cancel-rent/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CancelRent(int id) 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var ress = await unitOfWork.CarRentRepository.Get(crr => crr.CarRentId == id, null, "RentedCar");
                var rent = ress.FirstOrDefault();
                var car = rent.RentedCar;

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                if (Math.Abs((rent.TakeOverDate - DateTime.Now).TotalDays) < 2)
                {
                    return BadRequest("Cant cancel reservation");
                }

                user.CarRents.Remove(rent);
                car.Rents.Remove(rent);

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.CarRepository.Update(car);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to cancel rent car. One of transactions failed");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to cancel rent car");
            }
        }

        [HttpGet]
        [Route("get-car-reservations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRents() 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var rents = await unitOfWork.CarRentRepository.GetRents(user);
                var retVal = new List<object>();

                foreach (var rent in rents)
                {
                    retVal.Add(new {
                        brand = rent.RentedCar.Brand,
                        carId = rent.RentedCar.CarId,
                        carServiceId = rent.RentedCar.RentACarService == null ? 
                                       rent.RentedCar.Branch.RentACarService.RentACarServiceId : rent.RentedCar.RentACarService.RentACarServiceId,
                        model = rent.RentedCar.Model,
                        name = rent.RentedCar.RentACarService == null ?
                                       rent.RentedCar.Branch.RentACarService.Name : rent.RentedCar.RentACarService.Name,
                        seatsNumber = rent.RentedCar.SeatsNumber,
                        pricePerDay = rent.RentedCar.PricePerDay,
                        type = rent.RentedCar.Type,
                        year = rent.RentedCar.Year,
                        totalPrice = rent.TotalPrice,
                        from = rent.TakeOverCity,
                        to = rent.ReturnCity,
                        dep = rent.TakeOverDate,
                        ret = rent.ReturnDate,
                        city = rent.RentedCar.RentACarService == null ?
                                       rent.RentedCar.Branch.RentACarService.Address.City : rent.RentedCar.RentACarService.Address.City,
                        state = rent.RentedCar.RentACarService == null ?
                                       rent.RentedCar.Branch.RentACarService.Address.State : rent.RentedCar.RentACarService.Address.State,
                        isCarRated = rent.IsCarRated,
                        isRACSRated = rent.IsRACSRated,
                        reservationId = rent.CarRentId
                    });
                }

                return Ok(retVal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to cancel rent car");
            }
        }

        private async Task<float> CalculateTotalPrice(DateTime startDate, DateTime endDate, float pricePerDay) 
        {
            await Task.Yield();

            return (float)((endDate - startDate).TotalDays == 0 ? pricePerDay : pricePerDay * (endDate - startDate).TotalDays);
        }
        #endregion

        #region Rate car/racs methods
        [HttpPost]
        [Route("rate-car")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RateCar(RateDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var rents = await unitOfWork.CarRentRepository.Get(crr => crr.User == user, null, "RentedCar");

                var rent = rents.FirstOrDefault(r => r.RentedCar.CarId == dto.Id);

                if (rent == null)
                {
                    return BadRequest("This car is not on your rent list");
                }

                if (rent.IsCarRated)
                {
                    return BadRequest("you already rate this car");
                }

                if (rent.ReturnDate > DateTime.Now)
                {
                    return BadRequest("You can rate this car only when rate period expires");
                }

                var rentedCar = rent.RentedCar;

                rentedCar.Rates.Add(new CarRate()
                {
                    Rate = dto.Rate,
                    User = user,
                    UserId = user.Id,
                    Car = rentedCar
                });

                rent.IsCarRated = true;

                try
                {
                    unitOfWork.CarRepository.Update(rentedCar);
                    unitOfWork.CarRentRepository.Update(rent);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to rate car. One of transactions failed");
                }

                //nesto treba vratiti

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rate car");
            }
        }
        [HttpPost]
        [Route("rate-car-service")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RateRACS(RateDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }


                var rents = await unitOfWork.CarRentRepository
                    .Get(crr => crr.User == user &&
                    crr.RentedCar.RentACarService == null ?
                    crr.RentedCar.Branch.RentACarService.RentACarServiceId == dto.Id : crr.RentedCar.RentACarService.RentACarServiceId == dto.Id
                    , null, "RentedCar");

                //var rent = rents.FirstOrDefault(r => r.RentedCar.CarId == dto.Id);
                var rent = rents.FirstOrDefault();

                if (rent == null)
                {
                    return BadRequest("Cant rate this service.");
                }

                if (rent.IsRACSRated)
                {
                    return BadRequest("you already rate this racs");
                }

                if (rent.ReturnDate > DateTime.Now)
                {
                    return BadRequest("You can rate this rent service only when rate period expires");
                }

                var racs = await unitOfWork.RentACarRepository.GetByID(dto.Id);

                racs.Rates.Add(new RentCarServiceRates()
                {
                    Rate = dto.Rate,
                    User = user,
                    UserId = user.Id,
                    RentACarService = racs
                });

                rent.IsRACSRated = true;

                try
                {
                    unitOfWork.RentACarRepository.Update(racs);
                    unitOfWork.CarRentRepository.Update(rent);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to rate car. One of transactions failed");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rate rent service");
            }
        }
        #endregion

        #region Flight reservation methods
        [HttpPost]
        [Route("flight-reservation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> FlightReservation(FlightReservationDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var seatsForUpdate = new List<Seat>();

                var mySeats = await unitOfWork.SeatRepository.Get(s => dto.MySeatsIds.Contains(s.SeatId));

                if (mySeats.ToList().Count != dto.MySeatsIds.Count)
                {
                    return BadRequest("Something went wrong");
                }

                var err = mySeats.FirstOrDefault(s => s.Available == false || s.Reserved == true);

                if (err != null)
                {
                    return BadRequest("One of seats is already reserved by other user");
                }

                var myFlightReservation = new FlightReservation()
                {
                    User = user,
                };

                var myTickets = new List<Ticket>();

                foreach (var seat in mySeats)
                {
                    myTickets.Add(new Ticket() {
                        Seat = seat,
                        SeatId = seat.SeatId,
                        Reservation = myFlightReservation,
                        Price = seat.Price, //trebalo bi popust uracunati
                        Passport = dto.MyPassport,
                    });

                    seat.Available = false;
                    seat.Reserved = true;
                    seat.Ticket = myTickets.Last();
                    seatsForUpdate.Add(seat);
                }

                myFlightReservation.Tickets = myTickets;

                //za neregistrovane prijatelje koji idu na putovanje******************

                var unregTicketList = new List<Ticket2>();

                foreach (var unregisteredRes in dto.UnregisteredFriends)
                {
                    var seats = await unitOfWork.SeatRepository.Get(s => dto.MySeatsIds.Contains(s.SeatId));

                    if (seats.ToList().Count != unregisteredRes.SeatsIds.Count)
                    {
                        return BadRequest("Something went wrong");
                    }

                    var error = seats.FirstOrDefault(s => s.Available == false || s.Reserved == true);

                    if (err != null)
                    {
                        return BadRequest("One of seats is already reserved by other user");
                    }

                    foreach (var s in seats)
                    {
                        unregTicketList.Add(new Ticket2()
                        {
                            Seat = s,
                            SeatId = s.SeatId,
                            Reservation = myFlightReservation,
                            Price = s.Price, //trebalo bi popust uracunati
                            Passport = unregisteredRes.Passport,
                            FirstName = unregisteredRes.FirstName,
                            LastName = unregisteredRes.LastName,
                        });

                        s.Available = false;
                        s.Reserved = true;
                        s.Ticket2 = unregTicketList.Last();
                        seatsForUpdate.Add(s);
                    }
                }

                myFlightReservation.UnregistredFriendsTickets = unregTicketList;

                //za registrovane prijatelje

                foreach (var friend in dto.Friends)
                {
                    var seats = await unitOfWork.SeatRepository.Get(s => dto.MySeatsIds.Contains(s.SeatId));

                    if (seats.ToList().Count != friend.SeatsIds.Count)
                    {
                        return BadRequest("Something went wrong");
                    }

                    var error = seats.FirstOrDefault(s => s.Available == false || s.Reserved == true);

                    if (err != null)
                    {
                        return BadRequest("One of seats is already reserved by other user");
                    }

                    foreach (var s in seats)
                    {
                        s.Available = false;
                        s.Reserved = true;
                        seatsForUpdate.Add(s);
                    }
                }

                user.FlightReservations.Add(myFlightReservation);

                try
                {
                    foreach (var seat in seatsForUpdate)
                    {
                        unitOfWork.SeatRepository.Update(seat);
                    }

                    unitOfWork.UserRepository.Update(user);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "One of transactions failed. Failed to reserve flight");
                }

                try
                {
                    foreach (var friend in dto.Friends)
                    {
                        await unitOfWork.AuthenticationRepository.SendMailToFriend(friend.Email, user); //rebalo bi slati i letove
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to send one of emails.");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to reserve flight");
            }
        }
        #endregion

        #region Rate flight/air methods
        [HttpPost]
        [Route("rate-flight")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RateFlight(RateDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

               

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rate car");
            }
        }
        [HttpPost]
        [Route("rate-airline")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RateAirline(RateDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rate rent service");
            }
        }
        #endregion
    }
}
