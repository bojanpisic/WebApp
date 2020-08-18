using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ICarRentRepository: IGenericRepository<CarRent>
    {
        Task<IEnumerable<CarRent>> GetRents(User user);
    }
}
