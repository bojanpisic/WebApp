using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public FlightReservation Reservation { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public float Price { get; set; }
        public string Passport { get; set; }
    }
}
