using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class BranchDto
    {
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
    }
}
