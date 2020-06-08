using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.DTOs;
using WebApi.Models;
using WebApi.Repository;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IConfiguration configuration;
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(IAuthenticationRepository repository, UserManager<Person> userManager,
            SignInManager<Person> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            this._authenticationRepository = repository;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this.configuration = config;
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromBody]RegistrationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!_authenticationRepository.CheckPasswordMatch(userDto.Password, userDto.ConfirmPassword))
                {
                    return BadRequest("Passwords dont match");
                }
                var createUser = new User()
                {
                    Email = userDto.Email,
                    UserName = userDto.UserName,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    ImageUrl = userDto.ImageUrl,
                    PhoneNumber = userDto.Phone,
                    City = userDto.City,
                    EmailConfirmed = false,
                };

                var result = await this._authenticationRepository.RegisterUser(createUser, userDto.Password, this._userManager, this._roleManager);

                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest("");
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("RegisterSystemAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterSystemAdmin([FromBody] RegistrationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createUser = new Person()
                {
                    Email = userDto.Email,
                    UserName = userDto.UserName,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    ImageUrl = userDto.ImageUrl,
                    PhoneNumber = userDto.Phone,
                    City = userDto.City,
                };

                var result = await this._authenticationRepository.RegisterSystemAdmin(createUser, userDto.Password, this._userManager, this._roleManager);

                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest("Username already exist!");
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("RegisterAirlineAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAirlineAdmin([FromBody] RegistrationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createUser = new AirlineAdmin()
                {
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    ImageUrl = userDto.ImageUrl,
                    PhoneNumber = userDto.Phone,
                    City = userDto.City,
                    EmailConfirmed = false,
                };

                var result = await this._authenticationRepository.RegisterAirlineAdmin(createUser, userDto.Password, this._userManager, this._roleManager);

                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest("");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (String.IsNullOrEmpty(loginUser.Email) && String.IsNullOrEmpty(loginUser.Username))
            //{
            //    return BadRequest();
            //}

            try
            {
                var user = await this._authenticationRepository.GetPerson(loginUser.UserNameOrEmail/*String.IsNullOrEmpty(loginUser.Email) ? loginUser.Username : loginUser.Email*/,
                    loginUser.Password, this._userManager);

                if (user == null)
                {
                    return NotFound();
                }

                var isPasswordCorrect = await this._authenticationRepository.CheckPassword(user, loginUser.Password, this._userManager);

                if (!isPasswordCorrect)
                {
                    //return BadRequest("Password is incorrect.");
                    return Unauthorized();
                }

                var role = (await _authenticationRepository.GetRoles(user, _userManager)).First();

                if (role == "RegularUser")
                {
                    var isEmailConfirmed = await this._authenticationRepository.IsEmailConfirmed(user, _userManager);

                    if (!isEmailConfirmed)
                    {
                        return BadRequest("Email is not confirmed.");
                    }
                }
                else if (role == "AirlineAdmin" || role == "RentCarServiceAdmin") 
                {
                    
                }


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim("Role", role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                            SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Ok(new { token });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.
                            ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("externallogin")]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectionUrl = Url.Action("ExternalLoginCallback", "Authentication", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectionUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginDto logindto = new LoginDto
            {
                //ReturnUrl = returnUrl,
                //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                return BadRequest();
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return BadRequest();
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, 
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else 
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new Person
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await _userManager.CreateAsync(user);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("SocialLogin")]
        public async Task<IActionResult> SocialLogin([FromBody] LoginDto loginModel, string provider)
        {
            if (_authenticationRepository.VerifyToken(loginModel.IdToken))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    //Key min: 16 characters
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }

            return Ok();
        }
    }
}
