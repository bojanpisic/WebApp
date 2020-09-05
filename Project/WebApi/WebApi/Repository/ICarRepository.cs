using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ICarRepository: IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> AllCars(Expression<Func<Car, bool>> filter = null);
        Task<IEnumerable<Car>> CarsOfBranch(int id);
    }
}
