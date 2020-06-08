using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class LoginDto
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        //[DataType(DataType.EmailAddress)]
        //[EmailAddress]
        //public string Email { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        public string IdToken { get; set; }
        //public string ReturnUrl { get; set; }
        //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    }
}
