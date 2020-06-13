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
using System.Net.Http;
using WebApi.Data;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        private readonly DataContext _context;

        public AuthenticationController(UserManager<Person> userManager,
            SignInManager<Person> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config, DataContext context)
        {
            this._context = context;
            this._authenticationRepository = new AuthenticationRepository(context, userManager, roleManager);
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this.configuration = config;
        }


        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User createUser;

            try
            {
                if (!_authenticationRepository.CheckPasswordMatch(userDto.Password, userDto.ConfirmPassword))
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Passwords dont match" });
                }

                createUser = new User()
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
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }

            using (var transaction = _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await this._authenticationRepository.RegisterUser(createUser, userDto.Password);

                    //if (!result.Succeeded)
                    //{
                    //    return BadRequest(result.Errors);
                    //    //return BadRequest(result);
                    //}

                    var addToRoleResult = await _authenticationRepository.AddToRole(createUser, "RegularUser");

                    //if (!addToRoleResult.Succeeded)
                    //{
                    //    return BadRequest(addToRoleResult);
                    //}
                   await transaction.Result.CommitAsync();

                }
                catch (Exception)
                {
                    await transaction.Result.RollbackAsync();
                    return StatusCode(500, "Internal server error.");
                }
            }

            var emailSent = await _authenticationRepository.SendConfirmationMail(createUser, "user");
            return Ok();
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

                if (!_authenticationRepository.CheckPasswordMatch(userDto.Password, userDto.ConfirmPassword))
                {
                    return BadRequest(new IdentityError() { Code = "400", Description = "Passwords dont match" });
                }
                createUser = new Person()
                {
                    Email = userDto.Email,
                    UserName = userDto.UserName,
                };
            }
            catch (Exception)
            {
                throw;
            }

            using (var transaction = _context.Database.BeginTransactionAsync())
                try
                {
                    var result = await this._authenticationRepository.RegisterSystemAdmin(createUser, userDto.Password);

                    //if (!result.Succeeded)
                    //{
                    //    return BadRequest(result);
                    //}

                    var addToRoleResult = await _authenticationRepository.AddToRole(createUser, "Admin");

                    //if (!addToRoleResult.Succeeded)
                    //{
                    //    return BadRequest(addToRoleResult);
                    //}
                    await transaction.Result.CommitAsync();

                }
                catch (Exception)
                {
                    await transaction.Result.RollbackAsync();
                    return StatusCode(500, "Internal server error.");
                }

            var emailSent = await _authenticationRepository.SendConfirmationMail(createUser, "admin", userDto.Password);
            return Ok();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {


                var user = await this._authenticationRepository.GetPerson(loginUser.UserNameOrEmail
                    /*String.IsNullOrEmpty(loginUser.Email) ? loginUser.Username : loginUser.Email*/,
                    loginUser.Password);

                if (user == null)
                {
                    var res = new IdentityError();
                    res.Code = "404";
                    res.Description = "Username or email doesnt exist";
                    return NotFound(res);
                }

                if (loginUser.Token != null && loginUser.UserId != null)
                {
                    await _authenticationRepository.ConfirmEmail(user, loginUser.Token);
                }

                var isPasswordCorrect = await this._authenticationRepository.CheckPassword(user, loginUser.Password);

                if (!isPasswordCorrect)
                {
                    var res = new IdentityError();
                    res.Code = "400";
                    res.Description = "Username or email or password is incorrect";
                    return BadRequest(res);
                    //return Unauthorized();
                }

                var role = (await _authenticationRepository.GetRoles(user)).FirstOrDefault();

                if (role == "RegularUser")
                {
                    var isEmailConfirmed = await this._authenticationRepository.IsEmailConfirmed(user);

                    if (!isEmailConfirmed)
                    {
                        return BadRequest(new IdentityError() { Description = "Email not confirmed"});
                    }
                }


                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim("Roles", role),
                        new Claim("PasswordChanged", user.PasswordChanged.ToString())
                        //new Claim("EmailConfirmed", user.EmailConfirmed.ToString())
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
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await _authenticationRepository.GetUserById(userId);
                if (user == null)
                {
                    return BadRequest(new IdentityError() { Code = "User dont exist"});
                }

                var result = await _authenticationRepository.ConfirmEmail(user, token);

                if (result.Succeeded)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Route("external-login")]
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
        [Route("social-login")]
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
