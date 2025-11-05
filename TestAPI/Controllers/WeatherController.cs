using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult WindChill() => View("~/Views/Shared/WindChill.cshtml");
        public IActionResult Hitzeindex() => View("~/Views/Shared/Hitzeindex.cshtml");
        public IActionResult Taupunkt() => View("~/Views/Shared/Taupunkt.cshtml");
        public IActionResult Wolkenhöhe() => View("~/Views/Shared/Wolkenhöhe.cshtml");
    }
}
