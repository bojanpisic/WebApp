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

        [HttpPost("register-systemadmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> RegisterSystemAdmin([FromBody] RegisterSystemAdminDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Person createUser;

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                //if ((await unitOfWork.AuthenticationRepository.GetPersonByEmail(userDto.Email)) != null)
                //{
                //    return BadRequest("User with that email already exists!");
                //}
                if ((await unitOfWork.AuthenticationRepository.GetPersonByUserName(userDto.UserName)) != null)
                {
                    return BadRequest("User with that usermane already exists!");
                }

                if (userDto.Password.Length > 20 || userDto.Password.Length < 8)
                {
                    return BadRequest("Password length has to be between 8-20");
                }

                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(userDto.Password, userDto.ConfirmPassword))
                {
                    return BadRequest("Passwords dont match");
                }
                createUser = new Person()
                {
                    Email = userDto.Email,
                    UserName = userDto.UserName,
                };
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to register system admin");
            }

            //using (var transaction = new TransactionScope())
            try
            {
                var result = await this.unitOfWork.AuthenticationRepository.RegisterSystemAdmin(createUser, userDto.Password);
                //unitOfWork.Commit();

                var addToRoleResult = await unitOfWork.AuthenticationRepository.AddToRole(createUser, "Admin");
                await unitOfWork.Commit();

                //await transaction.Result.CommitAsync();
            }
            catch (Exception)
            {
                //unitOfWork.Rollback();
                //await transaction.Result.RollbackAsync();
                return StatusCode(500, "Internal server error. Registration failed.");
            }

            try
            {
                var emailSent = await unitOfWork.AuthenticationRepository.SendConfirmationMail(createUser, "admin", userDto.Password);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Sending email failed.");
            }

            return Ok();
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
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                //if ((await unitOfWork.AuthenticationRepository.GetPersonByEmail(registerDto.Email)) != null)
                //{
                //    return BadRequest("User with that email already exists!");
                //}
                if ((await unitOfWork.AuthenticationRepository.GetPersonByUserName(registerDto.UserName)) != null)
                {
                    return BadRequest("User with that usermane already exists!");
                }
                if (registerDto.Password.Length > 20 || registerDto.Password.Length < 8)
                {
                    return BadRequest("Password length has to be between 8-20");
                }
                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
                {
                    return BadRequest("Passwords dont match");
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
                return Ok();
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
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                //if ((await unitOfWork.AuthenticationRepository.GetPersonByEmail(registerDto.Email)) != null)
                //{
                //    return BadRequest("User with that email already exists!");
                //}
                if ((await unitOfWork.AuthenticationRepository.GetPersonByUserName(registerDto.UserName)) != null)
                {
                    return BadRequest("User with that usermane already exists!");
                }
                if (registerDto.Password.Length > 20 || registerDto.Password.Length < 8)
                {
                    return BadRequest("Password length has to be between 8-20");
                }
                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(registerDto.Password, registerDto.ConfirmPassword))
                {
                    return BadRequest("Passwords dont match");
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

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to register racs admin");
            }
        }

        [HttpPost]
        [Route("set-bonus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> SetBonus([FromBody] BonusDto dto) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (dto.Bonus < 0 || dto.Discount < 0 || dto.Discount > 100)
                {
                    return BadRequest("Inputs are not valid");
                }

                var bonus = await unitOfWork.BonusRepository.GetByID(1);

                if (bonus == null)
                {
                    bonus = new Bonus()
                    {
                        BonusPerKilometer = dto.Bonus,
                        DiscountPerReservation = dto.Discount
                    };

                    try
                    {
                        await unitOfWork.BonusRepository.Insert(bonus);
                        await unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        return StatusCode(500, "Transaction failed");
                    }
                    return Ok();
                }

                bonus.BonusPerKilometer = dto.Bonus;
                bonus.DiscountPerReservation = dto.Discount;

                try
                {
                    unitOfWork.BonusRepository.Update(bonus);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Transaction failed");
                }

                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to set bonus");
            }
        }


        [HttpGet]
        [Route("get-bonus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetBonus()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (!userRole.Equals("Admin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var bonus = await unitOfWork.BonusRepository.GetByID(1);

                if (bonus == null)
                {
                    return Ok(new { bonus = 0, discount = 0});

                }

                var retVal = new 
                {
                    bonus = bonus.BonusPerKilometer,
                    discount = bonus.DiscountPerReservation,
                };

                return Ok(retVal);

            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to set bonus");
            }
        }
    }
}
