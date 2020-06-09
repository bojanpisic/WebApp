using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IAuthenticationRepository
    {
        Task<IdentityResult> RegisterUser(User user, string password);
        Task<IdentityResult> RegisterAirlineAdmin(AirlineAdmin admin, string password);
        Task<IdentityResult> RegisterSystemAdmin(Person admin, string password);
        Task<Person> GetPerson(string email, string password);
        //Task<Person> GetPersonById(int id, UserManager<Person> userManager);
        Task<bool> CheckPassword(Person user, string password);
        Task<bool> IsEmailConfirmed(Person user);
        Task<IList<string>> GetRoles(Person user);
        Task<IdentityResult> AddToRole(Person user, string roleName);
        bool VerifyToken(string providerToken);
        bool CheckPasswordMatch(string password, string confirmPassword);
        Task<IdentityResult> SendConfirmationMail(Person user, string usertype, string password = "");
        Task<IdentityResult> ConfirmEmail(Person user, string token);
        Task<Person> GetUserById(string id);
    }
}
