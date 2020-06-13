﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class FlightDto
    {
        //[Required]
        //public int AdminId { get; set; }
        //[Required]
        //public int ArilineId { get; set; }
        [Required]
        public string FlightNumber  { get; set; }
        [Required]
        public string TakeOffDateTime { get; set; }

        [Required]
        public string LandingDateTime { get; set; }

        public IList<int> StopIds{ get; set; }

        [Required]
        public int FromId { get; set; }
        [Required]
        public int ToId { get; set; }

        [Required]
        public List<SeatDto> Seats { get; set; }

        [Required]
        public float TripLength { get; set; }

    }
}
