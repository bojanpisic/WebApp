using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public class ProfileRepository : IProfileRepository
    {
        public async Task<IdentityResult> ChangeCity(ChangeCityDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!profile.City.Equals(user.City))
            {
                user.City = profile.City;
                return await userManager.UpdateAsync(user);
            }

            return IdentityResult.Success;

        }

        public async Task<IdentityResult> ChangeEmail(ChangeEmailDto profile, UserManager<Person> userManager)
        { 
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!profile.Email.Equals(user.Email))
            {
                var emailChangeToken = await userManager.GenerateChangeEmailTokenAsync(user, profile.Email);
                var result = await userManager.ChangeEmailAsync(user, profile.Email, emailChangeToken);

                if (!result.Succeeded)
                {
                    return IdentityResult.Failed();
                }
            }

            return IdentityResult.Success;

        }

        public async Task<IdentityResult> ChangeFirstName(ChangeFirstNameDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!profile.FirstName.Equals(user.FirstName))
            {
                user.FirstName = profile.FirstName;
                return await userManager.UpdateAsync(user);

            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ChangeImgUrl(ChangeImgUrlDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!profile.ImgUrl.Equals(user.ImageUrl))
            {
                user.ImageUrl = profile.ImgUrl;
                return await userManager.UpdateAsync(user);

            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ChangeLastName(ChangeLastNameDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!profile.LastName.Equals(user.LastName))
            {
                user.LastName = profile.LastName;
                return await userManager.UpdateAsync(user);

            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

            if (!String.IsNullOrEmpty(profile.Password))
            {
                if (!PasswordMatch(profile.Password, profile.PasswordConfirm))
                {
                    return IdentityResult.Failed();
                }

                var result = await userManager.ChangePasswordAsync(user, user.PasswordHash, profile.Password);
                if (!result.Succeeded)
                {
                    return IdentityResult.Failed();
                }
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ChangePhone(ChangePhoneDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }
            if (!profile.Phone.Equals(user.PhoneNumber))
            {
                var phoneToken = await userManager.GenerateChangePhoneNumberTokenAsync(user, profile.Phone);
                var result = await userManager.ChangePhoneNumberAsync(user, profile.Phone, phoneToken);

                if (!result.Succeeded)
                {
                    return IdentityResult.Failed();
                }
            }
            return IdentityResult.Success;

        }

        public async Task<IdentityResult> ChangeUserName(ChangeUserNameDto profile, UserManager<Person> userManager)
        {
            var user = await userManager.FindByIdAsync(profile.UserId.ToString());

            if (user == null)
            {
                return IdentityResult.Failed();
            }

           
            if (!profile.UserName.Equals(user.UserName))
            {
                var result = await userManager.SetUserNameAsync(user, profile.UserName);
                if (!result.Succeeded)
                {
                    return IdentityResult.Failed();
                }
            }

            return IdentityResult.Success;
        }

        private bool PasswordMatch(string newPassword, string passwConfirm)
        {
            return newPassword.Equals(passwConfirm);
        }

    }
}
