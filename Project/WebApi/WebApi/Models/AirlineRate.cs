﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AirlineRate
    {
        public int AirlineRateId { get; set; }
        public float  Rate { get; set; }

        public Airline Airline { get; set; }
    }
}
