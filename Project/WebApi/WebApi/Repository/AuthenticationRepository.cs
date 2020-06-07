using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web;

namespace WebApi.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public async Task<Person> GetPerson(string email, string password, UserManager<Person> userManager)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return await userManager.FindByNameAsync(email);
            }

            return user;
        }

        public async Task<bool> CheckPassword(Person user, string password, UserManager<Person> userManager)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<bool> IsEmailConfirmed(Person user, UserManager<Person> userManager)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }

        private const string GoogleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";

        public bool VerifyToken(string providerToken)
        {
            var httpClient = new HttpClient();
            var requestUri = new Uri(string.Format(GoogleApiTokenInfoUrl, providerToken));

            HttpResponseMessage httpResponseMessage;

            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
            }
            catch (Exception ex)
            {
                return false;
            }

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var response = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var googleApiTokenInfo = JsonConvert.DeserializeObject<GoogleApiTokenInfo>(response);

            return true;
        }

        public async Task<IList<string>> GetRoles(Person user, UserManager<Person> userManager)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> RegisterAirlineAdmin(AirlineAdmin admin, string password, UserManager<Person> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var result = await userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync("AirlineAdmin"))
                {
                    var res = await roleManager.CreateAsync(new IdentityRole("AirlineAdmin"));
                }

                await userManager.AddToRoleAsync(admin, "AirlineAdmin");

                var sended = await this.SendConfirmationMail(admin, userManager, "admin", password);

                if (!sended)
                {
                    return IdentityResult.Failed();
                }

                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }
        }

        public async Task<IdentityResult> RegisterSystemAdmin(Person admin, string password, UserManager<Person> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            var result = await userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var res = await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                await userManager.AddToRoleAsync(admin, "Admin");

                //var sended = await this.SendConfirmationMail(admin, userManager, "admin");

                //if (!sended)
                //{
                //    return IdentityResult.Failed();
                //}

                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }

        }

        public async Task<IdentityResult> RegisterUser(User user, string password, UserManager<Person> userManager,
            RoleManager<IdentityRole> roleManager)
        {

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync("RegularUser"))
                {
                    var res = await roleManager.CreateAsync(new IdentityRole("RegularUser"));


                }

                await userManager.AddToRoleAsync(user, "RegularUser");

                var sent = await this.SendConfirmationMail(user, userManager, "user");

                if (!sent)
                {
                    return IdentityResult.Failed();
                }

                return IdentityResult.Success;
            }
            else
            {
                return IdentityResult.Failed();
            }

        }

        private async Task<bool> SendConfirmationMail(Person user, UserManager<Person> userManager, string usertype, string password = "")
        {
            var fromMail = new MailAddress("bojanpisic@gmail.com");
            var frontEmailPassowrd = "bojan.pisic.123";

            var toMail = new MailAddress(user.Email);
            string subject;
            string body;

            if (usertype == "user")
            {

                string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                string confirmationTokenHtmlVersion = HttpUtility.UrlEncode(confirmationToken);

                var varifyUrl = "https://localhost:5001/api/authentication/ConfirmEmail?userId=" + user.Id + "&token=" +
                    confirmationTokenHtmlVersion;

                subject = "Your account is successfull created. Please confirm your email.";
                body = "<br/><br/>We are excited to tell you that your account is" +
                                " successfully created. Please click on the below link to verify your account" +
                                " <br/><br/><a href='" + varifyUrl + "'> Click here</a> ";
            }
            else
            {
                var loginurl = "https://localhost:5001/api/authentication/Login";

                subject = "Your account is successfull created.";
                body = "<br/>Your username is: " + user.UserName + "<br/>Password for your account is" + password + "<br/>" +
                    "Please change your password when you log in. <br/>" +
                    "Login to SkyRoads by clicking on this link: <a href='" + loginurl + "'> Login</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, frontEmailPassowrd)
            };

            using (var message = new MailMessage(fromMail, toMail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

            return true;
        }
    }
}
