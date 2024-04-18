using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
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
    public class RazorViewPdfOptions : PdfOptions, IRazorViewOptions
    {
        public string Html { get; set; }

        public async Task RenderViewToHtmlAsync(ActionContext actionContext, ViewDataDictionary viewData, string viewName)
        {
            Html = await RenderHtmlFromViewAsync(actionContext, viewData, viewName);
        }

        private static async Task<string> RenderHtmlFromViewAsync(ActionContext context, ViewDataDictionary viewData, string viewName)
        {
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = (context.ActionDescriptor as ControllerActionDescriptor).ActionName;
            }

            ViewEngineResult viewEngine = ResolveView(context, viewName);
            var tempDataProvider = context.HttpContext.RequestServices.GetService<ITempDataProvider>();

            using (var output = new StringWriter())
            {
                var view = viewEngine.View;
                var tempDataDictionary = new TempDataDictionary(context.HttpContext, tempDataProvider);
                var viewContext = new ViewContext(
                    context,
                    viewEngine.View,
                    viewData,
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
