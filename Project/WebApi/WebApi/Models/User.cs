using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User: Person
    {
        public User()
        {
            FlightReservations = new HashSet<Ticket>();
        }
        public ICollection<Ticket> FlightReservations { get; set; }
        //public string ActivationCode { get; set; }
    }
}
