using HelloWorldWebFordjour.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloWorldWebFordjour.Data
{
    public class TicketsDbContext : DbContext
    {
        public TicketsDbContext(DbContextOptions<TicketsDbContext> options)
            : base(options)
        {
        }

        // DbSet for the Ticket model, representing the 'Tickets' table in the database
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial data to be created with migrations
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket { Id = 1, Name = "Implement Favorites Feature", Description = "Add ability to save and view favorite countries.", SprintNumber = 1, PointValue = 8, Status = TicketStatus.Done },
                new Ticket { Id = 2, Name = "Create ToDo List Module", Description = "Develop ticket creation, viewing, and editing.", SprintNumber = 2, PointValue = 10, Status = TicketStatus.InProgress },
                new Ticket { Id = 3, Name = "Design Landing Page", Description = "Create a welcoming home page for the Olympics site.", SprintNumber = 2, PointValue = 5, Status = TicketStatus.ToDo },
                new Ticket { Id = 4, Name = "Fix Footer Alignment", Description = "Adjust CSS for consistent footer positioning.", SprintNumber = 1, PointValue = 2, Status = TicketStatus.QA }
            );
        }
    }
}