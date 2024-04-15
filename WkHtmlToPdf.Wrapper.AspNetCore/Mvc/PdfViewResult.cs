using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Mvc
{
    public class PdfViewResult : ViewResult
    {
        public RazorPdfViewOptions Options { get; set; } = new RazorPdfViewOptions();

        public PdfViewResult(ViewDataDictionary viewData = null)
        {
            ViewName = string.Empty;
            if (ViewData == null)
            {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            }
            //ViewData = viwData;
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
            var fileContent = await BuildFileBytesAsync(context);
            var response = PrepareResponse(context.HttpContext.Response);
            await response.Body.WriteAsync(fileContent);
        }

        public async Task<byte[]> BuildFileBytesAsync(ActionContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var result = await GeneratePdfAsync(context);
            if (!result.Success)
            {
                throw new InvalidOperationException();
            }
            return result.GetBytes();
        }

        public async Task<ConversionResult> SaveAsync(ActionContext context, string folderPath)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            //ArgumentException.ThrowIfNullOrWhiteSpace(folderPath, nameof(folderPath));

            var options = await Options.ToHtmlOptionsAsync(this, context);
            var wrapper = context.HttpContext.RequestServices.GetService<WkHtmlToPdfWrapper>();
            return await wrapper.GenerateAsync(options);
        }

        public async Task<Stream> BuildFileStreamAsync(ActionContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var fileContent = await GeneratePdfAsync(context);
            return fileContent.GetStream();
        }

        private HttpResponse PrepareResponse(HttpResponse response)
        {
            response.ContentType = "application/pdf";
            var filename = Options.Filename;
            if (!string.IsNullOrEmpty(filename))
            {
                var contentDisposition = Options.ContentDisposition == ContentDisposition.Attachment ? "attachment" : "inline";
                filename = SanitizeFileName(filename);
                response.Headers.Append("Content-Disposition", string.Format("{0}; filename=\"{1}\"", contentDisposition, filename));
            }

            return response;
        }

        protected async Task<ConversionResult> GeneratePdfAsync(ActionContext context)
        {
            var options = await Options.ToHtmlOptionsAsync(this, context);
            var wrapper = context.HttpContext.RequestServices.GetService(typeof(WkHtmlToPdfWrapper)) as WkHtmlToPdfWrapper;
            return await wrapper.GenerateAsync(options);
        }

        private static async Task<string> GenerateHtmlFromViewAsync<TViewResult>(TViewResult viewResult, ActionContext context) where TViewResult : ViewResult
        {
            string viewName = viewResult.ViewName;
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = context.ActionDescriptor switch
                {
                    CompiledPageActionDescriptor compiledPageActionDescriptor => compiledPageActionDescriptor.RelativePath,
                    ControllerActionDescriptor controllerActionDescriptor => controllerActionDescriptor.ActionName,
                    _ => throw new ArgumentNullException(viewName, nameof(viewName))
                };
            }

            ViewEngineResult viewEngine = ResolveView(context, viewName);
            var tempDataProvider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();

            //var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            //{
            //    Model = viewResult.ViewData.Model
            //};

            //if (viewResult.ViewData != null)
            //{
            //    foreach (var item in viewResult.ViewData)
            //    {
            //        viewDataDictionary.Add(item);
            //    }
            //}

            using (var output = new StringWriter())
            {
                var view = viewEngine.View;
                var tempDataDictionary = new TempDataDictionary(context.HttpContext, tempDataProvider);
                var viewContext = new ViewContext(
                    context,
                    viewEngine.View,
                    viewResult.ViewData,
                    tempDataDictionary,
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                string baseUrl = string.Format("{0}://{1}", context.HttpContext.Request.Scheme, context.HttpContext.Request.Host);

                var html = output.GetStringBuilder().ToString();
                html = Regex.Replace(html, "<head>", string.Format("<head><base href=\"{0}\" />", baseUrl), RegexOptions.IgnoreCase);
                return html;
            }
        }

        private static ViewEngineResult ResolveView(ActionContext context, string viewName)
        {
            var engine = context.HttpContext.RequestServices.GetService<ICompositeViewEngine>();
            var getViewResult = engine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: true);
            if (getViewResult.Success)
            {
                return getViewResult;
            }

            var findViewResult = engine.FindView(context, viewName, isMainPage: true);
            if (findViewResult.Success)
            {
                return findViewResult;
            }

            var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

            throw new InvalidOperationException(errorMessage);
        }

        private static string SanitizeFileName(string name)
        {
            var invalidChars = string.Concat(new string(Path.GetInvalidPathChars()), new string(Path.GetInvalidFileNameChars()));
            var invalidCharsPattern = string.Format(@"[{0}]+", Regex.Escape(invalidChars));
            var result = Regex.Replace(name, invalidCharsPattern, "_");
            return result;
        }
    }
}
