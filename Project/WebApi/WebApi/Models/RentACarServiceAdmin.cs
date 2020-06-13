using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RentACarServiceAdmin: Person
    {
        public RentACarService RentACarService { get; set; }

        public RentACarServiceAdmin()
        {
        }
    }
}
