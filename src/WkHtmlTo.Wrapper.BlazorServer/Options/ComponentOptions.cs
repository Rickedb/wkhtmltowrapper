using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.IO;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.Options;

namespace WkHtmlTo.Wrapper.BlazorServer.Options
{
    /// <summary>
    /// The conversion options to convert a blazor component to pdf in wkhtmltopdf
    /// </summary>
    public class ComponentOptions : PdfOptions, IHtmlOptions
    {
        string IHtmlOptions.Html { get; set; }

        /// <summary>
        /// The output filename of the pdf
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ComponentOptions"/>.
        /// </summary>
        public ComponentOptions()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ComponentOptions"/>.
        /// </summary>
        /// <param name="filename">The output filename of the pdf</param>
        public ComponentOptions(string filename)
        {
            Filename = filename;
        }

        internal async Task RenderHtmlFromComponentAsync<TComponent>(HtmlRenderer renderer, ParameterView parameterView) where TComponent : IComponent
        {
            (this as IHtmlOptions).Html = await renderer.Dispatcher.InvokeAsync(async () =>
            {
                using var output = new StringWriter();
                var htmlComponent = await renderer.RenderComponentAsync<TComponent>(parameterView);
                htmlComponent.WriteHtmlTo(output);
                return output.ToString();
            });
        }
    }
}
