using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class RegistrationDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "You must specify password between 6 and 20 characters")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [RegularExpression("^[(][+][0-9]{3}[)][0-9]{2}[/][0-9]{3}[-][0-9]{3,4}", ErrorMessage = "Please enter valid phone no.")]
        public string Phone { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
