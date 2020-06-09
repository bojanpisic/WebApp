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
        Task<IdentityResult> Logout();
        Task<IdentityResult> ChangeEmail(int id, ChangeEmailDto profile);
        Task<IdentityResult> ChangePassword(int id, ChangePasswordDto profile);
        Task<IdentityResult> ChangeUserName(int id, ChangeUserNameDto profile);
        Task<IdentityResult> ChangeLastName(int id, ChangeLastNameDto profile);
        Task<IdentityResult> ChangeFirstName(int id, ChangeFirstNameDto profile);
        Task<IdentityResult> ChangePhone(int id, ChangePhoneDto profile);
        Task<IdentityResult> ChangeCity(int id, ChangeCityDto profile);
        Task<IdentityResult> ChangeImgUrl(int id, ChangeImgUrlDto profile);
    }
}
