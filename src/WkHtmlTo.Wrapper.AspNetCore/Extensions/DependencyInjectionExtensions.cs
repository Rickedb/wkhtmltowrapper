using WkHtmlTo.Wrapper;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to help wrapper dependency injection
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds WkHtmlTo wrapper into service collection
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance</param>
        public static IServiceCollection AddWkHtmlToWrapper(this IServiceCollection services)
            => services.AddWkHtmlToWrapper(string.Empty);

        /// <summary>
        /// Adds WkHtmlTo wrapper into service collection specifying the wkhtmlto executable path
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance</param>
        /// <param name="wkHtmlToExecutablePath">Folder path where your whkhtmltopdf executable is located</param>
        public static IServiceCollection AddWkHtmlToWrapper(this IServiceCollection services, string wkHtmlToExecutablePath)
            => services.AddScoped((provider) => new WkHtmlToPdfWrapper(wkHtmlToExecutablePath));
    }
}
