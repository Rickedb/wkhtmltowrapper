using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WkHtmlToPdf.Wrapper.AspNetCore;
using WkHtmlToPdf.Wrapper.AspNetCore.Mvc;

namespace WkHtmlToPdf.Wrapper.BlazorServer.Tests.Pages
{
    public class ExportModel : PageModel
    {
        public string MyString { get; set; } = "String";

        public ActionResult OnGet()
        {
            return new PdfViewResult(this);
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
