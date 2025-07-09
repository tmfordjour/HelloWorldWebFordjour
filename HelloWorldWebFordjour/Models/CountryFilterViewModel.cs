// Models/CountryFilterViewModel.cs
namespace HelloWorldWebFordjour.Models
{
    public class CountryFilterViewModel
    {
        public string GameFilter { get; set; } = "ALL"; // Default to "ALL"
        public string CategoryFilter { get; set; } = "ALL"; // Default to "ALL"
        // You could also include the list of countries here if passing back and forth
        // public List<Country> Countries { get; set; }
    }
}