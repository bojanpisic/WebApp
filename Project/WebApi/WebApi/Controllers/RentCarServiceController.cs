using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class RentCarServiceController : Controller
    {
        private readonly IRentCarServiceRepository _rentCarServiceRepository;
        private readonly IAuthenticationRepository _authenticationRepository;

        private readonly DataContext _context;
        private readonly UserManager<Person> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RentCarServiceController(DataContext dbContext, UserManager<Person> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = dbContext;
            _userManager = userManager;
            _rentCarServiceRepository = new RentCarServiceRepository(dbContext, userManager);
            _authenticationRepository = new AuthenticationRepository(dbContext, userManager, roleManager);
        }
        [HttpGet]
        [Route("get-racs-address")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRACSAddress()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "RACS not found" });
                }

                object obj = new
                {
                    city = racs.Address.City,
                    state = racs.Address.State
                };

                return Ok(obj);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-racs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetAdminsRACSProfile()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "RACS not found" });
                }

                object obj = new
                {
                    racs.Name,
                    racs.About,
                    racs.Address,
                    racs.LogoUrl
                };

                return Ok(obj);

            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut]
        [Route("change-racs-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeRACSInfo(int id, ChangeRACSInfoDto dto)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest(new IdentityError() { Code = "", Description = "id empty" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return BadRequest(new IdentityError() { Description = "Racs doesnt exist" });
                }

                racs.Name = dto.Name;
                racs.About = dto.PromoDescription;

                bool addressChanged = false;

                if (!racs.Address.City.Equals(dto.Address.City) || !racs.Address.City.Equals(dto.Address.State) ||
                    !racs.Address.Lat.Equals(dto.Address.Lat) || !racs.Address.Lon.Equals(dto.Address.Lon))
                {
                    racs.Address.City = dto.Address.City;
                    racs.Address.State = dto.Address.State;
                    racs.Address.Lon = dto.Address.Lon;
                    racs.Address.Lat = dto.Address.Lat;
                    addressChanged = true;
                }
                var result = IdentityResult.Success;

                if (addressChanged)
                {
                    using (var transaction = _context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var res = await _rentCarServiceRepository.UpdateRACS(racs);
                            var res2 = await _rentCarServiceRepository.UpdateAddress(racs.Address);

                            await transaction.Result.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.Result.RollbackAsync();
                            throw;
                        }
                    }
                }
                else
                {
                    result = await _rentCarServiceRepository.UpdateRACS(racs);
                }
                //racs.Address = dto.Address;


                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error!");
            }
        }

        [HttpPut]
        [Route("change-racs-logo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeRACSLogo(IFormFile img)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Racs doesnt exist" });
                }

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    racs.LogoUrl = stream.ToArray();
                }

                var res = await _rentCarServiceRepository.UpdateRACS(racs);

                if (res.Succeeded)
                {
                    return Ok(res);

                }
                return BadRequest(res.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-racs-branches")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRACSBranches()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Rent Service not found" });
                }

                var branches = racs.Branches;

                List<object> objs = new List<object>();

                foreach (var item in branches)
                {
                    objs.Add(new { item.City, item.BranchId, item.State });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-racs-cars")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRACSCars()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSAndCars(userId);

                if (racs == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Airline not found" });
                }

                var allCars = racs.Cars;

                List<object> objs = new List<object>();

                foreach (var item in allCars)
                {
                    objs.Add(new
                    {
                        racs.Name,
                        item.CarId,
                        item.ImageUrl,
                        item.Model,
                        item.PricePerDay,
                        item.SeatsNumber,
                        item.Type,
                        item.Year,
                        item.Brand,
                        racs.Address.City,
                        racs.Address.State
                    });
                }

                foreach (var branch in racs.Branches)
                {
                    foreach (var car in branch.Cars)
                    {
                        objs.Add(new
                        {
                            racs.Name,
                            car.CarId,
                            car.ImageUrl,
                            car.Model,
                            car.PricePerDay,
                            car.SeatsNumber,
                            car.Type,
                            car.Year,
                            car.Brand,
                            branch.City,
                            branch.State
                        });
                    }
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-branch-cars/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBranchCars(int id)
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await _userManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "400", Description = "User not found" });
                }

                var branch = await _rentCarServiceRepository.GetBranchById(id);

                if (branch == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Branch not found" });
                }

                var allCars = branch.Cars;

                List<object> objs = new List<object>();

                foreach (var item in allCars)
                {
                    objs.Add(new
                    {
                        branch.RentACarService.Name,
                        item.CarId,
                        item.ImageUrl,
                        item.Model,
                        item.PricePerDay,
                        item.SeatsNumber,
                        item.Type,
                        item.Year,
                        item.Brand
                    });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("add-branch")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddBranch(BranchDto branchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Racs not found" });
                }

                var branch = new Branch()
                {
                    RentACarService = racs,
                    City = branchDto.City,
                    State = branchDto.State
                };

                var result = await _rentCarServiceRepository.AddBranch(branch);

                var allBranches = await _rentCarServiceRepository.GetRACSBranches(racs);

                if (result.Succeeded)
                {
                    List<object> objs = new List<object>();

                    foreach (var item in allBranches)
                    {
                        objs.Add(new { item.City, item.BranchId, item.State });
                    }

                    return Ok(objs);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }

        }

        [HttpDelete]
        [Route("delete-branch/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var branch = await _rentCarServiceRepository.GetBranchById(id);

                if (branch == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Branch not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Racs not found" });
                }

                foreach (var car in branch.Cars)
                {
                    racs.Cars.Add(car);
                }

                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _rentCarServiceRepository.UpdateRACS(racs);
                        await _rentCarServiceRepository.DeleteBranch(branch);

                        await transaction.Result.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.Result.RollbackAsync();
                        return StatusCode(500, "Internal server error");

                    }
                }

                List<object> objs = new List<object>();

                foreach (var item in racs.Branches)
                {
                    objs.Add(new { item.City, item.BranchId, item.State });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost]
        [Route("add-car")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddCarToService([FromBody] CarDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Racs not found" });
                }
                var car = new Car()
                {
                    RentACarService = racs,
                    Branch = null,
                    Model = carDto.Model,
                    PricePerDay = carDto.PricePerDay,
                    Year = carDto.Year,
                    SeatsNumber = carDto.SeatsNumber,
                    Type = carDto.Type,
                    Brand = carDto.Brand
                };
                //using (var stream = new MemoryStream())
                //{
                //    await img.CopyToAsync(stream);
                //    car.ImageUrl = stream.ToArray();
                //}



                var result = await _rentCarServiceRepository.AddCar(car);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }
        }

        [HttpPost]
        [Route("add-car-to-branch")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddCarToBranch([FromBody] CarDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var branch = await _rentCarServiceRepository.GetBranchById(carDto.BranchId);

                if (branch == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Racs not found" });
                }
                var car = new Car()
                {
                    RentACarService = null,
                    Branch = branch,
                    Model = carDto.Model,
                    PricePerDay = carDto.PricePerDay,
                    Year = carDto.Year,
                    SeatsNumber = carDto.SeatsNumber,
                    Type = carDto.Type,
                    Brand = carDto.Brand
                };

                var result = await _rentCarServiceRepository.AddCar(car);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server error");
            }
        }

        [HttpPut]
        [Route("change-car-info/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeCarInfo(int id, ChangeCarDto dto)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest(new IdentityError() { Code = "", Description = "id empty" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var car = await _rentCarServiceRepository.GetRACSCarById(id);

                if (car == null)
                {
                    return BadRequest(new IdentityError() { Description = "Airline doesnt exist" });
                }

                car.Brand = dto.Brand;
                car.Model = dto.Model;
                car.PricePerDay = dto.PricePerDay;
                car.SeatsNumber = dto.SeatsNumber;
                car.Type = dto.Type;
                car.Year = dto.Year;

                var result = await _rentCarServiceRepository.UpdateCar(car);

                if (result.Succeeded)
                {
                    return Ok(result);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error!");
            }
        }

        [HttpPut]
        [Route("change-car-image/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeCarImage(IFormFile img, int id)
        {
            if (img == null)
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var car = await _rentCarServiceRepository.GetRACSCarById(id);

                if (car == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Car doesnt exist" });
                }

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    car.ImageUrl = stream.ToArray();
                }

                var res = await _rentCarServiceRepository.UpdateCar(car);

                if (res.Succeeded)
                {
                    return Ok(res);

                }
                return BadRequest(res.Errors);

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("get-car/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCar(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var car = await _rentCarServiceRepository.GetRACSCarById(id);

                if (car == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Car doesnt exist" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "RACS not found" });
                }

                List<object> specOffDates = new List<object>();

                foreach (var item in car.SpecialOffers)
                {
                    specOffDates.Add(new { From = item.FromDate, To = item.ToDate });
                }

                var obj = new
                {
                    racs.Name,
                    car.CarId,
                    car.ImageUrl,
                    car.Model,
                    car.PricePerDay,
                    car.SeatsNumber,
                    car.Type,
                    car.Year,
                    car.Brand,
                    SpecialOfferDatesOfCar = specOffDates
                };

                return Ok(obj);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete]
        [Route("delete-car/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest();
            }
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var car = await _rentCarServiceRepository.GetRACSCarById(id);

                if (car == null)
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Car doesnt exist" });
                }

                var result = await _rentCarServiceRepository.DeleteCar(car);

                if (!result.Succeeded)
                {
                    return BadRequest();
                }
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("add-car-specialoffer/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> AddSpecialOffer([FromBody] CarSpecialOfferDto specialOfferDto, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "User not found" });
                }

                var car = await _rentCarServiceRepository.GetRACSCarById(id);

                if (car == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "car not found" });
                }

                var oldSpecOffers = await _rentCarServiceRepository.GetSpecialOffersOfCar(car);

                var specialFromDate = Convert.ToDateTime(specialOfferDto.FromDate);
                var specialToDate = Convert.ToDateTime(specialOfferDto.ToDate);


                foreach (var item in oldSpecOffers)
                {
                    if (item.FromDate >= specialFromDate && item.ToDate <= specialFromDate
                        || item.FromDate >= specialToDate && item.ToDate <= specialToDate
                        || item.FromDate <= specialFromDate && item.ToDate >= specialToDate)
                    {
                        return BadRequest(new IdentityError() { Description = "Dates are unavailable. Car has another special offers in that time." });
                    }
                }


                var specialOffer = new CarSpecialOffer()
                {
                    Car = car,
                    NewPrice = specialOfferDto.NewPrice,
                    FromDate = specialFromDate,
                    ToDate = specialToDate
                };

                var oldPrice = Math.Abs(specialFromDate.Day - specialToDate.Day + 1) * car.PricePerDay;

                specialOffer.OldPrice = oldPrice;

                car.SpecialOffers.Add(specialOffer);

                using (var transaction = _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        await _rentCarServiceRepository.AddSpecOffer(specialOffer);
                        await _rentCarServiceRepository.UpdateCar(car);

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

        [HttpGet]
        [Route("get-cars-specialoffers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetSpecialOffers()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Seat not found" });
                }

                var racs = await _rentCarServiceRepository.GetRACSByAdmin(userId);

                if (racs == null)
                {
                    return NotFound(new IdentityError() { Code = "404", Description = "Airline not found" });
                }

                var specOffers = await _rentCarServiceRepository.GetSpecialOffersOfRacs(racs.RentACarServiceId);

                List<object> objs = new List<object>();

                foreach (var item in specOffers)
                {
                    objs.Add(new
                    {
                        racs.Name,
                        item.NewPrice,
                        item.OldPrice,
                        FromDate = item.FromDate.Date,
                        ToDate = item.ToDate.Date,
                        item.Car.Brand,
                        item.Car.CarId,
                        item.Car.ImageUrl,
                        item.Car.Model,
                        item.Car.SeatsNumber,
                        item.Car.Type,
                        item.Car.Year
                    });
                }

                return Ok(objs);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }


        //user methods

        [HttpGet]
        [Route("rent-car-services")]

        public async Task<IActionResult> RentCarServices()
        {

            try
            {

                var queryString = Request.Query;
                var sortType = queryString["typename"].ToString();
                var sortByName = queryString["sortbyname"].ToString();
                var sortTypeCity = queryString["typecity"].ToString();
                var sortByCity = queryString["sortbycity"].ToString();

                var services = await _rentCarServiceRepository.GetAllRACS();

                if (!String.IsNullOrEmpty(sortByCity) && !String.IsNullOrEmpty(sortTypeCity))
                {
                    if (sortType.Equals("ascending"))
                    {
                        services.OrderBy(a => a.Address.City);

                    }
                    else
                    {
                        services.OrderByDescending(a => a.Address.City);
                    }
                }
                if (!String.IsNullOrEmpty(sortByName) && !String.IsNullOrEmpty(sortType))
                {
                    if (sortType.Equals("ascending"))
                    {
                        services.OrderBy(a => a.Name);
                    }
                    else
                    {
                        services.OrderByDescending(a => a.Name);
                    }
                }


                List<object> retList = new List<object>();
                List<object> branches = new List<object>();

                foreach (var item in services)
                {
                    branches = new List<object>();
                    foreach (var d in item.Branches)
                    {
                        branches.Add(new
                        {
                            City = d.City,
                            State = d.State
                        });
                    }
                    retList.Add(new
                    {
                        Name = item.Name,
                        Logo = item.LogoUrl,
                        City = item.Address.City,
                        State = item.Address.State,
                        //Lon = item.Address.Lon,
                        //Lat = item.Address.Lat,
                        About = item.About,
                        Id = item.RentACarServiceId,
                        Branches = branches
                    });
                }

                return Ok(retList);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("rent-car-service/{id}")]
        public async Task<IActionResult> GetRentCarService(int id)
        {
            try
            {
                var rentservice = await _rentCarServiceRepository.RACSByIdInclude(id);

                List<object> branches = new List<object>();
                foreach (var item in rentservice.Branches)
                {
                    branches.Add(new {
                        item.City, BranchId = item.BranchId
                    });
                }

                object obj = new
                {
                    Name = rentservice.Name,
                    About = rentservice.About,
                    City = rentservice.Address.City,
                    State = rentservice.Address.State,
                    Lat = rentservice.Address.Lat,
                    Lon = rentservice.Address.Lon,
                    Logo = rentservice.LogoUrl,
                    Id = rentservice.RentACarServiceId,
                    Branches = branches
                };

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("cars")]
        public async Task<IActionResult> GetAllCars()
        {
            try
            {

                var queryString = Request.Query;
                var fromDate = queryString["dep"].ToString();
                var toDate = queryString["ret"].ToString();
                DateTime DateFrom = DateTime.Now;
                DateTime DateTo = DateTime.Now;

                if (!String.IsNullOrEmpty(fromDate))
                {
                    DateFrom = Convert.ToDateTime(fromDate);
                }
                if (!String.IsNullOrEmpty(toDate))
                {
                    DateTo = Convert.ToDateTime(toDate);
                }

                var fromCity = queryString["from"].ToString();
                var toCity = queryString["to"].ToString();
                var carType = queryString["type"].ToString();
                var seatFrom = Int32.Parse(queryString["seatfrom"].ToString());
                var seatTo = Int32.Parse(queryString["seatto"].ToString());

                float priceFrom = 0;
                float priceTo = 3000;

                if (String.IsNullOrEmpty(queryString["minprice"].ToString()))
                {
                    float.TryParse(queryString["minprice"].ToString(), out priceFrom);
                }

                if (String.IsNullOrEmpty(queryString["maxprice"].ToString()))
                {
                    float.TryParse(queryString["maxprice"].ToString(), out priceTo);
                }

                List<int> ids = new List<int>();
                if (!String.IsNullOrEmpty(queryString["racs"].ToString()))
                {
                    foreach (var item in queryString["racs"].ToString().Split(','))
                    {
                        ids.Add(int.Parse(item));
                    }
                }



                var allCars = await _rentCarServiceRepository.AllCars();

                List<object> objs = new List<object>();
                RentACarService rentService = new RentACarService();
                Branch branch = new Branch();
                Address2 address = new Address2();

                string name;
                byte[] logo;

                foreach (var item in allCars)
                {

                    if (item.Branch == null)
                    {
                        rentService = item.RentACarService;
                        address = rentService.Address;
                        logo = rentService.LogoUrl;
                        name = rentService.Name;
                    }
                    else
                    {
                        branch = item.Branch;
                        address.City = branch.City;
                        address.State = branch.State;
                        logo = branch.RentACarService.LogoUrl;
                        name = branch.RentACarService.Name;
                    }

                    if (!FilterPass(item, ids, priceFrom, priceTo, fromCity, toCity, carType, DateFrom.Date, DateTo.Date, seatFrom, seatTo)) 
                    {
                        continue;
                    }

                    objs.Add(new
                    {

                        item.CarId,
                        item.ImageUrl,
                        item.Model,
                        item.PricePerDay,
                        item.SeatsNumber,
                        item.Type,
                        item.Year,
                        item.Brand,
                        address.City,
                        address.State,
                        logo,
                        name
                    });
                }

                return Ok(objs);
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("car/{id}")]
        public async Task<IActionResult> Car(int id)
        {
            try
            {
                var car = await _rentCarServiceRepository.GetCarById(id);
                //string name;
                //byte[] logo;

                //if (car.Branch == null)
                //{
                //    name = car.RentACarService.Name;
                //    logo = car.RentACarService.LogoUrl;
                //}
                //else
                //{
                //    name = car.Branch.RentACarService.Name;
                //    logo = car.Branch.RentACarService.LogoUrl;
                //}

                var obj = new
                {
                    car.CarId,
                    car.ImageUrl,
                    car.Model,
                    car.PricePerDay,
                    car.SeatsNumber,
                    car.Type,
                    car.Year,
                    car.Brand,
                    Name = car.Branch == null ? car.RentACarService.Name : car.Branch.RentACarService.Name,
                    Logo = car.Branch == null ? car.RentACarService.LogoUrl : car.Branch.RentACarService.LogoUrl,
                };

                return Ok(obj);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("racs-specialoffers/{id}")]
        public async Task<IActionResult> RacsSpecialOffers(int id) 
        {
            try
            {
                var specOffers = await _rentCarServiceRepository.GetSpecialOffersOfRacs(id);

                List<object> objs = new List<object>();

                foreach (var item in specOffers)
                {
                    objs.Add(new
                    {
                        Name = item.Car.Branch == null ? item.Car.RentACarService.Name : item.Car.Branch.RentACarService.Name,
                        Logo = item.Car.Branch == null ? item.Car.RentACarService.LogoUrl : item.Car.Branch.RentACarService.LogoUrl,
                        item.NewPrice,
                        item.OldPrice,
                        FromDate = item.FromDate.Date,
                        ToDate = item.ToDate.Date,
                        item.Car.Brand,
                        item.Car.CarId,
                        item.Car.ImageUrl,
                        item.Car.Model,
                        item.Car.SeatsNumber,
                        item.Car.Type,
                        item.Car.Year
                    });
                }

                return Ok(objs);

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        private bool FilterPass(Car item, List<int> ids, float priceFrom, float priceTo, string fromCity,
            string toCity, string carType, DateTime from, DateTime to, int seatFrom, int seatTo) 
        {
            int numOfDays = Math.Abs(from.Day - to.Day);
            if (ids.Count > 0)
            {
                if (item.RentACarService != null)
                {
                    if (!ids.Contains(item.RentACarService.RentACarServiceId))
                    {
                        return false;
                    }
                }
                else 
                {
                    if (!ids.Contains(item.Branch.RentACarServiceId))
                    {
                        return false;
                    }
                }
               
            }
            if (item.PricePerDay * numOfDays < priceFrom || item.PricePerDay * numOfDays > priceTo)
            {
                return false;
            }

            //rezervacije proveriti da li je slobodan
            if (!item.Type.Equals(carType) && carType != "")
            {
                return false;
            }
            RentACarService racs = null;
            if (item.RentACarService != null)
            {
                racs = item.RentACarService;
            }
            else
            {
                racs = item.Branch.RentACarService;
            }



            foreach (var offer in item.SpecialOffers)
            {
                if (offer.FromDate.Date >= from && offer.FromDate <= to || offer.ToDate.Date >= from && offer.ToDate <= to)
                {
                    return false;
                }
            }

            if (item.SeatsNumber < seatFrom || item.SeatsNumber > seatTo)
            {
                return false;
            }
            bool fromFound = false;
            bool toFound = false;

            if (racs.Address.City == fromCity)
            {
                fromFound = true;
            }
            else if (racs.Address.City == toCity)
            {
                toFound = true;
            }

            foreach (var branch in racs.Branches)
            {
                if (branch.City == fromCity && !fromFound)
                {
                    fromFound = true;
                }
                else if(branch.City == toCity && !toFound)
                {
                    toFound = true;
                }
            }

            if (!toFound || !fromFound)
            {
                return false;
            }


            return true;
        }
    }
}
