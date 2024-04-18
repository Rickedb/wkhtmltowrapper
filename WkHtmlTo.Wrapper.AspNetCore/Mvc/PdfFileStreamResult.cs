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
    public class PdfFileStreamResult : FileStreamResult
    {
        public string ViewName { get; set; }
        public object Model => ViewData.Model;
        public ViewDataDictionary ViewData { get; set; }
        public ContentDisposition ContentDisposition { get; set; }
        public IRazorViewOptions Options { get; set; } = new RazorViewPdfOptions();

        public PdfFileStreamResult() : this(null)
        {

        }

        public PdfFileStreamResult(string viewName) : base(Stream.Null, "application/pdf")
        {
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            ViewName = viewName;
        }

        public PdfFileStreamResult(string viewName, object model) : base(Stream.Null, "application/pdf")
        {
            ViewName = viewName;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await Options.RenderViewToHtmlAsync(context, ViewData, ViewName);

            var logger = context.HttpContext.RequestServices.GetService<ILogger<PdfFileStreamResult>>();
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            var onLog = new EventHandler<ConversionOutputEvent>((object sender, ConversionOutputEvent e) => logger.LogDebug(e.Message));

            wrapper.OutputEvent += onLog;
            var result = await wrapper.GenerateAsync(Options);
            wrapper.OutputEvent -= onLog;

            FileStream = result.GetStream();
            context.HttpContext.Response.Prepare(ContentDisposition, FileDownloadName);
            await base.ExecuteResultAsync(context);
        }
    }
}
