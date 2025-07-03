using Microsoft.AspNetCore.Mvc;

namespace HelloWorldWebFordjour.Controllers
{
    public class AdminController : Controller
    {
        // Admin Area Home Page
        public IActionResult Index()
        {
            ViewBag.Title = "Admin Dashboard";
            return View(); // Renders Views/Admin/Index.cshtml
        }
    }
}
