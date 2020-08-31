using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Car>> AllCars()
        {
            return await context.Cars
                .Include(c => c.SpecialOffers)
                .Include(c => c.Branch)
                .ThenInclude(b => b.RentACarService)
                .Include(c => c.RentACarService)
                .ThenInclude(r => r.Address)
                .Include(c => c.RentACarService)
                .ThenInclude(r => r.Branches)
                .Include(c => c.Rates)
                .Include(c =>c.Rents)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> CarsOfBranch(int id)
        {
            return await context.Cars
                .Include(c => c.Branch)
                .ThenInclude(b => b.RentACarService)
                .ThenInclude(r => r.Address)
                .Include(c => c.Rates)
                .Where(c => c.BranchId == id)
                .ToListAsync();
        }
    }
}
