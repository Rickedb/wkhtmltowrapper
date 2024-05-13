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
    /// <summary>
    /// Represents an <see cref="ActionResult"/> that renders a pdf view to the response.
    /// </summary>
    public class PdfViewResult : ViewResult
    {
        /// <summary>
        /// Gets or sets the content disposition header
        /// </summary>
        public ContentDisposition ContentDisposition { get; set; }

        /// <summary>
        /// Gets or sets the wkhtmlto rendering options 
        /// </summary>
        public RazorViewPdfOptions Options { get; set; } = new RazorViewPdfOptions();

        /// <summary>
        /// Initializes a new instance of <see cref="PdfFileContentResult"/>
        /// </summary>
        /// <param name="viewData">The <see cref="ViewDataDictionary"/>, usually from <see cref="Controller.ViewData"/></param>
        public PdfViewResult(ViewDataDictionary viewData = null)
        {
            ViewName = string.Empty;
            if (ViewData == null)
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PdfFileContentResult"/>
        /// </summary>
        /// <param name="viewName">The name of the view that should be rendered and used to generate the pdf</param>
        /// <param name="viewData">The <see cref="ViewDataDictionary"/>, usually from <see cref="Controller.ViewData"/></param>
        public PdfViewResult(string viewName, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PdfFileContentResult"/>
        /// </summary>
        /// <param name="model">The model instance associated with the view</param>
        /// <param name="viewData">The <see cref="ViewDataDictionary"/>, usually from <see cref="Controller.ViewData"/></param>
        public PdfViewResult(object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewData.Model = model;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PdfFileContentResult"/>
        /// </summary>
        /// <param name="viewName">The name of the view that should be rendered and used to generate the pdf</param>
        /// <param name="model">The model instance associated with the view</param>
        /// <param name="viewData">The <see cref="ViewDataDictionary"/>, usually from <see cref="Controller.ViewData"/></param>
        public PdfViewResult(string viewName, object model, ViewDataDictionary viewData = null)
            : this(viewData)
        {
            ViewName = viewName;
            ViewData.Model = model;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PdfFileContentResult"/>
        /// </summary>
        /// <param name="viewName">The name of the view that should be rendered and used to generate the pdf</param>
        /// <param name="model">The model instance associated with the view</param>
        public PdfViewResult(string viewName, object model)
            : this(viewName, model, null)
        {

        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            await Options.RenderViewToHtmlAsync(context, ViewData, ViewName);

            var logger = context.HttpContext.RequestServices.GetService<ILogger<PdfViewResult>>();
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            var onLog = new EventHandler<ConversionOutputEventArgs>((object sender, ConversionOutputEventArgs e) => logger?.Log(e));

            wrapper.OutputEvent += onLog;
            var result = await wrapper.ConvertAsync(Options);
            wrapper.OutputEvent -= onLog;
            var response = context.HttpContext.Response.Prepare(ContentDisposition, ViewName);
            await response.Body.WriteAsync(result.GetBytes());
        }
    }
}
