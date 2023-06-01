using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WkHtmlToPdf.Wrapper.RazorPages.Test.Pages
{
    public class ExportModel : PageModel
    {
        public string Title { get; set; } = "Title";
        public void OnGet()
        {

        }
 
    }
}
