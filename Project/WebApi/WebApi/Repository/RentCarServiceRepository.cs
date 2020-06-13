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
    public class RentCarServiceRepository : IRentCarServiceRepository
    {
        private readonly DataContext context;
        private readonly UserManager<Person> userManager;
        public RentCarServiceRepository(DataContext _context, UserManager<Person> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        public async Task<IdentityResult> AddBranch(Branch branch)
        {
            context.Branches.Add(branch);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Add error" });
        }

        public async Task<IdentityResult> AddCar(Car car)
        {
            context.Cars.Add(car);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Error when trying to add car" });
        }

        public async Task<ICollection<RentACarService>> GetAllRACS()
        {
            return await context.RentACarServices.Include(c => c.Address).Include(c => c.Branches).ToListAsync();
        }

        public async Task<Branch> GetBranchById(int branchId)
        {
            return await context.Branches.Include(b => b.Cars).Include(b => b.RentACarService).FirstOrDefaultAsync(b => b.BranchId == branchId);
        }

        public async Task<IEnumerable<Car>> GetCarsOfRACS(int racsId)
        {
            return await context.Cars.Where(c => c.RentACarServiceId == racsId).ToListAsync();
        }

        public async Task<RentACarService> GetRACSByAdmin(string adminId)
        {
            return await context.RentACarServices.Include(a => a.Address).Include(a => a.Branches).FirstOrDefaultAsync(a => a.AdminId == adminId);
        }

        public async Task<ICollection<Branch>> GetRACSBranches(RentACarService racs)
        {
            return await context.Branches.Where(b => b.RentACarService == racs).ToListAsync();
        }

        public async Task<RentACarService> GetRACSById(int racsId)
        {
            return await context.RentACarServices.FirstOrDefaultAsync(r => r.RentACarServiceId == racsId);
        }

        public async Task<Car> GetRACSCarById(int carId)
        {
            return await context.Cars.Include(c => c.SpecialOffers).FirstOrDefaultAsync(c => c.CarId == carId);
        }

        public async Task<IEnumerable<RentACarService>> GetTopRated()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> RemoveBranch(Branch branch)
        {
            throw new NotImplementedException();
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

        public async Task<IdentityResult> UpdateRACS(RentACarService racs)
        {
            context.Entry(racs).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Description = "Update error" });
        }

        public async Task<IEnumerable<Car>> GetRACSCars(RentACarService racs)
        {
            return await context.Cars.Where(c => c.RentACarService == racs).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetBranchCars(Branch branch)
        {
            return await context.Cars.Where(c => c.Branch == branch).ToListAsync();
        }

        public async Task<RentACarService> GetRACSAndCars(string adminId)  // kupi i od filijala auta
        {
            return await context.RentACarServices.Include(r => r.Cars).Include(r => r.Address).Include(r => r.Branches).ThenInclude(b => b.Cars).FirstOrDefaultAsync(r=>r.AdminId == adminId);
        }

        public async Task<IdentityResult> DeleteBranch(Branch branch)
        {
            context.Branches.Remove(branch);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Add error" });
        }

        public async Task<IdentityResult> UpdateCar(Car car)
        {
            context.Entry(car).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Description = "Update error" });
        }

        public async Task<IdentityResult> DeleteCar(Car car)
        {
            context.Cars.Remove(car);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Add error" });
        }

        public async Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfCar(Car car)
        {
            return await context.CarSpecialOffers.Where(s => s.Car == car).ToListAsync();
        }

        public async Task<IdentityResult> AddSpecOffer(CarSpecialOffer specOffer)
        {
            context.CarSpecialOffers.Add(specOffer);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Error when trying to add special offer" });
        }

        public Task<IdentityResult> DeleteSpecOffer(CarSpecialOffer specOffer)
        {
            throw new NotImplementedException();
        }

        public async Task<CarSpecialOffer> GetSpecialOfferById(int id)
        {
            return await context.CarSpecialOffers.FirstOrDefaultAsync(s => s.CarSpecialOfferId == id);
        }

        public async Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfRacs(int racsId)
        {
            return await context.CarSpecialOffers.Include(s => s.Car)
                .ThenInclude(c => c.Branch)
                .Include(s => s.Car)
                .ThenInclude(c => c.RentACarService)
                .Where(spec => spec.Car.Branch.RentACarServiceId == racsId || spec.Car.RentACarServiceId ==racsId)
                .ToListAsync();
        }

        public async Task<RentACarService> RACSByIdInclude(int racsId)
        {
            return await context.RentACarServices.Include(r => r.Address).Include(r => r.Branches).FirstOrDefaultAsync(r => r.RentACarServiceId == racsId);
        }

        public async Task<IEnumerable<Car>> AllCars()
        {
            return await context.Cars.Include(c => c.SpecialOffers)
                .Include(c => c.Branch)
                .ThenInclude(b=>b.RentACarService)
                .Include(c => c.RentACarService)
                .ThenInclude(r => r.Address)
                .Include(c => c.RentACarService)
                .ThenInclude(r => r.Branches)
                .ToListAsync();
        }

        public async Task<Car> GetCarById(int carId)
        {
            return await context.Cars.Include(c => c.Branch).Include(c => c.RentACarService).ThenInclude(r => r.Address).FirstOrDefaultAsync(c => c.CarId ==carId);
        }


    }
}
