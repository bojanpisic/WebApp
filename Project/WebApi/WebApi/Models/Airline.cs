using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Airline
    {

        public Airline()
        {
            Destinations = new HashSet<AirlineDestionation>();
            Flights = new HashSet<Flight>();
            Rates = new HashSet<AirlineRate>();
        }
        public int AirlineId { get; set; }
        public AirlineAdmin Admin { get; set; }

        public string Name { get; set; }
        public string PromoDescription { get; set; }
        public string LogoUrl { get; set; }
        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<AirlineDestionation> Destinations { get; set; }

        public virtual ICollection<Flight> Flights { get; set; }

        public virtual ICollection<AirlineRate> Rates { get; set; }
    }
}
