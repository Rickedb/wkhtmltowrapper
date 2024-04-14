using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WkHtmlToPdf.Wrapper.AspNetCore;

namespace WkHtmlToPdf.Wrapper.BlazorServer.Tests.Pages
{
    public class ExportModel : PageModel
    {
        public ActionResult OnGet()
        {
            return new PdfViewResult();
        }

        public override PageResult Page()
        {
            return base.Page();
        }
        public override ViewComponentResult ViewComponent(string componentName)
        {
            return base.ViewComponent(componentName);
        }
    }
}
