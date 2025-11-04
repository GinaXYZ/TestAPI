using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult WindChill() => View("~/Views/Shared/WindChill.cshtml");
    }
}
