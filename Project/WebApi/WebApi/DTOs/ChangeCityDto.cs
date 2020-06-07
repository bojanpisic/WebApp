﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class ChangeCityDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string City { get; set; }
    }
}
