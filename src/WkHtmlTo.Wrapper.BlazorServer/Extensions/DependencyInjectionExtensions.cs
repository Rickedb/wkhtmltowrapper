using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using WkHtmlTo.Wrapper;
using WkHtmlTo.Wrapper.BlazorServer;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to help wrapper dependency injection for blazor server
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds WkHtmlTo wrapper into service collection
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance</param>
        public static IServiceCollection AddWkHtmlToPdfWrapper(this IServiceCollection service)
            => service.AddWkHtmlToPdfWrapper(string.Empty);

        /// <summary>
        /// Adds WkHtmlTo wrapper into service collection specifying the wkhtmlto executable path
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance</param>
        /// <param name="wkHtmlToExecutablePath">Folder path where your whkhtmltopdf executable is located</param>
        public static IServiceCollection AddWkHtmlToPdfWrapper(this IServiceCollection services, string wkHtmlToExecutablePath)
        {
            services.AddScoped(_ => new WkHtmlToPdfWrapper(wkHtmlToExecutablePath))
                    .AddScoped(provider =>
                    {
                        var loggerFactory = provider.GetService<ILoggerFactory>();
                        var logger = loggerFactory.CreateLogger<PdfComponentRenderer>();
                        var jsRuntime = provider.GetService<IJSRuntime>();
                        var htmlRenderer = new HtmlRenderer(provider, loggerFactory);
                        var wrapper = provider.GetService<WkHtmlToPdfWrapper>();
                        return new PdfComponentRenderer(logger, jsRuntime, htmlRenderer, wrapper);
                    });
            return services;
        }
    }
}
