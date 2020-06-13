using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class SpecialOfferDto
    {
        [Required]
        public float NewPrice { get; set; }
        [Required]
        public List<int> SeatsIds { get; set; }
    }
}
