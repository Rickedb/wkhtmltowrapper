using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WkHtmlTo.Wrapper.AspNetCore.Mvc;
using WkHtmlTo.Wrapper.Samples.Mvc.Models;

namespace WkHtmlTo.Wrapper.Samples.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WeatherForecasts()
        {
            return View(WeatherForecast.Forecasts);
        }

        public IActionResult DownloadWeatherForecasts()
        {
            var result = new PdfFileStreamResult("WeatherForecasts", WeatherForecast.Forecasts, ViewData);
            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
