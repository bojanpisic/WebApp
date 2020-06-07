using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class SystemAdminRepository : ISystemAdminRepository
    {
        public async Task<IdentityResult> CreateAirlineForAdmin(int adminId, Airline airline, DataContext context)
        {
            var admin = await context.Persons.FindAsync(adminId);

            if (admin == null)
            {
                return IdentityResult.Failed();
            }

            airline.Admin = admin as AirlineAdmin;

            context.Airlines.Add(airline);
            await context.SaveChangesAsync();

            return IdentityResult.Success;
        }
    }
}
