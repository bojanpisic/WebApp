using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class RentCarServiceRepository : GenericRepository<RentACarService>, IRentCarServiceRepository
    {
        public RentCarServiceRepository(DataContext context) : base(context)
        {
        }

        public async Task<IdentityResult> UpdateAddress(Address2 addr)
        {
            context.Entry(addr).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Code = "Update error" });
        }

        public async Task<RentACarService> GetRACSAndCars(string adminId)  // kupi i od filijala auta
        {
            return await context.RentACarServices
                .Include(r => r.Cars)
                .Include(r => r.Address)
                .Include(r => r.Branches)
                .ThenInclude(b => b.Cars)
                .ThenInclude(c => c.Rates)
                .FirstOrDefaultAsync(r=>r.AdminId == adminId);
        }
    }
}
