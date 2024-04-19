using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.AspNetCore.Extensions;
using WkHtmlTo.Wrapper.AspNetCore.Options;
using WkHtmlTo.Wrapper.Logging;

namespace WkHtmlTo.Wrapper.AspNetCore.Mvc
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
            await Options.RenderViewToHtmlAsync(context, ViewData, ViewName);

            var logger = context.HttpContext.RequestServices.GetService<ILogger<PdfViewResult>>();
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            var onLog = new EventHandler<ConversionOutputEvent>((object sender, ConversionOutputEvent e) => logger?.Log(e));

            wrapper.OutputEvent += onLog;
            var result = await wrapper.ConvertAsync(Options);
            wrapper.OutputEvent -= onLog;
            var response = context.HttpContext.Response.Prepare(ContentDisposition, ViewName);
            await response.Body.WriteAsync(result.GetBytes());
        }
    }
}
