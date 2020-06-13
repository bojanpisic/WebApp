using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class RentACarService
    {
        public int RentACarServiceId { get; set; }
        public string Name { get; set; }
        public string AdminId { get; set; }
        public RentACarServiceAdmin Admin { get; set; }
        public Address2 Address { get; set; }
        public string About { get; set; }
        public ICollection<Branch> Branches { get; set; }
        public ICollection<Car> Cars { get; set; }
        public byte[] LogoUrl { get; set; }
        public ICollection<RentCarServiceRates> Rates { get; set; }

        public RentACarService()
        {
            Branches = new HashSet<Branch>();
            Cars = new HashSet<Car>();
            Rates = new HashSet<RentCarServiceRates>();
        }
    }
}
