using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FlightReservation
    {
        public int FlightReservationId { get; set; }
        public User User { get; set; }
        public IEnumerable<Ticket> Tickets { get; set; }
        public IEnumerable<Ticket2> UnregistredFriendsTickets { get; set; }
        public float Price { get; set; }
        public DateTime ReservationDate { get; set; }

        public int? CarRentId { get; set; }
        public CarRent CarRent { get; set; }

        public FlightReservation()
        {
            Tickets = new HashSet<Ticket>();
            UnregistredFriendsTickets = new HashSet<Ticket2>();
        }
    }
}
