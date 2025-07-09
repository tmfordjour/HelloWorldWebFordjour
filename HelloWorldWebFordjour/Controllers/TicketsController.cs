using Microsoft.AspNetCore.Mvc;
using HelloWorldWebFordjour.Models; 
using System.Collections.Generic;
using System.Linq; 

namespace HelloWorldWebFordjour.Controllers
{
    public class TicketsController : Controller
    {
        // Static list 
        // This data will reset when the application restarts.
        private static List<Ticket> _tickets = new List<Ticket>
        {
            // Seed some initial data
            new Ticket { Id = 1, Name = "Implement Favorites Feature", Description = "Add ability to save and view favorite countries.", SprintNumber = 1, PointValue = 8, Status = TicketStatus.Done },
            new Ticket { Id = 2, Name = "Create ToDo List Module", Description = "Develop ticket creation, viewing, and editing.", SprintNumber = 2, PointValue = 10, Status = TicketStatus.InProgress },
            new Ticket { Id = 3, Name = "Design Landing Page", Description = "Create a welcoming home page for the Olympics site.", SprintNumber = 2, PointValue = 5, Status = TicketStatus.ToDo },
            new Ticket { Id = 4, Name = "Fix Footer Alignment", Description = "Adjust CSS for consistent footer positioning.", SprintNumber = 1, PointValue = 2, Status = TicketStatus.QA }
        };

        // Used to generate unique IDs for new tickets
        private static int _nextId = _tickets.Any() ? _tickets.Max(t => t.Id) + 1 : 1;

        // GET: Tickets (List all tickets)
        public IActionResult Index()
        {
            // Order tickets by Sprint Number and then by Status (ToDo first)
            var sortedTickets = _tickets.OrderBy(t => t.SprintNumber)
                                        .ThenBy(t => (int)t.Status) // Order by enum value (ToDo=0, InProgress=1, etc.)
                                        .ToList();
            return View(sortedTickets);
        }

        // GET: Tickets/Create (Display the form to create a new ticket)
        public IActionResult Create()
        {
            return View(); // Return an empty view for the form
        }

        // POST: Tickets/Create (Handle form submission for new ticket)
        [HttpPost]
        [ValidateAntiForgeryToken] // Recommended for POST actions to prevent CSRF attacks
        public IActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid) // Check if model validation passes
            {
                ticket.Id = _nextId++; // Assign a unique ID
                _tickets.Add(ticket);
                TempData["Message"] = "Ticket added successfully!"; // Optional: show success message
                return RedirectToAction(nameof(Index)); // Redirect to the list view
            }
            // If validation fails, return the view with validation errors
            return View(ticket);
        }

        // GET: Tickets/Edit/{id} (Display the form to edit an existing ticket)
        public IActionResult Edit(int id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound(); // Return 404 if ticket not found
            }
            return View(ticket);
        }

        // POST: Tickets/Edit (Handle form submission for updating a ticket)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var existingTicket = _tickets.FirstOrDefault(t => t.Id == ticket.Id);
                if (existingTicket != null)
                {
                    // Update properties of the existing ticket
                    existingTicket.Name = ticket.Name;
                    existingTicket.Description = ticket.Description;
                    existingTicket.SprintNumber = ticket.SprintNumber;
                    existingTicket.PointValue = ticket.PointValue;
                    existingTicket.Status = ticket.Status;
                    TempData["Message"] = "Ticket updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                return NotFound(); // Should not happen if Id is correct
            }
            return View(ticket); // Return view with errors if validation fails
        }

        // GET: Tickets/Delete/{id} (Display confirmation for deleting a ticket) - Optional but good practice
        public IActionResult Delete(int id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete (Handle deletion of a ticket)
        [HttpPost, ActionName("Delete")] // Map to POST /Tickets/Delete, but use "DeleteConfirmed" internally
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var ticketToRemove = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticketToRemove != null)
            {
                _tickets.Remove(ticketToRemove);
                TempData["Message"] = "Ticket deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}