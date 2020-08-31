﻿using Microsoft.AspNetCore.Http;
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
using WebApi.Data;

namespace WebApi.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<Person> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthenticationRepository(UserManager<Person> _userManager,
            RoleManager<IdentityRole> _roleManager = null)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        #region User methods
        public async Task<Person> GetUserById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }
        public async Task<Person> GetPerson(string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return await userManager.FindByNameAsync(email);
            }

            return user;
        }

        public async Task<bool> CheckPassword(Person user, string password)
        {
            return await userManager.CheckPasswordAsync(user, password);
        }

        public bool CheckPasswordMatch(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }

        public async Task<bool> IsEmailConfirmed(Person user)
        {
            return await userManager.IsEmailConfirmedAsync(user);
        }
        public async Task<IdentityResult> ConfirmEmail(Person user, string token)
        {
            return await userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<Person> GetPersonByUserName(string username)
        {
            return await userManager.FindByNameAsync(username);
        }

        public async Task<Person> GetPersonByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        #endregion

        #region Role methods
        public async Task<IList<string>> GetRoles(Person user)
        {
            return await userManager.GetRolesAsync(user);
        }
        public async Task<IdentityResult> AddToRole(Person user, string roleName)
        {
            IdentityResult createRoleRes = IdentityResult.Success;

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                createRoleRes = await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            if (!createRoleRes.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError() { Code = "Cant create role" });
            }

            await userManager.AddToRoleAsync(user, roleName);

            return IdentityResult.Success;
        }

        #endregion

        #region Register
        public async Task<IdentityResult> RegisterAirlineAdmin(AirlineAdmin admin, string password)
        {
            return await userManager.CreateAsync(admin, password);
        }

        public async Task<IdentityResult> RegisterSystemAdmin(Person admin, string password)
        {
            return await userManager.CreateAsync(admin, password);
        }

        public async Task<IdentityResult> RegisterUser(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }
        public async Task<IdentityResult> RegisterRACSAdmin(Person admin, string password)
        {
            return await userManager.CreateAsync(admin, password);
        }

        #endregion

        #region Send mail methods
        public async Task<IdentityResult> SendConfirmationMail(Person user, string usertype, string password = "")
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

                var varifyUrl = "http://localhost:4200/signin/" + user.Id + "/" +
                    confirmationTokenHtmlVersion;

                subject = "Your account is successfull created. Please confirm your email.";
                body = "<br/><br/>We are excited to tell you that your account is" +
                                " successfully created. Please click on the below link to verify your account" +
                                " <br/><br/><a href='" + varifyUrl + "'> Click here</a> ";
            }
            else
            {
                var loginurl = "http://localhost:4200/signin";

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

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SendRentConfirmationMail(Person user, CarRent rent)
        {
            await Task.Yield();

            var fromMail = new MailAddress("bojanpisic@gmail.com");
            var frontEmailPassowrd = "bojan.pisic.123";

            var toMail = new MailAddress(user.Email);
            string subject;
            string body;

            subject = "Your rent is successfull created.";
            body = "<br/><br/>Your rent is created. Check your rents on your profile.<br/><br/>Reservation details: " +
                    "<br/><t/><t/>From: " + rent.TakeOverCity + 
                    "<br/><t/><t/>To: " + rent.ReturnCity + 
                    "<br/><t/><t/>Take over date: " + rent.TakeOverDate +
                    "<br/><t/><t/>Return date: " + rent.ReturnDate + 
                    "<br/><br/>Car details<br/>" + 
                    "<br/><t/><t/>Brand: " + rent.RentedCar.Brand +
                    "<br/><t/><t/>Model: " + rent.RentedCar.Model +
                    "<br/><t/><t/>Car type: " + rent.RentedCar.Type +
                    "<br/><t/><t/>Price per day: " + rent.RentedCar.PricePerDay +"$" +
                    "</br>Total price: " + rent.TotalPrice+ "$" +
                    "<br/><t/><t/>"
                    + "<br/><br/> SkyRoads";

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

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SendTicketConfirmationMail(Person user, FlightReservation reservation)
        {
            await Task.Yield();

            var fromMail = new MailAddress("bojanpisic@gmail.com");
            var frontEmailPassowrd = "bojan.pisic.123";

            var toMail = new MailAddress(user.Email);
            string subject;
            string body;

            subject = "Your trip is successfull created.";
            body = "<br/><br/>Your trip is created. Check your flights on your profile.<br/><br/>Reservation details:";
            foreach (var item in reservation.Tickets)
            {
                body += "<br/><t/>Ticket:" +
                 "<br/><t/><t/>Flight number: " + item.Seat.Flight.FlightNumber +
                 "<br/><t/><t/>From: " + item.Seat.Flight.From +
                 "<br/><t/><t/>To: " + item.Seat.Flight.To +
                 "<br/><t/><t/>Departure: " + item.Seat.Flight.TakeOffDateTime +
                 "<br/><t/><t/>Arrival: " + item.Seat.Flight.LandingDateTime +
                 "<br/><t/><t/>Travel time: " + item.Seat.Flight.TripTime +
                 "<br/><t/><t/>Travel length: " + item.Seat.Flight.tripLength + "km" +
                 "<br/><t/><t/>Seat details: "+
                 "<br/><t/><t/><t/>Seat number: " + item.Seat.Row + item.Seat.Column +
                 "<br/><t/><t/><t/>Class: " + item.Seat.Class +
                 "<br/><t/><t/>Ticket price: " + item.Price + "$";
            }
                   
                   body += "</br></br>Total price: " + reservation.Price +
                    "<br/><t/><t/>"
                    + "<br/><br/> SkyRoads";

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

            return IdentityResult.Success;
        }
        public async Task<IdentityResult> SendMailToFriend(Invitation invitation)
        {
            await Task.Yield();

            var fromMail = new MailAddress("bojanpisic@gmail.com");
            var frontEmailPassowrd = "bojan.pisic.123";

            var toMail = new MailAddress(invitation.Receiver.Email);
            string subject;
            string body;

            var url = "http://localhost:4200/signin/messages" ;

            subject = "You have trip invite!";
            body = "<br/><br/>Your friend " + invitation.Sender.FirstName + " " + invitation.Sender.LastName + " invited you on a trip!" +
                    "<br/>Trip informations<br/>" +
                     "<br/><t/>Ticket:" +
                     "<br/><t/><t/>Flight number: " + invitation.Seat.Flight.FlightNumber +
                     "<br/><t/><t/>From: " + invitation.Seat.Flight.From +
                     "<br/><t/><t/>To: " + invitation.Seat.Flight.To +
                     "<br/><t/><t/>Departure: " + invitation.Seat.Flight.TakeOffDateTime +
                     "<br/><t/><t/>Arrival: " + invitation.Seat.Flight.LandingDateTime +
                     "<br/><t/><t/>Travel time: " + invitation.Seat.Flight.TripTime +
                     "<br/><t/><t/>Travel length: " + invitation.Seat.Flight.tripLength + "km" +
                     "<br/><t/><t/>Seat details: " +
                     "<br/><t/><t/><t/>Seat number: " + invitation.Seat.Row + invitation.Seat.Column +
                     "<br/><t/><t/><t/>Class: " + invitation.Seat.Class +
                     "<br/><t/><t/>Ticket price: " + invitation.Price + "$" +
                     "<br/><br/>" +
                     "<a href='" + url + "'>Click here </a> to answer on invite!" +
                            " <br/><br/> SkyRoads";

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

            return IdentityResult.Success;
        }
        #endregion


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
    }
}
