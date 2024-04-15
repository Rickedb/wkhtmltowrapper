using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WkHtmlToPdf.Wrapper.AspNetCore.Mvc;
using WkHtmlToPdf.Wrapper.Mvc.Test.Models;

namespace WkHtmlToPdf.Wrapper.Mvc.Test.Controllers
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
            return View(new HomeViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Export()
        {
            var result = new PdfViewResult("Index", new HomeViewModel());
            return result;
        }
    }
}
