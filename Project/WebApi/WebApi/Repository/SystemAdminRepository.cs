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
        private readonly DataContext context;
        public SystemAdminRepository(DataContext _context)
        {
            context = _context;
        }
        public async Task<IdentityResult> CreateAirlineForAdmin(Airline airline)
        {
            context.Airlines.Add(airline);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = "Add error"});
        }
    }
}
