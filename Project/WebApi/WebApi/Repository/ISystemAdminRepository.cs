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
    public interface ISystemAdminRepository
    {
        Task<IdentityResult> CreateAirlineForAdmin(Airline airline);

        //Task<IdentityResult> CreateRentServiceForAdmin(string adminUserName, RentServiceDto rentService);

    }
}
