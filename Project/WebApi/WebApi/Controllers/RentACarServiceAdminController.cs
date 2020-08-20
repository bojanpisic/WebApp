using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Xml;
using System.Security.Principal;
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

                var sum = 0.0;
                foreach (var r in racs.Rates)
                {
                    sum += r.Rate;
                }

                float rate = sum == 0 ? 0 : (float)sum / racs.Rates.ToArray().Length;

                object obj = new
                {
                    racs.Name,
                    racs.About,
                    racs.Address,
                    racs.LogoUrl,
                    rate = rate
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
        [Route("get-car-report")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetReport()  //  dobijaju izveštaje o slobodnim i zauzetim vozilima za određeni period
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

                var racss = await unitOfWork.RentACarRepository.Get(racs => racs.AdminId == userId);
                var racs = racss.FirstOrDefault();

                if (racs == null)
                {
                    return NotFound("RACS not found");
                }

                var queryString = Request.Query;
                var from = queryString["from"].ToString();
                var to = queryString["to"].ToString();
                var isFree = queryString["isFree"].ToString().ToUpper();

                var fromDate = DateTime.Now;
                var toDate = DateTime.Now;

                if (!DateTime.TryParse(from, out fromDate))
                {
                    return BadRequest("Incorrect date format");
                }

                if (!DateTime.TryParse(to, out toDate))
                {
                    return BadRequest("Incorrect date format");
                }

                var allCars =
                    await unitOfWork.CarRepository
                    .Get(car => car.Branch.RentACarService.RentACarServiceId == racs.RentACarServiceId
                    || car.RentACarService.RentACarServiceId == racs.RentACarServiceId,
                    null, "Rents,Rates");

                List<object> objs = new List<object>();

                foreach (var item in allCars)
                {
                    //PROVERA DA LI POSTOJI REZERVACIJA U TOM PERIODU
                    var rent = item.Rents.FirstOrDefault(rent => 
                    rent.TakeOverDate >= fromDate && rent.ReturnDate <= toDate ||
                    rent.TakeOverDate <= fromDate && rent.ReturnDate >= toDate ||
                    rent.TakeOverDate <= fromDate && rent.ReturnDate <= toDate && rent.ReturnDate >= fromDate ||
                    rent.TakeOverDate >= fromDate && rent.TakeOverDate <= toDate && rent.ReturnDate >= toDate);

                    if (isFree.Equals("TRUE") && rent != null || isFree.Equals("FALSE") && rent == null)
                    {
                        continue;
                    }

                    var sum = 0.0;
                    foreach (var r in item.Rates)
                    {
                        sum += r.Rate;
                    }

                    float rate = sum == 0 ? 0 : (float)sum / item.Rates.ToArray().Length;

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
                        racs.Address.State,
                        isFree = rent == null ? true: false,
                        rate = rate
                    });
                }

                return Ok(objs);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to return reports");
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

                    var sum = 0.0;
                    foreach (var r in item.Rates)
                    {
                        sum += r.Rate;
                    }

                    float rate = sum == 0 ? 0 : (float)sum / item.Rates.ToArray().Length;

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
                        racs.Address.State,
                        rate = rate
                    });
                }

                foreach (var branch in racs.Branches)
                {
                    foreach (var car in branch.Cars)
                    {
                        var sum = 0.0;
                        foreach (var r in car.Rates)
                        {
                            sum += r.Rate;
                        }

                        float rate = sum == 0 ? 0 : (float)sum / car.Rates.ToArray().Length;

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
                            branch.State,
                            rate = rate
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

                //var res = await unitOfWork.Branchrepository.Get(b => b.BranchId == id, null, "Cars,RentACarService");
                var res = await unitOfWork.CarRepository.Get(car => car.Branch.BranchId == id, null, "Rates");

                var branch = res.FirstOrDefault();

                if (branch == null)
                {
                    return BadRequest("Branch not found");
                }

                //var allCars = branch.Cars;

                List<object> objs = new List<object>();

                foreach (var item in res) //bilo allcars
                {
                    var sum = 0.0;
                    foreach (var r in item.Rates)
                    {
                        sum += r.Rate;
                    }

                    float rate = sum == 0 ? 0 : (float)sum / item.Rates.ToArray().Length;

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
                        item.Brand, 
                        rate = rate
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

                var cars = await unitOfWork.CarRepository.Get(car => car.CarId == id, null, "Rents");
                var car = cars.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                if (car.Rents.FirstOrDefault(rent => rent.TakeOverDate <= DateTime.Now && rent.TakeOverDate >= DateTime.Now) != null)
                {
                    return BadRequest("Cant modifie this car");
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

                var cars = await unitOfWork.CarRepository.Get(car => car.CarId == id, null, "Rents");
                var car = cars.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                if (car.Rents.FirstOrDefault(rent => rent.TakeOverDate <= DateTime.Now && rent.TakeOverDate >= DateTime.Now) != null)
                {
                    return BadRequest("Cant modifie this car");
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

                var res = await unitOfWork.CarRepository.Get(c => c.CarId == id, null, "SpecialOffers,Rates");
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

                var sum = 0.0;
                foreach (var r in car.Rates)
                {
                    sum += r.Rate;
                }

                float rate = sum == 0 ? 0 : (float)sum / car.Rates.ToArray().Length;

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
                    SpecialOfferDatesOfCar = specOffDates,
                    rate = rate
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

                var cars = await unitOfWork.CarRepository.Get(car => car.CarId == id, null, "Rents");
                var car = cars.FirstOrDefault();

                if (car == null)
                {
                    return NotFound("Car not found");
                }

                if (car.Rents.FirstOrDefault(rent => rent.TakeOverDate <= DateTime.Now && rent.TakeOverDate >= DateTime.Now) != null)
                {
                    return BadRequest("Cant delete this car");
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

                if (specialFromDate > specialToDate)
                {
                    return BadRequest("From date should be lower then to date");
                }


                foreach (var item in oldSpecOffers)
                {
                    if (item.FromDate >= specialFromDate && item.ToDate <= specialFromDate
                        || item.FromDate >= specialToDate && item.ToDate <= specialToDate
                        || item.FromDate <= specialFromDate && item.ToDate >= specialToDate)
                    {
                        return BadRequest("Dates are unavailable. Car has another special offers in that time.");
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

        #region Chart methods
        [HttpGet]
        [Route("get-day-stats")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDayStats() 
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

                var queryString = Request.Query;
                var date = queryString["date"].ToString();

                var day = DateTime.Now;

                if (!DateTime.TryParse(date, out day))
                {
                    return BadRequest("Date format is incorrect");
                }

                var cars = await unitOfWork.CarRepository.Get(car =>
                    car.RentACarService == null ?
                    car.Branch.RentACarService.AdminId == userId : car.RentACarService.AdminId == userId,
                    null, "Rents");

                int rentNum = 0;

                foreach (var item in cars)
                {
                    if (item.Rents.FirstOrDefault(rent => rent.TakeOverDate == day) != null)
                    {
                        rentNum++;
                    }
                }

                return Ok(new Tuple<DateTime, int>(day, rentNum));
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to get day state");
            }
        }

        [HttpGet]
        [Route("get-week-stats")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetWeekStats()
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

                var queryString = Request.Query;
                var week = queryString["week"].ToString().Split("W")[1];
                var year = queryString["week"].ToString().Split("W")[0];

                int weekNum = 0;
                int yearNum = 0;

                if (!Int32.TryParse(week, out weekNum))
                {
                    return BadRequest();
                }
                if (!Int32.TryParse(year, out yearNum))
                {
                    return BadRequest();
                }

                List<DateTime> daysOfWeek = new List<DateTime>();

                var lastDay = new DateTime(yearNum, 1, 1).AddDays((weekNum) * 7);
                daysOfWeek.Add(lastDay);

                for (int i = 1; i < 7; i++)
                {
                    daysOfWeek.Add(lastDay.AddDays(-i));
                }

                var cars = await unitOfWork.CarRepository.Get(car =>
                    car.RentACarService == null ?
                    car.Branch.RentACarService.AdminId == userId : car.RentACarService.AdminId == userId,
                    null, "Rents");

                List<Tuple<DateTime, int>> stats = new List<Tuple<DateTime, int>>();

                foreach (var day in daysOfWeek)
                {
                    stats.Add(new Tuple<DateTime, int>(day, 0));
                }

                CarRent r;

                foreach (var item in cars)
                {
                    if (( r = item.Rents.FirstOrDefault(rent => daysOfWeek.Contains(rent.TakeOverDate))) != null)
                    {
                        var s = stats.Find(s => s.Item1 == r.TakeOverDate);
                        int index = stats.IndexOf(s);

                        stats[index] =  new Tuple<DateTime, int>(s.Item1, s.Item2 + 1);
                    }
                }

                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to get day state");
            }
        }

        [HttpGet]
        [Route("get-month-stats")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMonthStats()
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

                var queryString = Request.Query;
                var month = queryString["week"].ToString().Split("-")[1];
                var year = queryString["week"].ToString().Split("-")[0];

                int monthNum = 0;
                int yearNum = 0;

                if (!Int32.TryParse(month, out monthNum))
                {
                    return BadRequest();
                }
                if (!Int32.TryParse(year, out yearNum))
                {
                    return BadRequest();
                }

                int numOfDays = DateTime.DaysInMonth(yearNum, monthNum);
                DateTime firstDayOfMonth = new DateTime(yearNum, monthNum, 1);


                List<DateTime> daysOfMonth = new List<DateTime>();

                daysOfMonth.Add(firstDayOfMonth);

                for (int i = 1; i < numOfDays; i++)
                {
                    daysOfMonth.Add(firstDayOfMonth.AddDays(i));
                }

                var cars = await unitOfWork.CarRepository.Get(car =>
                    car.RentACarService == null ?
                    car.Branch.RentACarService.AdminId == userId : car.RentACarService.AdminId == userId,
                    null, "Rents");

                List<Tuple<DateTime, int>> stats = new List<Tuple<DateTime, int>>();

                foreach (var day in daysOfMonth)
                {
                    stats.Add(new Tuple<DateTime, int>(day, 0));
                }

                CarRent r;

                foreach (var item in cars)
                {
                    if ((r = item.Rents.FirstOrDefault(rent => daysOfMonth.Contains(rent.TakeOverDate))) != null)
                    {
                        var s = stats.Find(s => s.Item1 == r.TakeOverDate);
                        int index = stats.IndexOf(s);

                        stats[index] = new Tuple<DateTime, int>(s.Item1, s.Item2 + 1);
                    }
                }

                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to get day state");
            }
        }
        #endregion
    }
}
