using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AirlineAdmin: Person
    {
        public Airline Airline { get; set; }

        public AirlineAdmin()
        {
        }
    }
}
