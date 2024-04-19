using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.BlazorServer.Options;
using WkHtmlTo.Wrapper.Logging;

namespace WkHtmlTo.Wrapper.BlazorServer
{
    public class PdfComponentRenderer
    {
        private readonly ILogger<PdfComponentRenderer> _logger;
        private readonly IJSRuntime _jsRuntime;
        private readonly HtmlRenderer _htmlRenderer;
        private readonly WkHtmlToPdfWrapper _wrapper;

        public PdfComponentRenderer(ILogger<PdfComponentRenderer> logger, IJSRuntime jsRuntime, HtmlRenderer htmlRenderer, WkHtmlToPdfWrapper wrapper)
        {
            _logger = logger;
            _jsRuntime = jsRuntime;
            _htmlRenderer = htmlRenderer;
            _wrapper = wrapper;
        }

        public Task GenerateAndDownloadAsync<TComponent>() where TComponent : IComponent
        {
            var options = new ComponentOptions();
            return GenerateAndDownloadAsync<TComponent>(options);
        }

        public Task GenerateAndDownloadAsync<TComponent>(string filename) where TComponent : IComponent
        {
            var options = new ComponentOptions(filename);
            return GenerateAndDownloadAsync<TComponent>(options);
        }

        public async Task GenerateAndDownloadAsync<TComponent>(IComponentOptions options) where TComponent : IComponent
        {
            await options.RenderHtmlFromComponentAsync<TComponent>(_htmlRenderer);

            _wrapper.OutputEvent += OnLog;
            var result = await _wrapper.ConvertAsync(options);
            _wrapper.OutputEvent -= OnLog;
            var fileStream = result.GetStream();

            var filename = options.Filename;
            filename = !string.IsNullOrWhiteSpace(filename) ? SanitizeFileName(filename) : typeof(TComponent).Name;

            using var streamReference = new DotNetStreamReference(stream: fileStream);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", $"{filename}.pdf", streamReference);
        }

        private void OnLog(object sender, ConversionOutputEvent e)
            => _logger?.Log(e);

        private static string SanitizeFileName(string name)
        {
            var invalidChars = string.Concat(new string(Path.GetInvalidPathChars()), new string(Path.GetInvalidFileNameChars()));
            var invalidCharsPattern = string.Format(@"[{0}]+", Regex.Escape(invalidChars));
            var result = Regex.Replace(name, invalidCharsPattern, "_");
            return result;
        }
    }
}
