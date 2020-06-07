using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FlightAddress
    {
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
        public int CityStateAddressId { get; set; }
        public CityStateAddress CityStateAddress { get; set; }
    }
}
