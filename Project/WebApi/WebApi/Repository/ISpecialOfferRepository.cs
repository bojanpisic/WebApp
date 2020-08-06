using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ISpecialOfferRepository: IGenericRepository<SpecialOffer>
    {
        Task<IEnumerable<SpecialOffer>> GetAllSpecOffers();
        Task<IEnumerable<SpecialOffer>> GetSpecialOffersOfAirline(Airline airline);
    }
}
