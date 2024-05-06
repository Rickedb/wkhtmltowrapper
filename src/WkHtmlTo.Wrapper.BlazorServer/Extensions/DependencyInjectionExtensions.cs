using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using System;
using WkHtmlTo.Wrapper;
using WkHtmlTo.Wrapper.BlazorServer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWkHtmlToPdfWrapper(this IServiceCollection service)
            => service.AddWkHtmlToPdfWrapper(string.Empty);

        public static IServiceCollection AddWkHtmlToPdfWrapper(this IServiceCollection services, string wkToHtmlExecutablePath)
        {
            services.AddScoped(_ => new WkHtmlToPdfWrapper(wkToHtmlExecutablePath))
                    .AddScoped(provider =>
                    {
                        var loggerFactory = provider.GetService<ILoggerFactory>();
                        return new HtmlRenderer(provider, loggerFactory);
                    })
                    .AddScoped<PdfComponentRenderer>();
            return services;
        }
    }
}
