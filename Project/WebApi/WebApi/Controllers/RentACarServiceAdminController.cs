using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentACarServiceAdminController : Controller
    {
        private IUnitOfWork unitOfWork;

        public RentACarServiceAdminController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
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
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId, null, "Address");
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
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

                return StatusCode(500, "Failed to return RACS address");
            }
        }

        [HttpGet]
        [Route("get-racs")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> GetRACSProfile()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId, null, "Address");
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
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

                return StatusCode(500, "Failed to return profile");
            }

        }

        #region Change info methods
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId, null, "Address");
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return BadRequest("Racs doesnt exist");
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

                if (addressChanged)
                {
                    //using (var transaction = new TransactionScope())
                    //{
                        try
                        {
                            unitOfWork.RentACarRepository.Update(racs);
                            //unitOfWork.Commit();
                            await unitOfWork.RentACarRepository.UpdateAddress(racs.Address);
                            await unitOfWork.Commit();

                            //transaction.Complete();
                            //await transaction.Result.CommitAsync();
                        }
                        catch (Exception)
                        {
                            return StatusCode(500, "Failed to apply changes");
                            //unitOfWork.Rollback();
                            //transaction.Dispose();
                            //await transaction.Result.RollbackAsync();
                        }
                    //}
                }
                else
                {
                    try
                    {
                        unitOfWork.RentACarRepository.Update(racs);
                        await unitOfWork.Commit();
                    }
                    catch (Exception)
                    {
                        return StatusCode(500, "Failed to apply changes");
                    }
                }
                //racs.Address = dto.Address;
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to apply changes");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return BadRequest("Racs doesnt exist");
                }

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    racs.LogoUrl = stream.ToArray();
                }
                try
                {
                    unitOfWork.RentACarRepository.Update(racs);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to apply changes");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to apply changes");
            }
        }
        #endregion

        #region Branch methods
        [HttpGet]
        [Route("get-racs-branches")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRACSBranches()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId, null, "Branches");
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return BadRequest("Rent Service not found");
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
                return StatusCode(500, "Failed to return branches");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("Racs not found");
                }

                var branch = new Branch()
                {
                    RentACarService = racs,
                    City = branchDto.City,
                    State = branchDto.State
                };

                try
                {
                    await unitOfWork.BranchRepository.Insert(branch);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to insert branch");
                }

                var allBranches = await unitOfWork.BranchRepository.Get(b => b.RentACarServiceId == racs.RentACarServiceId);

                List<object> objs = new List<object>();

                foreach (var item in allBranches)
                {
                    objs.Add(new { item.City, item.BranchId, item.State });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to insert branch");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var findBranch = await unitOfWork.BranchRepository.Get(b => b.BranchId == id);
                var branch = findBranch.FirstOrDefault();

                if (branch == null)
                {
                    return NotFound("Branch not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
                }

                foreach (var car in branch.Cars)
                {
                    racs.Cars.Add(car);
                }

                //using (var transaction = new TransactionScope())
                //{
                    try
                    {
                        unitOfWork.RentACarRepository.Update(racs);
                        //unitOfWork.Commit();

                        unitOfWork.BranchRepository.Delete(branch);
                        await unitOfWork.Commit();

                        //transaction.Complete();
                        //await transaction.Result.CommitAsync();
                    }
                    catch (Exception)
                    {
                        //unitOfWork.Rollback();
                        //transaction.Dispose();
                        //await transaction.Result.RollbackAsync();
                        return StatusCode(500, "Failed to delete branch. One of transaction failed");

                    }
                //}

                List<object> objs = new List<object>();

                foreach (var item in racs.Branches)
                {
                    objs.Add(new { item.City, item.BranchId, item.State });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to delete branch");
            }

        }
        #endregion

        #region Car methods
        [HttpGet]
        [Route("get-racs-cars")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetRACSCars()
        {
            try
            {
                string userId = User.Claims.First(c => c.Type == "UserID").Value;
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var racs = await unitOfWork.RentACarRepository.GetRACSAndCars(userId); //kupi i od filijala auta

                if (racs == null)
                {
                    return NotFound("RACS not found");
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
                return StatusCode(500, "Failed to return cars");
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
                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                string userRole = User.Claims.First(c => c.Type == "Roles").Value;

                if (!userRole.Equals("RentACarServiceAdmin"))
                {
                    return Unauthorized();
                }

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.BranchRepository.Get(b => b.BranchId == id, null, "Cars,RentACarService");
                var branch = res.FirstOrDefault();

                if (branch == null)
                {
                    return BadRequest("Branch not found");
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
                return StatusCode(500, "Failed to return branch cars");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found" );
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("Racs not found");
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


                try
                {
                    await unitOfWork.CarRepository.Insert(car);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to add car");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to add car");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var branch = await unitOfWork.BranchRepository.GetByID(carDto.BranchId);

                if (branch == null)
                {
                    return NotFound("Racs not found");
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

                try
                {
                    await unitOfWork.CarRepository.Insert(car);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to add car to branch");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to add car to branch");
            }
        }

        [HttpPut]
        [Route("change-car-info/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<IActionResult> ChangeCarInfo(int id, ChangeCarDto dto)
        {
            if (String.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest("id empty");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var car = await unitOfWork.CarRepository.GetByID(id);

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                car.Brand = dto.Brand;
                car.Model = dto.Model;
                car.PricePerDay = dto.PricePerDay;
                car.SeatsNumber = dto.SeatsNumber;
                car.Type = dto.Type;
                car.Year = dto.Year;

                try
                {
                    unitOfWork.CarRepository.Update(car);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to apply changes");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to apply changes");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var car = await unitOfWork.CarRepository.GetByID(id);

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                using (var stream = new MemoryStream())
                {
                    await img.CopyToAsync(stream);
                    car.ImageUrl = stream.ToArray();
                }

                try
                {
                    unitOfWork.CarRepository.Update(car);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to apply changes");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to apply changes");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.CarRepository.Get(c => c.CarId == id, null, "SpecialOffers");
                var car = res.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car doesnt exist");
                }

                var ress = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = ress.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
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
                return StatusCode(500, "Failed to return car");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var car = await unitOfWork.CarRepository.GetByID(id);

                if (car == null)
                {
                    return NotFound("Car doesnt exist");
                }
                try
                {
                    unitOfWork.CarRepository.Delete(car);
                    await unitOfWork.Commit();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Failed to delete car");
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to delete car");
            }
        }

        #endregion

        #region Special offer methods
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var ress = await unitOfWork.CarRepository.Get(c => c.CarId == id, null , "SpecialOffers");
                var car = ress.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                //var oldSpecOffers = await unitOfWork.RentACarRepository.GetSpecialOffersOfCar(car);
                var oldSpecOffers = car.SpecialOffers;

                var specialFromDate = Convert.ToDateTime(specialOfferDto.FromDate);
                var specialToDate = Convert.ToDateTime(specialOfferDto.ToDate);


                foreach (var item in oldSpecOffers)
                {
                    if (item.FromDate >= specialFromDate && item.ToDate <= specialFromDate
                        || item.FromDate >= specialToDate && item.ToDate <= specialToDate
                        || item.FromDate <= specialFromDate && item.ToDate >= specialToDate)
                    {
                        return BadRequest(new IdentityError() 
                        { Description = "Dates are unavailable. Car has another special offers in that time." });
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

                //using (var transaction = new TransactionScope())
                //{
                    try
                    {
                        await unitOfWork.RACSSpecialOfferRepository.Insert(specialOffer);
                        //unitOfWork.Commit();

                        unitOfWork.CarRepository.Update(car);
                        await unitOfWork.Commit();

                        //await transaction.Result.CommitAsync();
                        //transaction.Complete();
                    }
                    catch (Exception)
                    {
                        //unitOfWork.Rollback();
                        //transaction.Dispose();
                        //await transaction.Result.RollbackAsync();
                        return StatusCode(500, "Failed to add special offer");
                    }
                //}
                return Ok();
            }
            catch (Exception) 
            { 
                return StatusCode(500, "Failed to add special offer");
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

                var user = await unitOfWork.UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var res = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = res.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
                }

                var specOffers = await unitOfWork.RACSSpecialOfferRepository.GetSpecialOffersOfRacs(racs.RentACarServiceId);

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
                return StatusCode(500, "Failed to return special offers");
            }
        }

        #endregion

    }
}
