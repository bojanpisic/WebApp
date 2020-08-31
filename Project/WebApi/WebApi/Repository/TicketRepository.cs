using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(DataContext context) : base(context)
        {
        }

        public async Task<Ticket> GetTicket(int ticketId) 
        {
            return await context.Tickets
                .Include(t => t.Seat)
                .ThenInclude(s => s.Flight)
                .ThenInclude(f => f.Airline)
                .FirstOrDefaultAsync(t => t.TicketId == ticketId);
        }
    }
}
