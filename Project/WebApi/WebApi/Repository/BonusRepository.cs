using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class BonusRepository : GenericRepository<Bonus>, IBonusRepository
    {
        public BonusRepository(DataContext context) : base(context)
        {
        }
    }
}
