using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Address2
    {
        public Address2()
        {

        }
        public int Address2Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int RentACarServiceId { get; set; }
        public RentACarService RentACarService { get; set; }
    }
}
