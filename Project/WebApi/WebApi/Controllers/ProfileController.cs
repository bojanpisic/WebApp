using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProfileController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly IAuthenticationRepository _authRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly SignInManager<Person> _signInManager;
        private readonly RoleManager<IdentityRole> roleManager;



        private readonly DataContext _context;
        public ProfileController(UserManager<Person> userManager, DataContext context, SignInManager<Person> signInManager, RoleManager<IdentityRole> rm)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _profileRepository = new ProfileRepository(context, userManager, signInManager);
            _authRepository = new AuthenticationRepository(context, userManager, rm);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout() 
        {
            await _profileRepository.Logout();

            return Ok();
        }

        [HttpPut]
        [Route("change-city/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


        public async Task<IActionResult> ChangeCity([FromBody]ChangeCityDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

                //var user = await _authRepository.GetUserById(id.ToString());

                //if (user == null)
                //{
                //    return Unauthorized();
                //}

                user.City = profile.City;

                var result = await _profileRepository.ChangeProfile(user);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
                
                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-email/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]


        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                //var user = await _authRepository.GetUserById(id.ToString());

                //if (user == null)
                //{
                //    return Unauthorized();
                //}

                var result = await _profileRepository.ChangeEmail(user, profile.Email);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-firstname/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeFirstName([FromBody] ChangeFirstNameDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                //var user = await _authRepository.GetUserById(id.ToString());

                //if (user == null)
                //{
                //    return Unauthorized();
                //}

                user.FirstName = profile.FirstName;

                var result = await _profileRepository.ChangeProfile(user);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-lastname/{id}")]
        public async Task<IActionResult> ChangeLastName([FromBody] ChangeLastNameDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                //var user = await _authRepository.GetUserById(id.ToString());

                //if (user == null)
                //{
                //    return Unauthorized();
                //}

                user.LastName = profile.LastName;

                var result = await _profileRepository.ChangeProfile(user);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-img/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeImgUrl(string id, IFormFile img)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
                //var user = await _authRepository.GetUserById(id.ToString());

                //if (user == null)
                //{
                //    return Unauthorized();
                //}

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    user.ImageUrl = stream.ToArray();
                }

                var result = await _profileRepository.ChangeProfile(user);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-passw/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authRepository.GetUserById(id);

                if (user == null)
                {
                    return Unauthorized();
                }

                var oldPasswMatch = await _userManager.CheckPasswordAsync(user, profile.OldPassword);

                if (!oldPasswMatch)
                {
                    return BadRequest(new IdentityError() { Description = "Old Password dont match"});

                }

                if (!profile.Password.Equals(profile.PasswordConfirm))
                {
                    return BadRequest(new IdentityError() { Description = "Passwords dont match" });
                }

                var result = await _profileRepository.ChangePassword(user, profile.Password);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-phone/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authRepository.GetUserById(id.ToString());

                if (user == null)
                {
                    return Unauthorized();
                }

                var result = await _profileRepository.ChangePhone(user, profile.Phone);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }

        [HttpPut]
        [Route("change-username/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeUsername([FromBody] ChangeUserNameDto profile, string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _authRepository.GetUserById(id.ToString());

                if (user == null)
                {
                    return Unauthorized();
                }

                var result = await _profileRepository.ChangeUserName(user, profile.UserName);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");

            }
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("get-profile")]
        //GET : /api/UserProfile
        public async Task<object> GetUserProfile()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                return new
                {
                    user.City,
                    user.Email,
                    user.UserName,
                    user.ImageUrl,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber
                };
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
