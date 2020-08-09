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
            RateAirline = new HashSet<AirlineRate>();
            RateRACService = new HashSet<RentCarServiceRates>();
            FriendshipRequests = new HashSet<Friendship>();
            FriendshipInvitations = new HashSet<Friendship>();
            Friends = new HashSet<User>();
            CarRents = new HashSet<CarRent>();
        }
        public ICollection<Ticket> FlightReservations { get; set; }
        public ICollection<CarRent> CarRents { get; set; }

        public ICollection<AirlineRate> RateAirline { get; set; }
        public ICollection<RentCarServiceRates> RateRACService { get; set; }
        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<Friendship> FriendshipRequests { get; set; }
        public virtual ICollection<Friendship> FriendshipInvitations { get; set; }

        //public virtual ICollection<User> Friends { get; set; }
    }
}
