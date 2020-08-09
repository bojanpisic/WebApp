using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class RACSSpecialOfferRepository : GenericRepository<CarSpecialOffer>, IRACSSpecialOffer
    {
        public RACSSpecialOfferRepository(DataContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfRacs(int racsId)
        {
            return await context.CarSpecialOffers.Include(s => s.Car)
                .ThenInclude(c => c.Branch)
                .Include(s => s.Car)
                .ThenInclude(c => c.RentACarService)
                .Where(spec => spec.Car.Branch.RentACarServiceId == racsId || spec.Car.RentACarServiceId == racsId)
                .ToListAsync();
        }
    }
}
