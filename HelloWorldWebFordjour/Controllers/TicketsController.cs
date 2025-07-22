using Microsoft.AspNetCore.Mvc;
using HelloWorldWebFordjour.Models;
using System.Collections.Generic;
using System.Linq;
using HelloWorldWebFordjour.Data;
using HelloWorldWebFordjour.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace HelloWorldWebFordjour.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _ticketRepository; // Declare a private field for the DbticketRepository

        // Constructor to inject the ITicketRepository        
        public TicketsController(ITicketRepository ticketRepository) 
        {
            _ticketRepository = ticketRepository;
        }

        // GET: Tickets (List all tickets)
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketRepository.GetAllTicketsAsync(); // These tickets are already sorted at the database level
            return View(tickets);
        }

        // GET: Tickets/Create (Display the form to create a new ticket)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create (Handle form submission for new ticket)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,SprintNumber,PointValue,Status")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _ticketRepository.AddTicketAsync(ticket);
                TempData["Message"] = "Ticket added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/{id} (Display the form to edit an existing ticket)
        public async Task<IActionResult> Edit(int? id) // Make ID nullable
        {
            if (id == null)
            {
                return NotFound();
            }

            // Use the repository method to get the ticket
            var ticket = await _ticketRepository.GetTicketByIdAsync(id.Value); // .Value since id is nullable
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }
        
        // POST: Tickets/Edit (Handle form submission for updating a ticket)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,SprintNumber,PointValue,Status")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use the repository method to update the ticket
                    await _ticketRepository.UpdateTicketAsync(ticket);
                    TempData["Message"] = "Ticket updated successfully!";
                }
                catch (DbUpdateConcurrencyException) // This exception originates from EF Core in the repository
                {
                    // Use the repository method to check existence
                    if (!await _ticketRepository.TicketExistsAsync(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Re-throw if it's another concurrency issue
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }
        
        // GET: Tickets/Delete/{id} (Display confirmation for deleting a ticket)
        public async Task<IActionResult> Delete(int? id) // Make ID nullable
        {
            if (id == null)
            {
                return NotFound();
            }

            // Use the repository method to get the ticket
            var ticket = await _ticketRepository.GetTicketByIdAsync(id.Value);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete (Handle deletion of a ticket)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Use the repository method to delete the ticket
            await _ticketRepository.DeleteTicketAsync(id); // Repository handles finding and removing
            TempData["Message"] = "Ticket deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}