using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ICarRepository: IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> AllCars();
        Task<IEnumerable<Car>> CarsOfBranch(int id);
    }
}
