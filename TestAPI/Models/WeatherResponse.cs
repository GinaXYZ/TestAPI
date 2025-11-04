using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Models
{
    public class WeatherResponse : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
