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
        public User User { get; set; }
        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public float Price { get; set; }

        //proveri
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Passport { get; set; }
    }
}
