using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Destination
    {
        [Key]
        public int DestinationId { get; set; }
        public string ImageUrl { get; set; }
        public int AddressId { get; set; }
        public CityStateAddress Address { get; set; }
        public virtual ICollection<AirlineDestionation> Airlines { get; set; }
    }
}
