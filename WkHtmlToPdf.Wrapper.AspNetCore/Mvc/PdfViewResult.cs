using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Mvc
{
    public class PdfViewResult : ViewResult
    {
        public ContentDisposition ContentDisposition { get; set; }
        public IRazorViewOptions Options { get; } = new RazorViewPdfOptions();

        public PdfViewResult(ViewDataDictionary viewData = null)
        {
            ViewName = string.Empty;
            if (ViewData == null)
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            }
        }

        public PdfViewResult(string viewName, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
        }

        public PdfViewResult(object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewData.Model = model;
        }

        public PdfViewResult(string viewName, object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
            ViewData.Model = model;
        }

        public PdfViewResult(string viewName, object model)
            : this(viewName, model, null)
        {

        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            await Options.RenderViewToHtmlAsync(context, ViewData, ViewName);
            await wrapper.GenerateAsync(Options);
        }
    }
}
