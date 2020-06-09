using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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


        private readonly DataContext _context;
        public ProfileController(UserManager<Person> userManager, DataContext context, SignInManager<Person> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _profileRepository = new ProfileRepository(context, userManager, signInManager);


        }

        [HttpPost]
        [AllowAnonymous]
        [Route("logout")]
        public async Task<IActionResult> Logout() 
        {
            await _profileRepository.Logout();

            return Ok();
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
