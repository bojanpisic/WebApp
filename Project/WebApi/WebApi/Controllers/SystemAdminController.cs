using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public SystemAdminController(ISystemAdminRepository repository, DataContext context)
        {
            _repository = repository;
            _context = context;
        }

        public ISystemAdminRepository _repository { get; }
        public DataContext _context { get; }

        public async Task<IActionResult> CreateAirlineForAdmin([FromBody]AirlineDto airlineDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var airline = new Airline()
                {
                    Name = airlineDto.Name,
                    LogoUrl = airlineDto.LogoUrl,
                    PromoDescription = airlineDto.PromoDescription,
                    Address = airlineDto.Address
                };

                var result = await _repository.CreateAirlineForAdmin(airlineDto.adminId, airline, _context);

                if (result.Succeeded)
                {
                    return StatusCode(201, "Created");
                }
                else 
                {
                    return BadRequest("Something went wrong");

                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
