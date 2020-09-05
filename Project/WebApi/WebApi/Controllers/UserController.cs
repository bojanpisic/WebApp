using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

                var friendships = await unitOfWork.UserRepository.GetFriends(user as User);
                if (friendships.FirstOrDefault( f => f.User1 == user as User && f.User2 == friend || f.User2 == user && f.User1 == friend) != null)
                {
                    return BadRequest("Already friend");
                }

                //var result = await unitOfWork.UserRepository.CreateFriendshipInvitation(user, friend);
                User sender = (User)user;
                User receiver = (User)friend;

                var f = new Friendship()
                {
                    Rejacted = false,
                    Accepted = false,
                    User1 = sender,
                    User2 = receiver
                };

                sender.FriendshipInvitations.Add(f);
                receiver.FriendshipRequests.Add(f);

                try
                {
                    unitOfWork.UserRepository.Update(sender);
                    unitOfWork.UserRepository.Update(receiver);
                    //bilo update usera

                    //await transaction.Result.CommitAsync();
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to send invitation");
                }

                return Ok();
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
                    if (role.Count == 0)
                    {
                        continue;
                    }
                    if (!role.FirstOrDefault().Equals("RegularUser") || item.UserName.Equals(user.UserName))
                    {
                        continue;
                    }
                    if (user.Friends.Contains((User)item))
                    {
                        continue;
                    }
                    var invites = await unitOfWork.UserRepository.GetInvitations(user);
                    if (invites.FirstOrDefault(i => i.User2.UserName == item.UserName || i.User1.UserName == item.UserName) != null)
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

                var friends = await unitOfWork.UserRepository.GetFriends(user);
                //var friends = user.Friends;

                var usersToReturn = new List<object>();
                foreach (var item in friends)
                {

                    if (!item.Accepted)
                    {
                        continue;
                    }
                    if (item.User2 == user)
                    {
                        usersToReturn.Add(new
                        {
                            username = item.User1.UserName,
                            email = item.User1.Email,
                            firstname = item.User1.FirstName,
                            lastname = item.User1.LastName,
                            id = item.User1.Id,
                            //inviteSent = user.FriendshipInvitations.FirstOrDefault(i => i.User2Id == item.Id) ? true : false
                        });
                    }
                    else 
                    {
                        usersToReturn.Add(new
                        {
                            username = item.User2.UserName,
                            email = item.User2.Email,
                            firstname = item.User2.FirstName,
                            lastname = item.User2.LastName,
                            id = item.User2.Id,
                            //inviteSent = user.FriendshipInvitations.FirstOrDefault(i => i.User2Id == item.Id) ? true : false
                        });
                    }
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

                var friend = await unitOfWork.UserManager.FindByIdAsync(dto.UserId);

                if (friend == null)
                {
                    return NotFound("Searched user not found");
                }

                var friendships = await unitOfWork.UserRepository.GetFriends(user as User);
                if (friendships.FirstOrDefault(f => f.User1 == user as User && f.User2 == friend 
                                                || f.User2 == user && f.User1 == friend) != null)
                {
                    return BadRequest("Already friend");
                }

                var friendship = await unitOfWork.UserRepository.GetSpecificRequest(userId, dto.UserId);

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

                var friendship = await unitOfWork.UserRepository.GetSpecificRequest(userId, dto.UserId);

                if (friendship == null)
                {
                    return BadRequest();
                }

                user.FriendshipRequests.Remove(friendship);
                var friend = friendship.User1 == user ? friendship.User2 : friendship.User1;
                friend.FriendshipInvitations.Remove(friendship);
                //using (var transaction = _context.Database.BeginTransactionAsync())
                //{
                try
                {
                    unitOfWork.UserRepository.DeleteFriendship(friendship);
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.UserRepository.Update(friend);

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

        [HttpPost]
        [Route("delete-friend")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteFriend([FromBody] AddFriendDto dto)
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

                var friendships = await unitOfWork.UserRepository.GetFriends(user);

                if (friendships == null)
                {
                    return BadRequest("You dont have any friendships");
                }

                var friendship = friendships.FirstOrDefault(f => f.User1 == user && f.User2Id == dto.UserId 
                                                            || f.User2 == user && f.User1Id == dto.UserId);

                if (friendship == null)
                {
                    return BadRequest("Friendship not found");
                }
                var friend = friendship.User1 == user ? friendship.User2 : friendship.User1;
                user.Friends.Remove(friend);
                friend.Friends.Remove(user);

                //using (var transaction = _context.Database.BeginTransactionAsync())
                //{
                try
                {
                    unitOfWork.UserRepository.DeleteFriendship(friendship);
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.UserRepository.Update(friend);

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

                if (dto.TakeOverDate < DateTime.Now.Date)
                {
                    return BadRequest("Date is in past");
                }

                if (dto.TakeOverDate > dto.ReturnDate)
                {
                    return BadRequest("Takeover date shoud be lower then return date.");
                }

                //var res = await unitOfWork.CarRepository.Get(c => c.CarId == dto.CarRentId, null, "Branch,Rents,SpecialOffers");
                //var car = res.FirstOrDefault();

                var car = (await unitOfWork.CarRepository.AllCars(c => c.CarId == dto.CarRentId)).FirstOrDefault();

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

                //var racsId = car.BranchId == null ? car.RentACarServiceId : car.Branch.RentACarServiceId;

                //var result = await unitOfWork.RentACarRepository.Get(r => r.RentACarServiceId ==racsId, null, "Address,Branches");
                //var racs = result.FirstOrDefault();

                //if (racs == null)
                //{
                //    return NotFound("RACS not found");
                //}
                if (car.Branch != null)
                {
                    if (!car.Branch.City.Equals(dto.TakeOverCity))
                    {
                        return BadRequest("Takeover city and rent service/branch city dont match");

                    }
                }
                else 
                {
                    if (!car.RentACarService.Address.City.Equals(dto.TakeOverCity))
                    {
                        return BadRequest("Takeover city and rent service/branch city dont match");
                    }
                }

                //provera da li postoje branch gde moze da se vrati auto

                var citiesToReturn = new List<string>();

                foreach (var item in car.RentACarService == null ? car.Branch.RentACarService.Branches : car.RentACarService.Branches)
                {
                    citiesToReturn.Add(item.City);
                }

                citiesToReturn.Add(car.RentACarService == null ? car.Branch.RentACarService.Address.City : car.RentACarService.Address.City);

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
                    TotalPrice = await CalculateTotalPrice(dto.TakeOverDate, dto.ReturnDate, car.PricePerDay),
                    RentDate = DateTime.Now
                };

                user.CarRents.Add(carRent);
                car.Rents.Add(carRent);

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.CarRepository.Update(car);

                    await unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException ex) 
                {
                    return BadRequest("Car is modified in the meantime, or reserved by another user");
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

                var ress = await unitOfWork.CarRentRepository.Get(crr => crr.CarRentId == id, null, "RentedCar,User");
                var rent = ress.FirstOrDefault();
                var car = rent.RentedCar;

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                if (rent.User != user)
                {
                    return BadRequest();
                }

                if (rent.TakeOverDate.AddDays(-2) < DateTime.Now.Date)
                {
                    return BadRequest("Cant cancel reservation");
                }

                var specialOffers = await unitOfWork.RACSSpecialOfferRepository.Get(s => s.Car == car && s.FromDate == rent.TakeOverDate && s.ToDate == rent.ReturnDate);
                var specOffer = specialOffers.FirstOrDefault();

                if (specOffer != null)
                {
                    specOffer.IsReserved = false;
                }
                user.CarRents.Remove(rent);
                car.Rents.Remove(rent);

                try
                {
                    if (specOffer != null)
                    {
                        unitOfWork.RACSSpecialOfferRepository.Update(specOffer);
                    }
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
                    var sum = 0.0;
                    foreach (var item in rent.RentedCar.Rates)
                    {
                        sum += item.Rate;
                    }

                    var rate = sum == 0.0 ? 0.0 : sum / rent.RentedCar.Rates.Count;

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
                        isCarRated = rent.RentedCar.Rates.FirstOrDefault(r => r.UserId == userId) != null ? true : false,
                        isRACSRated = (await unitOfWork.RentACarRepository.Get(r => r.RentACarServiceId == (rent.RentedCar.RentACarService == null ?
                                       rent.RentedCar.Branch.RentACarService.RentACarServiceId : rent.RentedCar.RentACarService.RentACarServiceId), null, "Rates"))
                                       .FirstOrDefault().Rates.FirstOrDefault(r => r.UserId == userId) == null ? false : true,
                        canCancel = rent.TakeOverDate.AddDays(-2) >= DateTime.Now.Date,
                        canRate = rent.ReturnDate < DateTime.Now.Date,
                        isUpcoming = rent.TakeOverDate >= DateTime.Now.Date,
                        reservationId = rent.CarRentId,
                        rate = rate
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

                if (dto.Rate > 5 || dto.Rate < 1)
                {
                    return BadRequest("Invalid rate. Rate from 1 to 5");
                }

                var rent = await unitOfWork.CarRentRepository.GetRentByFilter(r => r.User == user && r.RentedCar.CarId == dto.Id);

                //var rents = await unitOfWork.CarRentRepository.Get(crr => crr.User == user, null, "RentedCar");

                //var rent = rents.FirstOrDefault(r => r.RentedCar.CarId == dto.Id);

                if (rent == null)
                {
                    return BadRequest("This car is not on your rent list");
                }
                if (rent.RentedCar.Rates.FirstOrDefault(r => r.UserId == userId) != null)
                {
                    return BadRequest("You already rated this car");
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

                try
                {
                    unitOfWork.CarRepository.Update(rentedCar);

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

                if (dto.Rate > 5 || dto.Rate < 1)
                {
                    return BadRequest("Invalid rate. Rate from 1 to 5");
                }

                var racs = await unitOfWork.RentACarRepository.GetRacsWithRates(dto.Id);

                //if (rent.IsRACSRated)
                //{
                //    return BadRequest("you already rate this racs");
                //}

                if (racs.Rates.FirstOrDefault(r => r.UserId == userId) != null)
                {
                    return BadRequest("You already rate this racs");
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

                if (rent.ReturnDate > DateTime.Now)
                {
                    return BadRequest("You can rate this rent service only when rate period expires");
                }

                racs.Rates.Add(new RentCarServiceRates()
                {
                    Rate = dto.Rate,
                    User = user,
                    UserId = user.Id,
                    RentACarService = racs
                });

                try
                {
                    unitOfWork.RentACarRepository.Update(racs);
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

                if (dto.MySeatsIds.Count == 0)
                {
                    return BadRequest("Choose at least one seat on flight");
                }



                var seatsForUpdate = new List<Seat>();

                var mySeats = await unitOfWork.SeatRepository.Get(s => dto.MySeatsIds.Contains(s.SeatId), null, "Flight");

                if (mySeats.ToList().Count != dto.MySeatsIds.Count)
                {
                    return BadRequest("Something went wrong");
                }

                var MyTickets = await unitOfWork.TicketRepository.Get(t => t.Reservation.User == user, null, "Seat");

                if (MyTickets.FirstOrDefault(t => mySeats.Contains(t.Seat)) != null)
                {
                    return BadRequest("You already reserved seat on selected flight");
                }

                var err = mySeats.FirstOrDefault(s => s.Available == false || s.Reserved == true);

                if (err != null)
                {
                    return BadRequest("One of seats is already reserved by other user");
                }

                var myFlightReservation = new FlightReservation()
                {
                    User = user,
                    ReservationDate = DateTime.Now,
                };

                var myTickets = new List<Ticket>();

                //var systemB = await unitOfWork.BonusRepository.GetByID(1);

                //int systemBonus = 0;

                //if (systemB == null)
                //{
                //    systemBonus = 0;
                //}
                //else 
                //{
                //    systemBonus = systemB.BonusPerKilometer;
                //}
                //nije bilo ovde
                //int bonus = 0;
                //Int32.TryParse(seat.Flight.tripLength.ToString(), out bonus);
                //user.BonusPoints += bonus * systemBonus;

                float totalPrice = 0;

                foreach (var seat in mySeats)
                {
                    myTickets.Add(new Ticket() {
                        Seat = seat,
                        SeatId = seat.SeatId,
                        Reservation = myFlightReservation,
                        Price = seat.Price,
                        Passport = dto.MyPassport,
                    });

                    totalPrice += seat.Price; //bonus ovde
                    seat.Available = false;
                    seat.Reserved = true;
                    seat.Ticket = myTickets.Last();
                    seatsForUpdate.Add(seat);

                    //int bonus = 0;
                    //Int32.TryParse(seat.Flight.tripLength.ToString(), out bonus);
                    //user.BonusPoints += bonus * systemBonus;
                }

                if (dto.WithBonus)
                {
                    if (totalPrice < user.BonusPoints * 0.01)
                    {
                        totalPrice = 0;
                        user.BonusPoints = (int)(user.BonusPoints * 0.01 - totalPrice) * 100;
                    }
                    else 
                    {
                        totalPrice = (float)(totalPrice - user.BonusPoints * 0.01);
                        user.BonusPoints = 0;
                    }
                }

                myFlightReservation.Price = totalPrice;
                myFlightReservation.Tickets = myTickets;

                //za neregistrovane prijatelje koji idu na putovanje******************

                var unregTicketList = new List<Ticket2>();

                foreach (var unregisteredRes in dto.UnregisteredFriends)
                {
                    var seat = await unitOfWork.SeatRepository.GetByID(unregisteredRes.SeatId);

                    //if (seats.ToList().Count != unregisteredRes.SeatsIds.Count)
                    //{
                    //    return BadRequest("Something went wrong");
                    //}

                    //var error = seats.FirstOrDefault(s => s.Available == false || s.Reserved == true);

                    //if (err != null)
                    //{
                    //    return BadRequest("One of seats is already reserved by other user");
                    //}
                    if (seat == null)
                    {
                        return BadRequest("Seat not found");
                    }
                    if (!seat.Available || seat.Reserved)
                    {
                        return BadRequest("Selected seat is reserved already");
                    }

                    //foreach (var s in seats)
                    //{
                    unregTicketList.Add(new Ticket2()
                    {
                        Seat = seat,
                        SeatId = seat.SeatId,
                        Reservation = myFlightReservation,
                        Price = seat.Price, //trebalo bi popust uracunati
                        Passport = unregisteredRes.Passport,
                        FirstName = unregisteredRes.FirstName,
                        LastName = unregisteredRes.LastName,
                    });

                    seat.Available = false;
                    seat.Reserved = true;
                    seat.Ticket2 = unregTicketList.Last();
                    seatsForUpdate.Add(seat);
                    //}
                }

                myFlightReservation.UnregistredFriendsTickets = unregTicketList;

                //za registrovane prijatelje

                var friendsList = new List<User>();
                var invitationList = new List<Invitation>();

                foreach (var friend in dto.Friends)
                {
                    var f = await unitOfWork.AuthenticationRepository.GetUserById(friend.Id) as User;
                    friendsList.Add(f);

                    var seat = (await unitOfWork.SeatRepository.Get(s => s.SeatId == friend.SeatId, null, "Flight")).FirstOrDefault();

                    //if (seats.ToList().Count != friend.SeatsIds.Count)
                    //{
                    //    return BadRequest("Something went wrong");
                    //}

                    //var error = seats.FirstOrDefault(s => s.Available == false || s.Reserved == true);

                    //if (err != null)
                    //{
                    //    return BadRequest("One of seats is already reserved by other user");

                    if (seat == null)
                    {
                        return BadRequest("Seat not found");
                    }
                    if (!seat.Available || seat.Reserved)
                    {
                            return BadRequest("Selected seat is reserved");
                    }

                    //foreach (var s in seats)
                    //{
                    seat.Available = false;
                    seat.Reserved = true;
                    seatsForUpdate.Add(seat);
                    invitationList.Add(new Invitation() {
                        Sender = user,
                        Receiver = f,
                        Seat = seat,
                        Price = seat.Price,
                        Expires = seat.Flight.TakeOffDateTime < DateTime.Now.AddDays(3) ? 
                                    seat.Flight.TakeOffDateTime.AddHours(-3) : DateTime.Now.AddDays(3),
                    });
                    //}
                }

                //CAR RESERVATION
                Car car = null;
                if (dto.CarReservation != null)
                {
                    if (dto.CarReservation.TakeOverDate < DateTime.Now.Date)
                    {
                        return BadRequest("Date is in past");
                    }

                    if (dto.CarReservation.TakeOverDate > dto.CarReservation.ReturnDate)
                    {
                        return BadRequest("Takeover date shoud be lower then return date.");
                    }

                    car = (await unitOfWork.CarRepository.AllCars(c => c.CarId == dto.CarReservation.CarRentId)).FirstOrDefault();

                    if (car == null)
                    {
                        return NotFound("Car not found");
                    }

                    //provera da li ima special offer za taj period
                    var specialOffer = car.SpecialOffers.FirstOrDefault(so =>
                                        dto.CarReservation.ReturnDate >= so.FromDate && so.FromDate >= dto.CarReservation.TakeOverDate ||
                                        dto.CarReservation.ReturnDate >= so.ToDate && so.FromDate >= dto.CarReservation.TakeOverDate ||
                                        so.FromDate < dto.CarReservation.TakeOverDate && so.ToDate > dto.CarReservation.ReturnDate);

                    if (specialOffer != null)
                    {
                        return BadRequest("This car has special offer for selected period. Cant rent this car");
                    }

                    //provera da li je vec rezervisan u datom periodu
                    foreach (var rent in car.Rents)
                    {
                        if (!(rent.TakeOverDate < dto.CarReservation.TakeOverDate && rent.ReturnDate < dto.CarReservation.TakeOverDate ||
                            rent.TakeOverDate > dto.CarReservation.ReturnDate && rent.ReturnDate > dto.CarReservation.ReturnDate))
                        {
                            return BadRequest("The selected car is reserved for selected period");
                        }
                    }

                    //var racsId = car.BranchId == null ? car.RentACarServiceId : car.Branch.RentACarServiceId;

                    //var result = await unitOfWork.RentACarRepository.Get(r => r.RentACarServiceId == racsId, null, "Address,Branches");
                    //var racs = result.FirstOrDefault();

                    //if (racs == null)
                    //{
                    //    return NotFound("RACS not found");
                    //}
                    if (car.Branch != null)
                    {
                        if (!car.Branch.City.Equals(dto.CarReservation.TakeOverCity))
                        {
                            return BadRequest("Takeover city and rent service/branch city dont match");

                        }
                    }
                    else 
                    {
                        if (!car.RentACarService.Address.City.Equals(dto.CarReservation.TakeOverCity))
                        {
                            return BadRequest("Takeover city and rent service/branch city dont match");
                        }
                    }

                    //provera da li postoje branch gde moze da se vrati auto

                    var citiesToReturn = new List<string>();

                    foreach (var item in car.RentACarService == null ? car.Branch.RentACarService.Branches : car.RentACarService.Branches)
                    {
                        citiesToReturn.Add(item.City);
                    }

                    citiesToReturn.Add(car.RentACarService == null ? car.Branch.RentACarService.Address.City : car.RentACarService.Address.City);

                    if (!citiesToReturn.Contains(dto.CarReservation.ReturnCity))
                    {
                        return BadRequest("Cant return to selected city");
                    }

                    //using (var transaction = _context.Database.BeginTransactionAsync())
                    //{
                    var carRent = new CarRent()
                    {
                        TakeOverCity = dto.CarReservation.TakeOverCity,
                        ReturnCity = dto.CarReservation.ReturnCity,
                        TakeOverDate = dto.CarReservation.TakeOverDate,
                        ReturnDate = dto.CarReservation.ReturnDate,
                        RentedCar = car,
                        User = user,
                        TotalPrice = (await CalculateTotalPrice(dto.CarReservation.TakeOverDate, dto.CarReservation.ReturnDate, car.PricePerDay)),
                        RentDate = DateTime.Now
                    };

                    myFlightReservation.CarRent = carRent;
                    carRent.FlightReservation = myFlightReservation;

                    user.CarRents.Add(carRent);
                    car.Rents.Add(carRent);

                }

                user.FlightReservations.Add(myFlightReservation);

                try
                {
                    foreach (var seat in seatsForUpdate)
                    {
                        unitOfWork.SeatRepository.Update(seat);
                    }
                    foreach (var invitation in invitationList)
                    {
                        user.TripInvitations.Add(invitation);
                        invitation.Receiver.TripRequests.Add(invitation);
                        unitOfWork.UserRepository.Update(invitation.Receiver);
                    }
                    if (car != null)
                    {
                        unitOfWork.CarRepository.Update(car);
                    }

                    unitOfWork.UserRepository.Update(user);

                    await unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Somethin is modified in the meantime, or reserved by another user");
                }
                catch (Exception)
                {
                    return StatusCode(500, "One of transactions failed. Failed to reserve flight");
                }

                try
                {
                    await unitOfWork.AuthenticationRepository.SendTicketConfirmationMail(user, myFlightReservation);

                }
                catch (Exception)
                {
                    //return StatusCode(500, "Failed to send one of emails.");
                }
                foreach (var invitation in invitationList)
                {
                    try
                    {
                        await unitOfWork.AuthenticationRepository.SendMailToFriend(invitation);
                    }
                    catch (Exception)
                    {
                    }
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to reserve flight");
            }
        }


        [HttpDelete]
        [Route("cancel-flight-reservation/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CancelFlightReservation(int id)
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

                var reservation = await unitOfWork.FlightReservationRepository.GetReservationById(id);

                if (reservation == null)
                {
                    return NotFound();
                }

                if (reservation.User != user)
                {
                    return BadRequest();
                }

                if (Math.Abs((reservation.Tickets.OrderBy(t => t.Seat.Flight.TakeOffDateTime).FirstOrDefault().Seat.Flight.TakeOffDateTime - DateTime.Now).TotalHours) < 3)
                {
                    return BadRequest("Cant cancel flight");
                }

                var listOfSeatsToUpdate = new List<Seat>();

                //var systemB = await unitOfWork.BonusRepository.GetByID(1);

                //int systemBonus = 0;

                //if (systemB == null)
                //{
                //    systemBonus = 0;
                //}
                //else
                //{
                //    systemBonus = systemB.BonusPerKilometer;
                //}

                foreach (var ticket in reservation.Tickets)
                {
                    ticket.Seat.Available = true;
                    ticket.Seat.Reserved = false;
                    listOfSeatsToUpdate.Add(ticket.Seat);
                    //user.BonusPoints -= systemBonus * (int)ticket.Seat.Flight.tripLength;
                }

                user.FlightReservations.Remove(reservation);

                CarSpecialOffer carSpecOffer = null;

                if (reservation.CarRent != null)
                {
                    var specialOffers = await unitOfWork.RACSSpecialOfferRepository.Get(s => s.Car == reservation.CarRent.RentedCar
                                                    && s.FromDate == reservation.CarRent.TakeOverDate && s.ToDate == reservation.CarRent.ReturnDate);
                    carSpecOffer = specialOffers.FirstOrDefault();

                    if (carSpecOffer != null)
                    {
                        carSpecOffer.IsReserved = false;
                    }
                    user.CarRents.Remove(reservation.CarRent);
                    reservation.CarRent.RentedCar.Rents.Remove(reservation.CarRent);
                }

                try
                {
                    foreach (var seat in listOfSeatsToUpdate)
                    {
                        unitOfWork.SeatRepository.Update(seat);
                    }

                    unitOfWork.UserRepository.Update(user);
                    if (reservation.CarRent != null)
                    {
                        unitOfWork.CarRepository.Update(reservation.CarRent.RentedCar);
                    }
                    if (carSpecOffer != null)
                    {
                        unitOfWork.RACSSpecialOfferRepository.Update(carSpecOffer);
                    }

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to cancel reservation");
                }

                return Ok();
            }
            catch (Exception) 
            {
                return StatusCode(500, "Failed to cancel reservation");
            }
        }

        [HttpPost]
        [Route("get-trip-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTripInfo([FromBody]InfoDto dto) 
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

                var mySeats = await unitOfWork.SeatRepository.Get(s => dto.MySeatsIds.Contains(s.SeatId));
                var seats = new List<object>();

                foreach (var item in mySeats)
                {
                    seats.Add(new {
                        row = item.Row,
                        clas = item.Class,
                        col = item.Column,
                    });
                }

                var friendList = new List<object>();

                foreach (var item in dto.Friends)
                {
                    var friend = await unitOfWork.UserRepository.GetByID(item.Id);
                    var seat = await unitOfWork.SeatRepository.GetByID(item.SeatId);
                    friendList.Add(new {
                        friendEmail = friend.Email,
                        friendFirstName = friend.FirstName,
                        friendLastName = friend.LastName,
                        column = seat.Column,
                        row = seat.Row,
                        clas = seat.Class,
                    });
                }
                var unregisteredFriendList = new List<object>();

                foreach (var item in dto.UnregisteredFriends)
                {
                    var seat = await unitOfWork.SeatRepository.GetByID(item.SeatId);
                    unregisteredFriendList.Add(new
                    {
                        lastName = item.LastName,
                        firstName = item.FirstName,
                        passport = item.Passport,
                        column = seat.Column,
                        row = seat.Row,
                        clas = seat.Class,
                    });
                }

                float totalPrice = 0;

                foreach (var item in mySeats)
                {
                    totalPrice += item.Price;
                }

                float priceWithBonus = 0;

                if (totalPrice < user.BonusPoints * 0.01)
                {
                    priceWithBonus = 0;
                }
                else
                {
                    priceWithBonus = (float)(totalPrice - user.BonusPoints * 0.01);
                }

                return Ok(new 
                {
                    priceWithBonus = priceWithBonus,
                    totalPrice = totalPrice,
                    friends = friendList,
                    unregisteredFriends = unregisteredFriendList,
                    mySeats = seats,
                    myBonus = user.BonusPoints
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return trip info");
            }
        }

        #endregion

        #region Trip methods
        [HttpGet]
        [Route("get-previous-flights")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPreviousFlights()
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

                var trips = await unitOfWork.FlightReservationRepository.GetTrips(user);
                var retVal = new List<object>();

                foreach (var trip in trips)
                {
                    foreach (var ticket in trip.Tickets)
                    {
                        if (ticket.Seat.Flight.LandingDateTime >= DateTime.Now)
                        {
                            continue;
                        }
                        List<object> stops = new List<object>();

                        foreach (var stop in ticket.Seat.Flight.Stops)
                        {
                            stops.Add(new {
                                stop.Destination.City, 
                                stop.Destination.State
                            });
                        }
                        
                        retVal.Add(new
                        {
                            column = ticket.Seat.Column,
                            row = ticket.Seat.Row,
                            clas = ticket.Seat.Class,
                            seatId = ticket.Seat.SeatId,
                            seatPrice = ticket.Seat.Price,
                            takeOffDate = ticket.Seat.Flight.TakeOffDateTime.Date,
                            landingDate = ticket.Seat.Flight.LandingDateTime.Date,
                            airlineLogo = ticket.Seat.Flight.Airline.LogoUrl,
                            airlineName = ticket.Seat.Flight.Airline.Name,
                            airlineId = ticket.Seat.Flight.Airline.AirlineId,
                            from = ticket.Seat.Flight.From.City,
                            to = ticket.Seat.Flight.To.City,
                            takeOffTime = ticket.Seat.Flight.TakeOffDateTime.TimeOfDay,
                            landingTime = ticket.Seat.Flight.LandingDateTime.TimeOfDay,
                            flightTime = ticket.Seat.Flight.TripTime,
                            flightLength = ticket.Seat.Flight.tripLength,
                            flightNumber = ticket.Seat.Flight.FlightNumber,
                            flightId = ticket.Seat.Flight.FlightId,
                            stops = stops,
                            isAirlineRated = ticket.Seat.Flight.Airline.Rates.FirstOrDefault(r => r.User == user) == null ?
                                             false : true,
                            isFlightRated = ticket.Seat.Flight.Rates.FirstOrDefault(r => r.User == user) == null ?
                                             false : true,
                        });
                    }
                }

                return Ok(retVal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return trips");
            }
        }

        [HttpGet]
        [Route("get-upcoming-trips")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUpcomingTrips()
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

                var trips = await unitOfWork.FlightReservationRepository.GetTrips(user);
                var flights = new List<object>();
                var retVal = new List<object>();

                foreach (var trip in trips)
                {
                    var previous = trip.Tickets.Where(t => t.Seat.Flight.LandingDateTime < DateTime.Now);
                    if (previous.ToList().Count == trip.Tickets.ToList().Count)
                    {
                        continue;
                    }

                    flights = new List<object>();

                    foreach (var ticket in trip.Tickets)
                    {
                        
                        List<object> stops = new List<object>();

                        foreach (var stop in ticket.Seat.Flight.Stops)
                        {
                            stops.Add(new
                            {
                                stop.Destination.City,
                                stop.Destination.State
                            });
                        }

                        flights.Add(new
                        {
                            column = ticket.Seat.Column,
                            row = ticket.Seat.Row,
                            clas = ticket.Seat.Class,
                            seatId = ticket.Seat.SeatId,
                            seatPrice = ticket.Seat.Price,
                            takeOffDate = ticket.Seat.Flight.TakeOffDateTime.Date,
                            landingDate = ticket.Seat.Flight.LandingDateTime.Date,
                            airlineLogo = ticket.Seat.Flight.Airline.LogoUrl,
                            airlineName = ticket.Seat.Flight.Airline.Name,
                            airlineId = ticket.Seat.Flight.Airline.AirlineId,
                            from = ticket.Seat.Flight.From.City,
                            to = ticket.Seat.Flight.To.City,
                            takeOffTime = ticket.Seat.Flight.TakeOffDateTime.TimeOfDay,
                            landingTime = ticket.Seat.Flight.LandingDateTime.TimeOfDay,
                            flightTime = ticket.Seat.Flight.TripTime,
                            flightLength = ticket.Seat.Flight.tripLength,
                            flightNumber = ticket.Seat.Flight.FlightNumber,
                            flightId = ticket.Seat.Flight.FlightId,
                            stops = stops,
                            canCancel = Math.Abs((ticket.Seat.Flight.TakeOffDateTime - DateTime.Now).TotalHours) >= 3,
                        });
                    }
                    retVal.Add(new {
                        reservationId = trip.FlightReservationId,
                        flights = flights,
                        totalPrice = trip.Price
                    });
                }

                return Ok(retVal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return trips");
            }
        }
        [HttpPost]
        [Route("accept-trip-invitation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AcceptTripInvitation(AcceptTripDto dto)
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

                var invitation = await unitOfWork.TripInvitationRepository.GetTripInvitationById(dto.Id);

                if (invitation == null)
                {
                    return BadRequest("Cant find your invitation");
                }


                var flightReservation = new FlightReservation()
                {
                    User = user,
                };

                var tickets = new List<Ticket>();

                tickets.Add(new Ticket()
                {
                    Passport = dto.Passport,
                    Price = invitation.Price,
                    Seat = invitation.Seat,
                    SeatId = invitation.SeatId,
                    Reservation = flightReservation
                });

                var systemB = await unitOfWork.BonusRepository.GetByID(1);

                int systemBonus = 0;

                if (systemB == null)
                {
                    systemBonus = 0;
                }
                else
                {
                    systemBonus = systemB.BonusPerKilometer;
                }

                flightReservation.Tickets = tickets;

                user.FlightReservations.Add(flightReservation);
                user.TripRequests.Remove(invitation);
                user.BonusPoints += (int)invitation.Seat.Flight.tripLength * systemBonus;
                invitation.Sender.TripInvitations.Remove(invitation);

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.UserRepository.Update(invitation.Sender);

                    unitOfWork.TripInvitationRepository.Delete(invitation);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    StatusCode(500, "Failed to accept invitation");
                }
                try
                {

                }
                catch (Exception)
                {
                    StatusCode(500, "Successfully accepted invitation, but unable to send email.");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to accept trip");
            }
        }

        [HttpDelete]
        [Route("reject-trip-invitation/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RejectTripInvitation(int id)
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

                var invitation = await unitOfWork.TripInvitationRepository.GetTripInvitationById(id);

                if (invitation == null)
                {
                    return BadRequest("Cant find your invitation");
                }


                user.TripRequests.Remove(invitation);
                invitation.Sender.TripInvitations.Remove(invitation);

                invitation.Seat.Available = true;
                invitation.Seat.Reserved = false;

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.UserRepository.Update(invitation.Sender);
                    unitOfWork.SeatRepository.Update(invitation.Seat);

                    unitOfWork.TripInvitationRepository.Delete(invitation);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    StatusCode(500, "Failed to reject invitation");
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to reject invitation");
            }
        }

        [HttpGet]
        [Route("get-trip-invitations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetTripInvitations()
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

                var invitations = await unitOfWork.TripInvitationRepository.GetTripInvitations(user);
                var retVal = new List<object>();

                foreach (var invite in invitations)
                {
                    if (invite.Expires <= DateTime.Now)
                    {
                        user.TripRequests.Remove(invite);
                        invite.Sender.TripInvitations.Remove(invite);

                        invite.Seat.Available = true;
                        invite.Seat.Reserved = false;

                        try
                        {
                            unitOfWork.UserRepository.Update(user);
                            unitOfWork.UserRepository.Update(invite.Sender);
                            unitOfWork.SeatRepository.Update(invite.Seat);

                            unitOfWork.TripInvitationRepository.Delete(invite);

                            await unitOfWork.Commit();
                        }
                        catch (Exception)
                        {
                            
                        }
                        continue;
                    }
                    retVal.Add(new
                    {
                        column = invite.Seat.Column,
                        row = invite.Seat.Row,
                        clas = invite.Seat.Class,
                        seatId = invite.Seat.SeatId,
                        seatPrice = invite.Seat.Price,
                        takeOffDate = invite.Seat.Flight.TakeOffDateTime.Date,
                        landingDate = invite.Seat.Flight.LandingDateTime.Date,
                        airlineLogo = invite.Seat.Flight.Airline.LogoUrl,
                        airlineName = invite.Seat.Flight.Airline.Name,
                        airlineId = invite.Seat.Flight.Airline.AirlineId,
                        from = invite.Seat.Flight.From.City,
                        to = invite.Seat.Flight.To.City,
                        takeOffTime = invite.Seat.Flight.TakeOffDateTime.TimeOfDay,
                        landingTime = invite.Seat.Flight.TakeOffDateTime.TimeOfDay,
                        flightTime = invite.Seat.Flight.TripTime,
                        flightLength = invite.Seat.Flight.tripLength,
                        flightNumber = invite.Seat.Flight.FlightNumber,
                        flightId = invite.Seat.Flight.FlightId,
                        senderId = invite.Sender.Id,
                        senderUserName = invite.Sender.UserName,
                        senderFirstName = invite.Sender.FirstName,
                        senderLastName = invite.Sender.LastName,
                        senderEmail = invite.Sender.Email,
                        invitationId = invite.InvitationId
                    });
                }

                return Ok(retVal);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return trips");
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

                if (dto.Rate > 5 || dto.Rate < 1)
                {
                    return BadRequest("Invalid rate. Rate from 1 to 5");
                }

                var flights = await unitOfWork.FlightRepository.Get(f => f.FlightId == dto.Id, null, "Rates");

                var flight = flights.FirstOrDefault();

                if (flight == null)
                {
                    return BadRequest("Flight not found");
                }

                if (flight.Rates.FirstOrDefault(r => r.UserId == userId) != null)
                {
                    return BadRequest("You already rate this flight");
                }

                //PROVERA DA LI JE LETIO NA OVOM LETU
                var flightReservations = await unitOfWork.FlightReservationRepository
                                                .Get(f => f.Tickets.FirstOrDefault(t => t.Seat.Flight == flight) != null && f.User == user,
                                                null,
                                                "Tickets");

                var flightReservation = flightReservations.FirstOrDefault();

                if (flightReservation == null)
                {
                    return BadRequest("You didnt reserve seat on this flight. Cant rate.");
                }

                var ticketOfReservation = flightReservation.Tickets.FirstOrDefault();
                if (ticketOfReservation == null)
                {
                    return BadRequest();
                }

                var ticket = await unitOfWork.TicketRepository.GetTicket(ticketOfReservation.TicketId);

                if (ticket == null)
                {
                    return BadRequest();
                }

                if (ticket.Seat.Flight.LandingDateTime >= DateTime.Now)
                {
                    return BadRequest("You can rate flight after landing");
                }

                flight.Rates.Add(new FlightRate()
                {
                    Rate = dto.Rate,
                    User = user,
                    UserId = user.Id,
                    Flight = flight
                });

                try
                {
                    unitOfWork.FlightRepository.Update(flight);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to rate flight. One of transactions failed");
                }


                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rate flight");
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
                
                if (dto.Rate > 5 || dto.Rate < 1)
                {
                    return BadRequest("Invalid rate. Rate from 1 to 5");
                }

                var airlines = await unitOfWork.AirlineRepository.Get(a => a.AirlineId == dto.Id, null, "Rates");

                var airline = airlines.FirstOrDefault();

                if (airline == null)
                {
                    return BadRequest("Airline not found");
                }

                if (airline.Rates.FirstOrDefault(r => r.UserId == userId) != null)
                {
                    return BadRequest("You already rate this airlien company");
                }
                //PROVERA DA LI JE LETIO OVOM KOMPANIJOM
                var flightReservations = await unitOfWork.FlightReservationRepository
                                                .Get(f => f.Tickets.FirstOrDefault(t => 
                                                                                    t.Seat.Flight.Airline == airline 
                                                                                    && t.Seat.Flight.LandingDateTime >= DateTime.Now) != null
                                                                                    && f.User == user,
                                                null,
                                                "Tickets");

                airline.Rates.Add(new AirlineRate()
                {
                    Rate = dto.Rate,
                    User = user,
                    UserId = user.Id,
                    Airline = airline
                });

                try
                {
                    unitOfWork.AirlineRepository.Update(airline);

                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to rate airline. One of transactions failed");
                }
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rate airline");
            }
        }
        #endregion

        #region Reserve special offers of flight/car
        [HttpPost]
        [Route("reserve-special-offer-car")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ReserveSpecialOfferCar([FromBody] ReserveDto dto)
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

                var specialOffer = await unitOfWork.RACSSpecialOfferRepository.GetSpecialOfferById(dto.Id);

                if (specialOffer == null)
                {
                    return NotFound("Selected special offer is not found");
                }

                if (specialOffer.IsReserved)
                {
                    return BadRequest("Already reserved");
                }

                foreach (var rent in specialOffer.Car.Rents)
                {
                    if (!(rent.TakeOverDate < specialOffer.FromDate && rent.ReturnDate < specialOffer.FromDate ||
                        rent.TakeOverDate > specialOffer.ToDate && rent.ReturnDate > specialOffer.ToDate))
                    {
                        return BadRequest("The selected car is reserved for selected period");
                    }
                }

                var carRent = new CarRent()
                {
                    TakeOverDate = specialOffer.FromDate,
                    ReturnDate = specialOffer.ToDate,
                    TakeOverCity = specialOffer.Car.RentACarService == null ?
                                    specialOffer.Car.Branch.City : specialOffer.Car.RentACarService.Address.City,
                    ReturnCity = specialOffer.Car.RentACarService == null ?
                                    specialOffer.Car.Branch.City : specialOffer.Car.RentACarService.Address.City,
                    RentedCar = specialOffer.Car,
                    TotalPrice = specialOffer.NewPrice,
                    RentDate = DateTime.Now
                };

                user.CarRents.Add(carRent);
                specialOffer.Car.Rents.Add(carRent);
                specialOffer.IsReserved = true;

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.CarRepository.Update(specialOffer.Car);
                    unitOfWork.RACSSpecialOfferRepository.Update(specialOffer);

                    await unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Car is modified in the meantime, or reserved by another user");
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
                return StatusCode(500, "Failed to reserve special offer");
            }
        }

        [HttpPost]
        [Route("reserve-special-offer-flight")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ReserveSpecialOfferFlight([FromBody] ReserveFlightDto dto)
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

                var specialOffer = await unitOfWork.SpecialOfferRepository.GetSpecialOfferById(dto.Id);

                if (specialOffer == null)
                {
                    return NotFound("Selected special offer is not found");
                }

                if (specialOffer.IsReserved)
                {
                    return BadRequest("Already reserved");
                }

                var flightReservation = new FlightReservation() { 
                    User = user,
                    Price = specialOffer.NewPrice,
                    ReservationDate = DateTime.Now
                };

                var tickets = new List<Ticket>();

                foreach (var seat in specialOffer.Seats)
                {
                    if (seat.Flight.TakeOffDateTime < DateTime.Now)
                    {
                        return BadRequest("One of flights is in past");
                    }

                    tickets.Add(new Ticket() {
                        Seat = seat,
                        SeatId = seat.SeatId,
                        Reservation = flightReservation,
                        Passport = dto.Passport
                    });
                }

                flightReservation.Tickets = tickets;

                user.FlightReservations.Add(flightReservation);
                specialOffer.IsReserved = true;

                try
                {
                    unitOfWork.SpecialOfferRepository.Update(specialOffer);
                    unitOfWork.UserRepository.Update(user);

                    await unitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return BadRequest("Something is modified in the meantime, or reserved by another user");
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to reserve special offer");
                }

                try
                {
                    await unitOfWork.AuthenticationRepository.SendTicketConfirmationMail(user, flightReservation);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Successfully reserved, but failed to send email");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to reserve special offer");
            }
        }
        #endregion

    }
}
