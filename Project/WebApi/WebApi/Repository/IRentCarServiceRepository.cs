using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IRentCarServiceRepository : IGenericRepository<RentACarService>
    {
        Task<RentACarService> GetRACSAndCars(string adminId);
        Task<IdentityResult> UpdateAddress(Address2 addr);
        Task<IEnumerable<RentACarService>> GetTopRated();
    }
}
