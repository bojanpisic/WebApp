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
    public class SystemAdminController : Controller
    {
        private readonly ISystemAdminRepository _repository;
        private readonly IAuthenticationRepository _authenticationRepository;

        private readonly DataContext _context;
        private readonly UserManager<Person> _userManager;

        public SystemAdminController(DataContext dbContext, UserManager<Person> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _authenticationRepository = new AuthenticationRepository(dbContext, userManager, roleManager);
            _repository = new SystemAdminRepository(dbContext);
        }


        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("register-airline")]
        public async Task<IActionResult> RegisterAirlineAdmin([FromBody]RegisterAirlineAdminDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is invalid.");
            }

            try
            {
                if (!_authenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
                {
                    return BadRequest(new IdentityError() { Code = "Passwords dont match" });
                }
                var admin = new AirlineAdmin()
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                    //FirstName = registerDto.FirstName,
                    //LastName = registerDto.LastName,
                    //ImageUrl = registerDto.ImageUrl,
                    //PhoneNumber = registerDto.Phone,
                    //City = registerDto.City,
                    //EmailConfirmed = false,
                    //Airline = null,
                };

                var airline = new Airline()
                {
                    Name = registerDto.Name,
                    Address = registerDto.Address,
                    Admin = (AirlineAdmin)admin
                };

                admin.Airline = airline;

                var result = await this._authenticationRepository.RegisterAirlineAdmin(admin, registerDto.Password);

                if (!result.Succeeded)
                {
                    var err = result.Errors.ToList()[0];
                    return BadRequest(new { message = err.Description });
                }

                var addToRoleResult = await _authenticationRepository.AddToRole(admin, "AirlineAdmin");

                if (!addToRoleResult.Succeeded)
                {
                    return BadRequest(result);
                }

                var emailSent = await _authenticationRepository.SendConfirmationMail(admin, "admin");

                return Ok(result);

                //var result = await _repository.CreateAirlineForAdmin(airline);

                //if (result.Succeeded)
                //{
                //    return Ok(result);
                //}
                //else 
                //{
                //    return BadRequest(result.Errors);
                //}

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }


    }
}
