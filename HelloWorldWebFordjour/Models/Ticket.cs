using System.ComponentModel.DataAnnotations;

namespace HelloWorldWebFordjour.Models
{
    // Enum for Ticket Status
    public enum TicketStatus
    {
        [Display(Name = "To Do")]
        ToDo,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Quality Assurance")]
        QA,
        [Display(Name = "Done")]
        Done
    }

    public class Ticket
    {
        public int Id { get; set; } // Unique identifier for each ticket

        [Required(ErrorMessage = "Ticket Name is required.")]
        [StringLength(100, ErrorMessage = "Ticket Name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        [Display(Name = "Description")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Sprint Number is required.")]
        [Display(Name = "Sprint #")]
        [Range(1, int.MaxValue, ErrorMessage = "Sprint number must be positive.")]
        public int SprintNumber { get; set; }

        [Required(ErrorMessage = "Point Value is required.")]
        [Display(Name = "Point Value")]
        [Range(1, 10, ErrorMessage = "Point value must be between 1 and 10.")] // Example range
        public int PointValue { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public TicketStatus Status { get; set; }
    }
}