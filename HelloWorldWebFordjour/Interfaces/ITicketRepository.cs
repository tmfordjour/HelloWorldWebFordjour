using HelloWorldWebFordjour.Models;
using System.Collections.Generic;
using System.Threading.Tasks; // Needed for async methods

namespace HelloWorldWebFordjour.Interfaces
{
    public interface ITicketRepository
    {
        // CRUD operations
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketByIdAsync(int id); // Use nullable if not found
        Task AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id); 

        // Utility check
        Task<bool> TicketExistsAsync(int id);
    }
}