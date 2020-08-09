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
using System.Transactions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly SignInManager<Person> _signInManager;
        private IUnitOfWork unitOfWork;

        //public AuthenticationController(UserManager<Person> userManager,
        //    SignInManager<Person> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration config, DataContext context)
        //{
        //    this._context = context;
        //    this._authenticationRepository = new AuthenticationRepository(userManager, roleManager);
        //    this._roleManager = roleManager;
        //    this._signInManager = signInManager;
        //    this._userManager = userManager;
        //    this.configuration = config;
        //}
        public AuthenticationController(SignInManager<Person> signInManager, IConfiguration config, IUnitOfWork _unitOfWork) 
        {
            this._signInManager = signInManager;
            this.configuration = config;
            this.unitOfWork = _unitOfWork;
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
                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(userDto.Password, userDto.ConfirmPassword))
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
                return StatusCode(500, "Registration failed.");
            }

            //using (var transaction = new TransactionScope())
            //{
            try
            {
                await unitOfWork.AuthenticationRepository.RegisterUser(createUser, userDto.Password);
                await unitOfWork.AuthenticationRepository.AddToRole(createUser, "RegularUser");
                await unitOfWork.Commit();
                //transaction.Complete();
            }
            catch (Exception)
            {
                //unitOfWork.Rollback();
                return StatusCode(500, "Registration failed.");
            }
            //}
            try
            {
                var emailSent = await unitOfWork.AuthenticationRepository.SendConfirmationMail(createUser, "user");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sending email failed.");
            }
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

                if (!unitOfWork.AuthenticationRepository.CheckPasswordMatch(userDto.Password, userDto.ConfirmPassword))
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
                return StatusCode(500, "Internal server error.");
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

            return StatusCode(201, "Registered"); //201 - created
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
                var user = await this.unitOfWork.AuthenticationRepository.GetPerson(loginUser.UserNameOrEmail
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
                    await unitOfWork.AuthenticationRepository.ConfirmEmail(user, loginUser.Token);
                }

                var isPasswordCorrect = await this.unitOfWork.AuthenticationRepository.CheckPassword(user, loginUser.Password);

                if (!isPasswordCorrect)
                {
                    var res = new IdentityError();
                    res.Code = "400";
                    res.Description = "Username or email or password is incorrect";
                    return BadRequest(res);
                    //return Unauthorized();
                }

                var role = (await unitOfWork.AuthenticationRepository.GetRoles(user)).FirstOrDefault();

                if (role == "RegularUser")
                {
                    var isEmailConfirmed = await this.unitOfWork.AuthenticationRepository.IsEmailConfirmed(user);

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
                return StatusCode(500, "Login failed.");
            }
        }

        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await unitOfWork.AuthenticationRepository.GetUserById(userId);
                if (user == null)
                {
                    return BadRequest(new IdentityError() { Code = "User dont exist"});
                }

                var result = await unitOfWork.AuthenticationRepository.ConfirmEmail(user, token);

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
                return StatusCode(500, "Email confirmation failed.");
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
                    var user = await unitOfWork.UserManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new Person
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };
                        await unitOfWork.UserManager.CreateAsync(user);
                    }

                    await unitOfWork.UserManager.AddLoginAsync(user, info);
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
            if (unitOfWork.AuthenticationRepository.VerifyToken(loginModel.IdToken))
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
