﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class ChangeImgUrlDto
    {
        [Required]
        public byte[] ImgUrl { get; set; }
    }
}
