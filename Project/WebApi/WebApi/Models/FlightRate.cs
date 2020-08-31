using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FlightRate
    {
        public int FlightRateId { get; set; }
        public float Rate { get; set; }
        public Flight Flight { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public FlightRate()
        {
        }
    }
}
