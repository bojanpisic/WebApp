using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IAirlineRepository : IGenericRepository<Airline>
    {
        Task<IEnumerable<Airline>> GetAllAirlines();
        void UpdateAddress(Address addr);
        Task<IEnumerable<Airline>> GetTopRated();
        Task<Airline> GetAirline(int id);
    }
}
