﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Seat
    {
        public int SeatId { get; set; }
        public string Column { get; set; }
        public string Row { get; set; }
        public string Class { get; set; }
        public float Price { get; set; }
        public bool Available { get; set; }
        public bool Reserved { get; set; }
        public Flight Flight { get; set; }
        //public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public Ticket2 Ticket2 { get; set; }
        //public int InvitationId { get; set; }
        public Invitation Invitation { get; set; }

        public SpecialOffer SpecialOffer { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
