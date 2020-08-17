﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CarRate
    {
        public int CarRateId { get; set; }

        public float Rate { get; set; }
        //public int CarId { get; set; }
        public Car Car { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public CarRate()
        {

        }
    }
}
