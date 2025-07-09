using HelloWorldWebFordjour.Models;
using HelloWorldWebFordjour.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorldWebFordjour.Controllers
{
    public class DataTransferController : Controller
    {
        // Sample data (in a real application, you'd get this from a database)
        public List<Country> GetCountries()
        {
            return new List<Country>
            {
                new Country { Name = "Canada", Game = "Winter Olympics", Category = "Indoor", FlagPath = "canada.svg" },
                new Country { Name = "Sweden", Game = "Winter Olympics", Category = "Indoor", FlagPath = "sweden.svg" },
                new Country { Name = "Great Britain", Game = "Winter Olympics", Category = "Indoor", FlagPath = "greatbritain.svg" },
                new Country { Name = "Jamaica", Game = "Winter Olympics", Category = "Outdoor", FlagPath = "jamaica.png" },
                new Country { Name = "Italy", Game = "Winter Olympics", Category = "Outdoor", FlagPath = "italy.png" },
                new Country { Name = "Japan", Game = "Winter Olympics", Category = "Outdoor", FlagPath = "japan.svg" },
                new Country { Name = "Germany", Game = "Summer Olympics", Category = "Indoor", FlagPath = "germany.png" },
                new Country { Name = "China", Game = "Summer Olympics", Category = "Indoor", FlagPath = "china.svg" },
                new Country { Name = "Mexico", Game = "Summer Olympics", Category = "Indoor", FlagPath = "mexico.png" },
                new Country { Name = "Brazil", Game = "Summer Olympics", Category = "Outdoor", FlagPath = "brazil.svg" },
                new Country { Name = "Netherlands", Game = "Summer Olympics", Category = "Outdoor", FlagPath = "netherlands.svg" },
                new Country { Name = "USA", Game = "Summer Olympics", Category = "Outdoor", FlagPath = "usa.svg" },
                new Country { Name = "Thailand", Game = "Paralympics", Category = "Indoor", FlagPath = "thailand.svg" },
                new Country { Name = "Uruguay", Game = "Paralympics", Category = "Indoor", FlagPath = "uruguay.svg" },
                new Country { Name = "Ukraine", Game = "Paralympics", Category = "Indoor", FlagPath = "ukraine.svg" },
                new Country { Name = "Austria", Game = "Paralympics", Category = "Outdoor", FlagPath = "austria.svg" },
                new Country { Name = "Pakistan", Game = "Paralympics", Category = "Outdoor", FlagPath = "pakistan.png" },
                new Country { Name = "Zimbabwe", Game = "Paralympics", Category = "Outdoor", FlagPath = "zimbabwe.png" },
                new Country { Name = "France", Game = "Youth Olympic Games", Category = "Indoor", FlagPath = "france.svg" },
                new Country { Name = "Cyprus", Game = "Youth Olympic Games", Category = "Indoor", FlagPath = "cyprus.png" },
                new Country { Name = "Russia", Game = "Youth Olympic Games", Category = "Indoor", FlagPath = "russia.svg" },
                new Country { Name = "Finland", Game = "Youth Olympic Games", Category = "Outdoor", FlagPath = "finland.svg" },
                new Country { Name = "Slovakia", Game = "Youth Olympic Games", Category = "Outdoor", FlagPath = "slovakia.svg" },
                new Country { Name = "Portugal", Game = "Youth Olympic Games", Category = "Outdoor", FlagPath = "portugal.svg" }
            };
        }

        // Modified Index action to accept CountryFilterViewModel
        public IActionResult Index(CountryFilterViewModel model) // <--- Changed parameter type
        {
            var countries = GetCountries();

            // Apply filtering based on ViewModel properties
            if (!string.IsNullOrEmpty(model.GameFilter) && model.GameFilter != "ALL")
            {
                countries = countries.Where(c => c.Game == model.GameFilter).ToList();
            }

            if (!string.IsNullOrEmpty(model.CategoryFilter) && model.CategoryFilter != "ALL")
            {
                countries = countries.Where(c => c.Category == model.CategoryFilter).ToList();
            }

            // Sort countries alphabetically
            countries = countries.OrderBy(c => c.Name).ToList();

            // You might want to pass the model back to the view to maintain filter state
            ViewBag.CurrentFilters = model; // Store filters in ViewBag for now, or pass model directly

            // Pass the filtered and sorted countries to the view
            return View(countries); // Or return View(new DataTransferViewModel { Filters = model, Countries = countries }); if you create a larger viewmodel
        }
    }
}
