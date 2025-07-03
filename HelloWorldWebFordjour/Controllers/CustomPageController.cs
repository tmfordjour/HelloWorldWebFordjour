using Microsoft.AspNetCore.Mvc;

namespace HelloWorldWebFordjour.Controllers
{
    public class CustomPageController : Controller
    {
        // 3. Custom Routing Attribute Example
        // This action will only be accessible via the specified route attribute.
        [Route("dummy-pages/custom-attribute-page")]
        public IActionResult AttributeRoutingPage()
        {
            ViewBag.Title = "Custom Attribute Routing Page";
            return View("~/Views/Custom/AttributeRoutingPage.cshtml"); // Specify full path since it's not in CustomPage view folder
        }
    }
}
