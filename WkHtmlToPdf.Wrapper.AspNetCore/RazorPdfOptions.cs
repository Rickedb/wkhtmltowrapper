using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore
{
    public class RazorPdfOptions : PdfOptions
    {
        public ContentDisposition ContentDisposition { get; set; }
        public string Filename { get; set; }

        internal async Task<HtmlOptions> ToHtmlOptionsAsync(ViewResult viewResult, ActionContext context)
        {
            var html = await GenerateHtmlFromViewAsync(viewResult, context);
            return new HtmlOptions(html)
            {
                LogLevel = LogLevel
            }; //TODO
        }

        private static async Task<string> GenerateHtmlFromViewAsync(ViewResult viewResult, ActionContext context)
        {
            string viewName = viewResult.ViewName;
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = context.ActionDescriptor switch
                {
                    CompiledPageActionDescriptor compiledPageActionDescriptor => compiledPageActionDescriptor.RelativePath,
                    ControllerActionDescriptor controllerActionDescriptor => controllerActionDescriptor.ActionName,
                    _ => throw new ArgumentNullException(viewName, nameof(viewName))
                };
            }

            ViewEngineResult viewEngine = ResolveView(context, viewName);
            ITempDataProvider tempDataProvider = context.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            var viewDataDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = viewResult.Model
            };

            if (viewResult.ViewData != null)
            {
                foreach (var item in viewResult.ViewData)
                {
                    viewDataDictionary.Add(item);
                }
            }

            using (var output = new StringWriter())
            {
                var view = viewEngine.View;
                var tempDataDictionary = new TempDataDictionary(context.HttpContext, tempDataProvider);
                var viewContext = new ViewContext(
                    context,
                    viewEngine.View,
                    viewDataDictionary,
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
            var engine = context.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

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
    }
}
