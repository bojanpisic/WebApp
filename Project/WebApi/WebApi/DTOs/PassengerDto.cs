using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class PassengerDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Passport { get; set; }
        [Required]
        public List<int> SeatsIds { get; set; }

    }
}
