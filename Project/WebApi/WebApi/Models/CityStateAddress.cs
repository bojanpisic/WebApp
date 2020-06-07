using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CityStateAddress
    {
        public int CityStateAddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual ICollection<Flight> From { get; set; }
        public virtual ICollection<Flight> To { get; set; }
        public virtual ICollection<FlightAddress> Flights { get; set; }

        public virtual Destination Destination { get; set; }

        public CityStateAddress()
        {
            From = new HashSet<Flight>();
            To = new HashSet<Flight>();
            Flights = new HashSet<FlightAddress>();
        }
    }
}
