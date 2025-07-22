using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Required for session extensions
using HelloWorldWebFordjour.Models; // Assuming your Country model is here
using Newtonsoft.Json; // Required for JSON serialization/deserialization, you might need to install this NuGet package.
using System.Collections.Generic;
using System.Linq;

namespace HelloWorldWebFordjour.Controllers
{
    public class FavoritesController : Controller
    {
        // Key for storing favorites in session
        private const string FavoritesSessionKey = "Favorites";

        // Action to display favorite countries
        public IActionResult Index()
        {
            // Retrieve favorites from session
            List<Country> favorites = HttpContext.Session.GetObjectFromJson<List<Country>>(FavoritesSessionKey) ?? new List<Country>();

            // Pass the list of favorite countries to the view
            return View(favorites);
        }

        // Action to add a country to favorites
        // This will be called when the "Add to Favorites" button is clicked
        [HttpPost] // Mark as HttpPost because it changes state
        [ValidateAntiForgeryToken]

        public IActionResult Add(string countryName)
        {
            // You'll need to fetch the full Country object based on its name
            // For now, let's just add a placeholder or assume you fetch it from your GetCountries() equivalent
            // In a real app, you'd fetch the Country object from your data source (e.g., DataTransferController's GetCountries())
            // For demonstration, let's create a dummy Country object.
            // IMPORTANT: In a real application, you should get the Country object using a reliable ID, not just the name,
            // and ensure it's a valid country from your data source to prevent malicious input.

            // Get all countries (you might need to inject a service here in a real app)
            var allCountries = new DataTransferController().GetCountries(); // Directly calling is not ideal for larger apps

            Country countryToAdd = allCountries.FirstOrDefault(c => c.Name == countryName);

            if (countryToAdd != null)
            {
                List<Country> favorites = HttpContext.Session.GetObjectFromJson<List<Country>>(FavoritesSessionKey) ?? new List<Country>();

                // Add to favorites if not already present
                if (!favorites.Any(c => c.Name == countryName))
                {
                    favorites.Add(countryToAdd);
                    HttpContext.Session.SetObjectAsJson(FavoritesSessionKey, favorites);
                }
            }

            // Redirect back to the page where the user clicked "Add to Favorites"
            // For now, let's redirect to the DataTransfer/Index page
            return RedirectToAction("Index", "DataTransfer");
        }

        // Action to clear all favorite countries
        [HttpPost] // Mark as HttpPost because it changes state
        [ValidateAntiForgeryToken]

        public IActionResult Clear()
        {
            HttpContext.Session.Remove(FavoritesSessionKey);
            return RedirectToAction("Index", "Favorites"); // Redirect to the empty favorites page
        }
    }

    // --- Extension methods for ISession to store/retrieve objects as JSON ---
    // You can put this in a separate static class file (e.g., SessionExtensions.cs) in a 'Utilities' folder
    // or temporarily at the bottom of this file.
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}