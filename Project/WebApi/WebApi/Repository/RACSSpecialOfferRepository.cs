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
    }
}
