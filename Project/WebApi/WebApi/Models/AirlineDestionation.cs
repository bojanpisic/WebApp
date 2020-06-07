using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AirlineDestionation
    {
        //[Column(Order = 0), Key]
        public int AirlineId { get; set; }
        public Airline Airline { get; set; }

        //[Column(Order = 1), Key]
        public int DestinationId { get; set; }
        public Destination Destination { get; set; }
    }
}
