using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IProfileRepository
    {
        Task<IdentityResult> ChangeEmail(ChangeEmailDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangePassword(ChangePasswordDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangeUserName(ChangeUserNameDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangeLastName(ChangeLastNameDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangeFirstName(ChangeFirstNameDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangePhone(ChangePhoneDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangeCity(ChangeCityDto profile, UserManager<Person> userManager);
        Task<IdentityResult> ChangeImgUrl(ChangeImgUrlDto profile, UserManager<Person> userManager);
    }
}
