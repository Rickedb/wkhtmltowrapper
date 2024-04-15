using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
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

namespace WkHtmlToPdf.Wrapper.AspNetCore.Options
{
    public class RazorPdfViewOptions : PdfOptions
    {
        public ContentDisposition ContentDisposition { get; set; }
        public string Filename { get; set; }

        internal async Task<HtmlOptions> ToHtmlOptionsAsync<TViewResult>(TViewResult viewResult, ActionContext context) where TViewResult : ViewResult
        {
            var html = await GenerateHtmlFromViewAsync(viewResult, context);
            return new HtmlOptions(html)
            {
                LogLevel = LogLevel
            }; //TODO
        }

        internal async Task<HtmlOptions> ToHtmlOptionsAsync(FileStreamResult viewResult, ActionContext context)
        {
            var html = await GenerateHtmlFromViewAsync(viewResult, context);
            return new HtmlOptions(html)
            {
                LogLevel = LogLevel
            }; //TODO
        }

        private static async Task<string> GenerateHtmlFromViewAsync(FileStreamResult viewResult, ActionContext context) where TViewResult : ViewResult
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
    }
}
