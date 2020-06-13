using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Address
    {
        public Address()
        {

        }
        public int AddressId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public int AirlineId { get; set; }
        public virtual Airline Airline { get; set; }

    }
}
