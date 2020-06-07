using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Passenger
    {
        public string PassengerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Passport { get; set; }
        public User User { get; set; }
    }
}
