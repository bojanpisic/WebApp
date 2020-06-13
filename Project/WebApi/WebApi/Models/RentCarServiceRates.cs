using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RentCarServiceRates
    {
        //public int RentCarServiceRatesId { get; set; }
        public float Rate { get; set; }
        public int RentACarServiceId { get; set; }
        public RentACarService RentACarService { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
