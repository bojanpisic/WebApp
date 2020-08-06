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
        //Task<RentACarService> GetRACSByAdmin(string adminId);
        Task<RentACarService> GetRACSAndCars(string adminId);
        //Task<ICollection<RentACarService>> GetAllRACS();
        //Task<IdentityResult> UpdateRACS(RentACarService racs);
        //Task<IEnumerable<Car>> GetRACSCars(RentACarService racs);
        Task<IEnumerable<Car>> GetBranchCars(Branch branch); //?
        Task<IdentityResult> UpdateAddress(Address2 addr);
        //Task<RentACarService> GetRACSById(int racsId);
        //Task<IdentityResult> AddCar(Car car);
        //Task<IEnumerable<Car>> GetCarsOfRACS(int racsId);
        //Task<Car> GetRACSCarById(int carId);
        //Task<ICollection<Branch>> GetRACSBranches(RentACarService racs);
        //Task<Branch> GetBranchById(int branchId);
        //Task<IdentityResult> AddBranch(Branch branch);
        //Task<IdentityResult> RemoveBranch(Branch branch);
        Task<IEnumerable<RentACarService>> GetTopRated();

        //Task<IdentityResult> DeleteBranch(Branch branch);
        //Task<IdentityResult> UpdateCar(Car car);
        //Task<IdentityResult> DeleteCar(Car car);

        Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfCar(Car car);
        Task<IdentityResult> AddSpecOffer(CarSpecialOffer specOffer);
        Task<IdentityResult> DeleteSpecOffer(CarSpecialOffer specOffer);
        Task<CarSpecialOffer> GetSpecialOfferById(int id);

        Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfRacs(int racsId);
        Task<IEnumerable<Car>> AllCars();

        Task<RentACarService> RACSByIdInclude(int racsId);

        Task<Car> GetCarById(int carId);

    }
}
