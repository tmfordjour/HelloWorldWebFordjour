using HelloWorldWebFordjour.Data; // For TicketsDbContext
using HelloWorldWebFordjour.Interfaces; // For ITicketRepository
using HelloWorldWebFordjour.Models; // For Ticket model
using Microsoft.EntityFrameworkCore; // For EF Core methods 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorldWebFordjour.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketsDbContext _context;

        public TicketRepository(TicketsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                                 .OrderBy(t => t.SprintNumber)
                                 .ThenBy(t => (int)t.Status)
                                 .ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            // FindAsync is efficient for primary key lookups
            return await _context.Tickets.FindAsync(id);
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            _context.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTicketAsync(Ticket ticket)
        {
            // Marked the entity as modified so EF Core knows to update it
            _context.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTicketAsync(int id)
        {
            var ticketToRemove = await _context.Tickets.FindAsync(id);
            if (ticketToRemove != null)
            {
                _context.Tickets.Remove(ticketToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TicketExistsAsync(int id)
        {
            return await _context.Tickets.AnyAsync(e => e.Id == id);
        }
    }
}