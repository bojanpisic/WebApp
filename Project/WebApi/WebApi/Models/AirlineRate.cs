using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AirlineRate
    {
        public float  Rate { get; set; }
        public int AirlineId { get; set; }
        public Airline Airline { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
