using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class SocialNetworkDto
    {
        public string Email { get; set; }
        public string IdToken { get; set; }
        public string Provider { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
