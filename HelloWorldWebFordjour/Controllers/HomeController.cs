using System.Diagnostics;
using HelloWorldWebFordjour.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloWorldWebFordjour.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // 1. Default Routing Page
        // This action method will be accessible via default routing (e.g., /Home/Index or just / if configured as default).
        public IActionResult Index()
        {
            ViewBag.Title = "Welcome Home!";
            return View(); // Renders Views/Home/Index.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
