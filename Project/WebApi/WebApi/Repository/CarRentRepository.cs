using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CarRentRepository : GenericRepository<CarRent>, ICarRentRepository
    {
        public CarRentRepository(DataContext context) : base(context)
        {
        }

        //public async Task<CarRent> GetRent(int id)
        //{
        //    return await context.CarRents.Include(c => c.RentedCar).FirstOrDefaultAsync(c => c.CarRentId == id);
        //}

        public async Task<IEnumerable<CarRent>> GetRents(User user)
        {
            return await context.CarRents
                .Include(c => c.RentedCar)
                .ThenInclude(car => car.Branch)
                .Include(c => c.RentedCar)
                .ThenInclude(car => car.RentACarService)
                .ThenInclude(racs => racs.Address)
                .Include(c => c.RentedCar)
                .ThenInclude(c => c.Rates)
                .Where(c => c.User == user).ToListAsync();
        }

        public async Task<CarRent> GetRentByFilter(Expression<Func<CarRent, bool>> filter = null)
        {
            return await context.CarRents
                .Include(c => c.RentedCar)
                    .ThenInclude(c => c.Rates)
                .FirstOrDefaultAsync(filter);
        }
    }
}
