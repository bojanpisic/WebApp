﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Invitation
    {
        public int InvitationId { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public float Price { get; set; }
        public DateTime Expires { get; set; }

        public Invitation()
        {
        }
    }
}
