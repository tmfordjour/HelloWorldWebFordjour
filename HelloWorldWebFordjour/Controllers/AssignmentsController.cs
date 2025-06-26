using Microsoft.AspNetCore.Mvc;

namespace YourWebAppName.Controllers
{
    public class AssignmentsController : Controller
    {
        // 2. Custom Routing Rule Page (example of a dedicated controller for a specific area)
        // This action might be hit by a custom route defined in Program.cs
        public IActionResult Organize()
        {
            ViewBag.Title = "Assignments";
            return View(); // Renders Views/Assignments/Organize.cshtml
        }
    }
}
