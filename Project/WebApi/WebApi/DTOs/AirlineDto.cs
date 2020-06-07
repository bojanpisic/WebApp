using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class AirlineDto
    {
        [Required]
        public int adminId { get; set; }
        [Required]
        public string  Name{ get; set; }
        public Address Address { get; set; }
        public string PromoDescription { get; set; }

        public string LogoUrl { get; set; }
    }
}
