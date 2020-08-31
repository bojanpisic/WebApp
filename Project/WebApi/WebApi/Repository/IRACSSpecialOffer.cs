using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IRACSSpecialOffer: IGenericRepository<CarSpecialOffer>
    {
        Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfRacs(int racsId);
        Task<IEnumerable<CarSpecialOffer>> GetSpecialOffersOfAllRacs();
        Task<CarSpecialOffer> GetSpecialOfferById(int specialOfferId);
    }
}
