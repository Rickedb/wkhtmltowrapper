using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Blazor
{
    internal class PdfBlazor
    {
        public ContentDisposition ContentDisposition { get; set; }
        public IRazorViewOptions Options { get; } = new RazorViewPdfOptions();

        public PdfBlazor()
        {
        }

        public async Task GenerateAsync<TComponent>(HttpContext context)
        {
            var htmlHelper = context.RequestServices.GetService<IHtmlHelper>();
            
            var component = await htmlHelper.RenderComponentAsync(typeof(TComponent), RenderMode.Server, null);
            component.WriteTo(null, HtmlEncoder.Default);

        }
    }
}
