using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.AspNetCore.Extensions;
using WkHtmlTo.Wrapper.AspNetCore.Options;
using WkHtmlTo.Wrapper.Logging;

namespace WkHtmlTo.Wrapper.AspNetCore.Mvc
{
    public class PdfFileContentResult : FileContentResult
    {
        public string ViewName { get; set; }
        public object Model => ViewData.Model;
        public ViewDataDictionary ViewData { get; set; }
        public ContentDisposition ContentDisposition { get; set; }
        public RazorViewPdfOptions Options { get; set; } = new RazorViewPdfOptions();

        public PdfFileContentResult(ViewDataDictionary viewData = null) : base(Array.Empty<byte>(), "application/pdf")
        {
            ViewName = string.Empty;
            if (ViewData == null)
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            }
        }

        public PdfFileContentResult(string viewName, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
        }

        public PdfFileContentResult(object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewData.Model = model;
        }

        public PdfFileContentResult(string viewName, object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
            ViewData.Model = model;
        }

        public PdfFileContentResult(string viewName, object model)
            : this(viewName, model, null)
        {

        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await Options.RenderViewToHtmlAsync(context, ViewData, ViewName);

            var logger = context.HttpContext.RequestServices.GetService<ILogger<PdfViewResult>>();
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            var onLog = new EventHandler<ConversionOutputEvent>((object sender, ConversionOutputEvent e) => logger.LogDebug(e.Message));

            wrapper.OutputEvent += onLog;
            var result = await wrapper.ConvertAsync(Options);
            wrapper.OutputEvent -= onLog;

            FileContents = result.GetBytes();
            context.HttpContext.Response.Prepare(ContentDisposition, FileDownloadName);
            await base.ExecuteResultAsync(context);
        }
    }
}
