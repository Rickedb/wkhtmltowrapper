using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Collections.Generic;
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

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>() where TComponent : IComponent
        {
            var options = new ComponentOptions();
            return GenerateAndDownloadAsync<TComponent>(options);

        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="parameters">Dictionary of parameters for the component.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(Dictionary<string, object> parameters) where TComponent : IComponent
        {
            var options = new ComponentOptions();
            return GenerateAndDownloadAsync<TComponent>(options, parameters);
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="parameterView">Parameters for the component.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(ParameterView parameterView) where TComponent : IComponent
        {
            var options = new ComponentOptions();
            return GenerateAndDownloadAsync<TComponent>(options, parameterView);
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="filename">The output pdf filename.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(string filename) where TComponent : IComponent
        {
            var options = new ComponentOptions(filename);
            return GenerateAndDownloadAsync<TComponent>(options);
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="filename">The output pdf filename.</param>
        /// <param name="parameters">Dictionary of parameters for the component.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(string filename, Dictionary<string, object> parameters) where TComponent : IComponent
        {
            var options = new ComponentOptions(filename);
            return GenerateAndDownloadAsync<TComponent>(options, parameters);
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="filename">The output pdf filename.</param>
        /// <param name="parameterView">Parameters for the component.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(string filename, ParameterView parameterView) where TComponent : IComponent
        {
            var options = new ComponentOptions(filename);
            return GenerateAndDownloadAsync<TComponent>(options, parameterView);
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="options">The component pdf conversion options</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(ComponentOptions options) where TComponent : IComponent
        {
            return GenerateAndDownloadAsync<TComponent>(options, ParameterView.Empty);
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="options">The component pdf conversion options</param>
        /// <param name="parameters">Dictionary of parameters for the component.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public Task GenerateAndDownloadAsync<TComponent>(ComponentOptions options, Dictionary<string, object> parameters) where TComponent : IComponent
        {
            return GenerateAndDownloadAsync<TComponent>(options, ParameterView.FromDictionary(parameters));
        }

        /// <summary>
        /// Generates the pdf from the component rendered html and triggers the download at the client side
        /// </summary>
        /// <typeparam name="TComponent">The component type.</typeparam>
        /// <param name="options">The component pdf conversion options</param>
        /// <param name="parameterView">Parameters for the component.</param>
        /// <returns>
        /// A task that completes with the rendering, conversion and downloading the pdf from <typeparamref name="TComponent"/>
        /// </returns>
        public async Task GenerateAndDownloadAsync<TComponent>(ComponentOptions options, ParameterView parameterView) where TComponent : IComponent
        {
            await options.RenderHtmlFromComponentAsync<TComponent>(_htmlRenderer, parameterView);

            _wrapper.OutputEvent += OnLog;
            var result = await _wrapper.ConvertAsync(options);
            _wrapper.OutputEvent -= OnLog;
            var fileStream = result.GetStream();

            var filename = options.Filename;
            filename = !string.IsNullOrWhiteSpace(filename) ? SanitizeFileName(filename) : typeof(TComponent).Name;

            using var streamReference = new DotNetStreamReference(fileStream);
            await _jsRuntime.InvokeVoidAsync("downloadFileFromStream", filename, streamReference);
        }

        private void OnLog(object sender, ConversionOutputEventArgs e)
            => _logger?.Log(e);

        private static string SanitizeFileName(string name)
        {
            var invalidChars = string.Concat(new string(Path.GetInvalidPathChars()), new string(Path.GetInvalidFileNameChars()));
            var invalidCharsPattern = string.Format(@"[{0}]+", Regex.Escape(invalidChars));
            var result = Regex.Replace(name, invalidCharsPattern, "_");
            return result.EndsWith(".pdf") ? result : $"{result}.pdf";
        }
    }
}
