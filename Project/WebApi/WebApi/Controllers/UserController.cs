using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
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

                var res = await unitOfWork.CarRepository.Get(c => c.CarId == dto.CarRentId, null, "Branch, Rents");
                var car = res.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                foreach (var rent in car.Rents)
                {
                    if (!(rent.TakeOverDate < dto.TakeOverDate && rent.ReturnDate < dto.TakeOverDate ||
                        rent.TakeOverDate > dto.ReturnDate && rent.ReturnDate > dto.ReturnDate))
                    {
                        return BadRequest("The selected car is reserver for selected period");
                    }
                }

                var racsId = car.BranchId == null ? car.RentACarServiceId : car.Branch.RentACarServiceId;

                var result = await unitOfWork.RentACarRepository.Get(r => r.RentACarServiceId ==racsId, null, "Address, Branches");
                var racs = result.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
                }

                if ((!car.Branch.City.Equals(dto.TakeOverCity) && car.BranchId != null) || !racs.Address.City.Equals(dto.TakeOverCity))
                {
                    return BadRequest("Takeover city and rent service/branch city dont match");
                }

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
                    User = user
                };

                user.CarRents.Add(carRent);
                car.Rents.Add(carRent);

                try
                {
                    unitOfWork.UserRepository.Update(user);
                    unitOfWork.CarRepository.Update(car);

                    await unitOfWork.Commit();
                    //await transaction.Result.CommitAsync();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    //await transaction.Result.RollbackAsync();
                    return StatusCode(500, "Failed to rent car. One of transactions failed");
                }
                //}

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to rent car");
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

                var rent = await unitOfWork.UserRepository.GetRent(id);

                var car = rent.RentedCar;

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                if ((rent.TakeOverDate - DateTime.Now).TotalDays < 2)
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
                    //await transaction.Result.CommitAsync();
                }
                catch (Exception)
                {
                    //unitOfWork.Rollback();
                    //await transaction.Result.RollbackAsync();
                    return StatusCode(500, "Failed to cancel rent car. One of transactions failed");
                }
                //}

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to cancel rent car");
            }
        }

        [HttpGet]
        [Route("get-rents")]
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

                var rent = await unitOfWork.UserRepository.GetRents(user);
                //nesto treba vratiti

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to cancel rent car");
            }
        }

        //[HttpPost]
        //[Route("rate-car")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> RateCar(CarRateDto dto) 
        //{
        //    try
        //    {
        //        string userId = User.Claims.First(c => c.Type == "UserID").Value;
        //        var user = (User)await unitOfWork.UserManager.FindByIdAsync(userId);

        //        string userRole = User.Claims.First(c => c.Type == "Roles").Value;

        //        if (!userRole.Equals("RegularUser"))
        //        {
        //            return Unauthorized();
        //        }

        //        if (user == null)
        //        {
        //            return NotFound("User not found");
        //        }

        //        var rent = await unitOfWork.UserRepository.GetRents(user);
        //        //nesto treba vratiti

        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Failed to rate car");
        //    }
        //}
        #endregion
    }
}
