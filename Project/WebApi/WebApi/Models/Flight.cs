using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public int AirlineId { get; set; }
        public Airline Airline { get; set; }
        public string FlightNumber { get; set; }
        public System.DateTime TakeOffDate { get; set; }
        public System.DateTime LandingDate { get; set; }
        public string TripTime { get; set; }
        public float tripLength { get; set; }
        public string TakeOffTime { get; set; }
        public string LandingTime { get; set; }
        public ICollection<FlightAddress> ChangeOvers { get; set; }
        public CityStateAddress From { get; set; }
        public CityStateAddress To { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}
