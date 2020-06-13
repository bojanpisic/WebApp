using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.DTOs
{
    public class AddFriendDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
