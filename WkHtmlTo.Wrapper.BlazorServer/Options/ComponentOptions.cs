using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.IO;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.Options;

namespace WkHtmlTo.Wrapper.BlazorServer.Options
{
    public class ComponentOptions : PdfOptions, IComponentOptions
    {
        public string Html { get; set; }

        public async Task RenderHtmlFromComponentAsync<TComponent>(HtmlRenderer renderer) where TComponent : IComponent
        {
            Html = await renderer.Dispatcher.InvokeAsync<string>(async () =>
            {
                using var output = new StringWriter();
                var htmlComponent = await renderer.RenderComponentAsync<TComponent>();
                htmlComponent.WriteHtmlTo(output);
                return output.ToString();
            });
        }
    }
}
