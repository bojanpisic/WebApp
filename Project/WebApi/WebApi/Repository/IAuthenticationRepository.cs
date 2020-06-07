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
        Task<IdentityResult> RegisterUser(User user, string password, UserManager<Person> userManager,
            RoleManager<IdentityRole> roleManager);
        Task<IdentityResult> RegisterAirlineAdmin(AirlineAdmin admin, string password, UserManager<Person> userManager,
            RoleManager<IdentityRole> roleManager);
        Task<IdentityResult> RegisterSystemAdmin(Person admin, string password, UserManager<Person> userManager,
    RoleManager<IdentityRole> roleManager);
        Task<Person> GetPerson(string email, string password, UserManager<Person> userManager);
        //Task<Person> GetPersonById(int id, UserManager<Person> userManager);
        Task<bool> CheckPassword(Person user, string password, UserManager<Person> userManager);

        Task<bool> IsEmailConfirmed(Person user, UserManager<Person> userManager);

        Task<IList<string>> GetRoles(Person user, UserManager<Person> userManager);

        bool VerifyToken(string providerToken);
    }
}
