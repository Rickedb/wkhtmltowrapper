using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.BlazorServer.Options;
using WkHtmlTo.Wrapper.Logging;

namespace WkHtmlTo.Wrapper.BlazorServer
{
    public class PdfComponentRenderer(ILogger<PdfComponentRenderer> logger, IJSRuntime jsRuntime, HtmlRenderer htmlRenderer, WkHtmlToPdfWrapper wrapper)
    {
        public async Task GenerateAsync<TComponent>() where TComponent : IComponent
        {
            var options = new ComponentOptions();
            await options.RenderHtmlFromComponentAsync<TComponent>(htmlRenderer);

            wrapper.OutputEvent += OnLog;
            var result = await wrapper.GenerateAsync(options);
            wrapper.OutputEvent -= OnLog;
            var fileStream = result.GetStream();
            var fileName = $"{nameof(TComponent)}.pdf";

            using var streamReference = new DotNetStreamReference(stream: fileStream);
            await jsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamReference);
        }

        private void OnLog(object sender, ConversionOutputEvent e)
        {
            logger?.LogInformation(e.Message);
        }
    }
}
