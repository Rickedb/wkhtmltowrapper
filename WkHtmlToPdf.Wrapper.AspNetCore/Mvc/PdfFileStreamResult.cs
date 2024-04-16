using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.AspNetCore.Extensions;
using WkHtmlToPdf.Wrapper.AspNetCore.Mvc.Rendering;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Mvc
{
    public class PdfFileStreamResult : FileStreamResult
    {
        public string ViewName { get; set; }
        public object Model => ViewData.Model;
        public ViewDataDictionary ViewData { get; set; }
        public ContentDisposition ContentDisposition { get; set; }
        public IRazorOptions Options { get; set; } = new RazorPdfOptions();

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
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            var result = await wrapper.GenerateAsync(Options);
            FileStream = result.GetStream();
            context.HttpContext.Response.Prepare(ContentDisposition, FileDownloadName);
            await base.ExecuteResultAsync(context);
        }
    }
}
