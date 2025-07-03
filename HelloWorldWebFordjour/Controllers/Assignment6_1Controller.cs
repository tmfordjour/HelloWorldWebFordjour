using System.Diagnostics;
using HelloWorldWebFordjour.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HelloWorldWebFordjour.Controllers
{
    public class Assignment6_1Controller : Controller
    {
        // GET: /Assignment6_1/Index/{accessLevel}
        public ActionResult Index(int accessLevel)
        {
            // Validate accessLevel to be between 1 and 10
            if (accessLevel < 1)
            {
                accessLevel = 1; // Default to lowest access if less than 1
            }
            else if (accessLevel > 10)
            {
                accessLevel = 10; // Cap at highest access if greater than 10
            }

            // Create a list of sample students (between 3 and 5 students)
            var students = new List<Student>
            {
                new Student { FirstName = "Alice", LastName = "Smith", Grade = 95 },
                new Student { FirstName = "Bob", LastName = "Johnson", Grade = 82 },
                new Student { FirstName = "Charlie", LastName = "Brown", Grade = 78 },
                new Student { FirstName = "Diana", LastName = "Prince", Grade = 99 }
            };

            // Create the view model to pass data to the view
            var viewModel = new StudentListViewModel
            {
                Students = students,
                AccessLevel = accessLevel
            };

            // Return the view, passing the view model
            return View(viewModel);
        }
    }
}