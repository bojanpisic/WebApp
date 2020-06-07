using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AirlineAdmin: Person
    {
        public int AirlineId { get; set; }
        public Airline Airline { get; set; }
    }
}
