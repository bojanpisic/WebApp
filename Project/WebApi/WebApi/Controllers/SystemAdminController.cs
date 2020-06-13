using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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


        [HttpPost]
        [Route("register-airline")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> RegisterAirlineAdmin([FromBody]RegisterAirlineAdminDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is invalid.");
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                if (!_authenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
                {
                    return BadRequest(new IdentityError() { Description = "Passwords dont match" });
                }
                var admin = new AirlineAdmin()
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                };
                //var address = new Address() { 
                //    City = registerDto.Address.City,
                //    State = registerDto.Address.State,
                //    Lat = registerDto.Address.Lat,
                //    Lon = registerDto.Address.Lon
                //};
                var airline = new Airline()
                {
                    Name = registerDto.Name,
                    Address = new Address() { City = registerDto.Address.City, State = registerDto.Address.State,
                        Lat = registerDto.Address.Lat, Lon = registerDto .Address.Lon},
                    Admin = (AirlineAdmin)admin
                };

                admin.Airline = airline;

                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var result = await this._authenticationRepository.RegisterAirlineAdmin(admin, registerDto.Password);

                        if (!result.Succeeded)
                        {
                            return BadRequest(new IdentityError() { Description = "Failed to register admin" });

                        }

                        var addToRoleResult = await _authenticationRepository.AddToRole(admin, "AirlineAdmin");

                        if (!addToRoleResult.Succeeded)
                        {
                            return BadRequest(result);
                        }
                        await transaction.Result.CommitAsync();

                    }
                    catch (Exception)
                    {
                        await transaction.Result.RollbackAsync();
                        return StatusCode(500, "Internal server error");
                    }
                }

                var emailSent = await _authenticationRepository.SendConfirmationMail(admin, "admin", registerDto.Password);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("register-racservice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> RegisterRACSAdmin([FromBody] RegisterRACSAdminDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is invalid.");
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                if (!_authenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
                {
                    return BadRequest(new IdentityError() { Description = "Passwords dont match" });
                }
                var admin = new RentACarServiceAdmin()
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                };
                var racs = new RentACarService()
                {
                    Name = registerDto.Name,
                    Address = new Address2()
                    {
                        City = registerDto.Address.City,
                        State = registerDto.Address.State,
                        Lat = registerDto.Address.Lat,
                        Lon = registerDto.Address.Lon,
                    },
                    Admin = (RentACarServiceAdmin)admin
                };

                admin.RentACarService = racs;
                using (var transaction = _context.Database.BeginTransactionAsync())
                {

                    try
                    {
                        var result = await this._authenticationRepository.RegisterRACSAdmin(admin, registerDto.Password);

                        if (!result.Succeeded)
                        {
                            return BadRequest(new IdentityError() { Description = "Failed to register admin" });
                        }

                        var addToRoleResult = await _authenticationRepository.AddToRole(admin, "RentACarServiceAdmin");

                        if (!addToRoleResult.Succeeded)
                        {
                            return BadRequest(result);
                        }

                        await transaction.Result.CommitAsync();

                    }
                    catch (Exception)
                    {
                        await transaction.Result.RollbackAsync();
                        return StatusCode(500, "Internal server error");
                    }
                }

                var emailSent = await _authenticationRepository.SendConfirmationMail(admin, "admin", registerDto.Password);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
