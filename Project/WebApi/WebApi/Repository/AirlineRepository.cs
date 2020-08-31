using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Repository
{
    public class AirlineRepository : GenericRepository<Airline>, IAirlineRepository
    {
        public AirlineRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Airline>> GetAllAirlines()
        {
            return await context.Airlines
                .Include(a=>a.Destinations)
                .ThenInclude(d=>d.Destination)
                .Include(a => a.Rates)
                .Include(a => a.Address)
                .ToListAsync();
        }

        public void UpdateAddress(Address addr)
        {
            context.Entry(addr).State = EntityState.Modified;
        }

        public async Task<Airline> GetAirline(int id)
        {
            return await context.Airlines
                .Include(a => a.Destinations)
                .ThenInclude(d => d.Destination)
                .Include(a => a.Address)
                .Include(a => a.Rates)
                .FirstOrDefaultAsync(a => a.AirlineId == id);
        }
    }
}
