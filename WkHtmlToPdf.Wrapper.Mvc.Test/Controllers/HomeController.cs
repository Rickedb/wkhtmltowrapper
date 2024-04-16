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

        public IActionResult ViewPdf()
        {
            var result = new PdfViewResult("Index", new HomeViewModel());
            return result;
        }

        public IActionResult Download()
        {
            var result = new PdfFileStreamResult("Privacy");
            return result;
        }

        public IActionResult DownloadBytes()
        {
            var result = new PdfFileContentResult()
            {
                ViewName = "Privacy"
            };
            return result;
        }
    }
}
