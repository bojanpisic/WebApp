﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Destination
    {
        [Key]
        public int DestinationId { get; set; }
        public byte[] ImageUrl { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public virtual ICollection<Flight> From { get; set; }
        public virtual ICollection<Flight> To { get; set; }
        public virtual ICollection<FlightDestination> Flights { get; set; }
        public virtual ICollection<AirlineDestionation> Airlines { get; set; }

        public Destination()
        {
            From = new HashSet<Flight>();
            To = new HashSet<Flight>();
            Flights = new HashSet<FlightDestination>();
        }
    }
}
