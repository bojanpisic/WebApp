﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IDestinationRepository: IGenericRepository<Destination>
    {
        Task<IEnumerable<Destination>> GetAirlineDestinations(Airline airline);

    }
}
