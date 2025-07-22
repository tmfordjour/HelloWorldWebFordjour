using HelloWorldWebFordjour.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelloWorldWebFordjour.ViewComponents
{
    public class TicketStatusButtonViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(TicketStatus currentStatus)
        {
            string displayText = currentStatus.ToString();
            string buttonClass = "btn btn-sm text-white "; // Base Bootstrap classes for button look

            // Determine button styling based on current status
            switch (currentStatus)
            {
                case TicketStatus.ToDo:
                    buttonClass += "btn-secondary"; // Grey
                    break;
                case TicketStatus.InProgress:
                    buttonClass += "btn-primary"; // Blue
                    break;
                case TicketStatus.QA:
                    buttonClass += "btn-info"; // Light Blue
                    break;
                case TicketStatus.Done:
                    buttonClass += "btn-success"; // Green
                    break;
                default:
                    buttonClass += "btn-light"; // Light
                    break;
            }

            // Create a simple model to pass to the View Component's Razor view
            var model = new TicketStatusDisplayModel
            {
                DisplayText = displayText,
                ButtonClass = buttonClass
            };

            return View(model);
        }

        // --- Helper Model for the View Component View ---
        public class TicketStatusDisplayModel
        {
            public string DisplayText { get; set; } = string.Empty;
            public string ButtonClass { get; set; } = string.Empty;
        }
    }
}