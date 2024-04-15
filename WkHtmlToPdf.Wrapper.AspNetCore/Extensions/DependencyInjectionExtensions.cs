using Microsoft.Extensions.Logging;
using WkHtmlToPdf.Wrapper.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWkHtmlToPdfWrapper(this IServiceCollection service)
            => service.AddWkHtmlToPdfWrapper(string.Empty);

        public static IServiceCollection AddWkHtmlToPdfWrapper(this IServiceCollection services, string wkToHtmlExecutablePath)
        {
            services.AddScoped((provider) =>
            {
                var logger = provider.GetService<ILogger<WkHtmlToPdfWrapper>>();
                var wrapper = new WkHtmlToPdfWrapper(logger, wkToHtmlExecutablePath);
                //TODO: Check how to use logger properly
                return wrapper;
            });
            return services;
        }
    }
}
