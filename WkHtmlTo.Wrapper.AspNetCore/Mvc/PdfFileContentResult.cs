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
    public class PdfFileContentResult : FileContentResult
    {
        public string ViewName { get; set; }
        public object Model => ViewData.Model;
        public ViewDataDictionary ViewData { get; set; }
        public IRazorViewOptions Options { get; set; } = new RazorViewPdfOptions();
        public ContentDisposition ContentDisposition { get; set; }

        public PdfFileContentResult() : this(default(ViewDataDictionary))
        {
         
        }

        public PdfFileContentResult(object model) : this(null, model)
        {
            
        }

        public PdfFileContentResult(string viewName) : this(viewName, null)
        {
            
        }

        public PdfFileContentResult(string viewName, object model) : this()
        {
            ViewName = viewName;
            ViewData.Model = model;
        }

        public PdfFileContentResult(ViewDataDictionary viewData = null) : base(Array.Empty<byte>(), "application/pdf")
        {
            ViewData = viewData;
            if(viewData == null)
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            }
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            await Options.RenderViewToHtmlAsync(context, ViewData, ViewName);

            var logger = context.HttpContext.RequestServices.GetService<ILogger<PdfViewResult>>();
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            var onLog = new EventHandler<ConversionOutputEvent>((object sender, ConversionOutputEvent e) => logger.LogDebug(e.Message));

            wrapper.OutputEvent += onLog;
            var result = await wrapper.GenerateAsync(Options);
            wrapper.OutputEvent -= onLog;

            FileContents = result.GetBytes();
            context.HttpContext.Response.Prepare(ContentDisposition, FileDownloadName);
            await base.ExecuteResultAsync(context);
        }
    }
}
