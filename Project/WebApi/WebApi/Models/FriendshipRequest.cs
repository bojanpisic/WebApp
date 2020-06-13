using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FriendshipRequest
    {
        public string InviterId { get; set; }
        public string ReceiverId { get; set; }

    }
}
