using Microsoft.Extensions.Logging;
using WkHtmlTo.Wrapper;
using WkHtmlTo.Wrapper.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWkHtmlToWrapper(this IServiceCollection service)
            => service.AddWkHtmlToWrapper(string.Empty);

        public static IServiceCollection AddWkHtmlToWrapper(this IServiceCollection services, string wkToHtmlExecutablePath)
        {
            services.AddScoped((provider) =>
            {
                var wrapper = new WkHtmlToPdfWrapper(wkToHtmlExecutablePath);
                //TODO: Check how to use logger properly
                return wrapper;
            });
            return services;
        }
    }
}
