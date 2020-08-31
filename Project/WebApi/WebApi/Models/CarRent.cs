using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CarRent
    {
        public int CarRentId { get; set; }
        public DateTime TakeOverDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string TakeOverCity { get; set; }
        public string ReturnCity { get; set; }
        public float TotalPrice { get; set; }
        public Car RentedCar { get; set; }
        public User User { get; set; }
        public bool IsCarRated { get; set; }
        public bool IsRACSRated { get; set; }
        public DateTime RentDate { get; set; }
        public FlightReservation FlightReservation { get; set; }

        public CarRent()
        {
            IsCarRated = false;
            IsRACSRated = false;
        }
    }
}
