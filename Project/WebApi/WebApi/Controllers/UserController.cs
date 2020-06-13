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
        private readonly IUserRepository _userRepository;
        private readonly DataContext _context;
        private readonly IAirlineRepository _airlineRepository;
        private readonly UserManager<Person> _userManager;
        private readonly IAuthenticationRepository _authenticationRepository;



        public UserController(DataContext dbContext, UserManager<Person> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _userRepository = new UserRepository(dbContext, userManager);
            _authenticationRepository = new AuthenticationRepository(dbContext, userManager);
        }

        [HttpPost]
        [Route("send-friendship-invitation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> SendFriendshipInvitation([FromBody]AddFriendDto dto)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var friend = await _userManager.FindByIdAsync(dto.UserId);

                if (friend == null)
                {
                    return BadRequest(new IdentityError() { Description = "User doesnt exist"});
                }

                var result = await _userRepository.CreateFriendshipInvitation(user, friend);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return BadRequest();

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
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
                var user = (User)await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var requests = await _userRepository.GetRequests(user);

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

                return StatusCode(500, "Internal server error");
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
                var user = (User)await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var allUsers = await _userRepository.GetAllUsers();
                var usersToReturn = new List<object>();
                foreach (var item in allUsers)
                {
                    var role = await _authenticationRepository.GetRoles(item);
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
                return StatusCode(500, "Internal server error");
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
                var user = (User)await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                //var friends = await _userRepository.GetFriends(user);
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
                return StatusCode(500, "Internal server error");
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
                var user = (User)await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var friendship = await _userRepository.GetRequestWhere(userId, dto.UserId);

                if (friendship == null)
                {
                    return BadRequest();
                }

                user.Friends.Add(friendship.User1);
                user.FriendshipRequests.FirstOrDefault(f => f.User1Id == friendship.User1.Id && f.User2Id == friendship.User2.Id).Accepted = true;
                friendship.User1.Friends.Add(user);

                using (var transaction = _context.Database.BeginTransactionAsync()) 
                {
                    try
                    {
                        await _userRepository.UpdateUser(user);
                        await _userRepository.UpdateUser(friendship.User1);

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

        [HttpPost]
        [Route("reject-request")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RejectRequest([FromBody] AddFriendDto dto) 
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = (User)await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RegularUser"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var friendship = await _userRepository.GetRequestWhere(userId, dto.UserId);

                if (friendship == null)
                {
                    return BadRequest();
                }

                //user.FriendshipRequests.Remove(friendship);
                //friendship.User1.FriendshipInvitations.Remove(friendship);

                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _userRepository.DeleteFriendship(friendship);
                        await _userRepository.UpdateUser(user);
                        await _userRepository.UpdateUser(friendship.User1);

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

    }
}
