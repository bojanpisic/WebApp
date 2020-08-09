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
        private IUnitOfWork unitOfWork;
        public SystemAdminController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
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

                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
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
                    Address = new Address() 
                    { 
                        City = registerDto.Address.City, 
                        State = registerDto.Address.State,
                        Lat = registerDto.Address.Lat, 
                        Lon = registerDto .Address.Lon
                    },
                    Admin = (AirlineAdmin)admin
                };

                admin.Airline = airline;

                //using (var transaction = new TransactionScope())
                //{
                    try
                    {
                        await this.unitOfWork.AuthenticationRepository.RegisterAirlineAdmin(admin, registerDto.Password);
                        await unitOfWork.AuthenticationRepository.AddToRole(admin, "AirlineAdmin");
                        //await transaction.Result.CommitAsync();
                        await unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                    //await transaction.Result.RollbackAsync();
                    //unitOfWork.Rollback();
                    //transaction.Dispose();
                        return StatusCode(500, "Failed to register airline admin. One of transactions failed");
                    }
                //}

                return StatusCode(201, "Registered");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to register airline admin");
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

                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
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
                //using (var transaction = new TransactionScope())
                //{

                try
                {
                    await this.unitOfWork.AuthenticationRepository.RegisterRACSAdmin(admin, registerDto.Password);
                    await unitOfWork.AuthenticationRepository.AddToRole(admin, "RentACarServiceAdmin");
                    await unitOfWork.Commit();
                    //await transaction.Result.CommitAsync();
                }
                catch (Exception)
                {
                    //await transaction.Result.RollbackAsync();
                    //unitOfWork.Rollback();
                    return StatusCode(500, "Failed to register racs admin. One of transactions failed");
                }
                //}

                try
                {
                    var emailSent = await unitOfWork.AuthenticationRepository.SendConfirmationMail(admin, "admin", registerDto.Password);
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to send registration email");
                }

                return StatusCode(201, "Registered");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to register racs admin");
            }
        }
    }
}
