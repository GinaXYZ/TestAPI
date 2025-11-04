using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction(nameof(WindChill));
        }

        public IActionResult WindChill()
        {
            return View();
        }
    }
}
