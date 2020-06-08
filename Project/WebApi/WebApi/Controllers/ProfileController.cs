using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public IAuthenticationRepository _authRepository { get; }
        public IProfileRepository _profileRepository { get; }

        public DataContext _context { get; }
        public ProfileController(UserManager<Person> userManager, DataContext context, IProfileRepository profileRepo)
        {
            _userManager = userManager;
            _context = context;
            _profileRepository = profileRepo;


        }

        //[HttpPut]
        //[Route("ChangeProfile")]

        //public async Task<IActionResult> ChangeProfile([FromBody]ChangeProfileDto profile) 
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        var result = await _profileRepository.ChangeProfile(profile, _userManager);

        //        if (result.Succeeded)
        //        {
        //            return Ok();
        //        }

        //        return BadRequest();
        //    }
        //    catch (Exception)
        //    {

        //        return StatusCode(500, "Internal server error.");

        //    }
        //}

    }
}
