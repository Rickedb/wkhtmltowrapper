using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mime;
using WkHtmlToPdf.Wrapper.AspNetCore.Mvc;

namespace WkHtmlToPdf.Wrapper.RazorPages.Test.Pages
{
    public class ExportModel : PageModel
    {
        public string Title { get; set; } = "Title";
        public IActionResult OnGet()
        {
            return Page();
        }

        public override PageResult Page()
        {
            //PageContext.HttpContext.Response.ContentType = "application/pdf";
            return new PdfPageResult();
        }
    }
}
