using WkHtmlToPdf.Wrapper.AspNetCore.Logging;
using WkHtmlToPdf.Wrapper.AspNetCore.Options;

namespace Microsoft.Extensions.Logging
{
    internal static class LoggingExtensions
    {
        public static LogLevel ToLogLevel(this PromptLogLevel promptLogLevel)
        {
            return promptLogLevel switch
            {
                PromptLogLevel.Error => LogLevel.Error,
                PromptLogLevel.Warning => LogLevel.Warning,
                PromptLogLevel.Info => LogLevel.Information,
                _ => LogLevel.None,
            };
        }

        public static LogLevel ToLogLevel(this ConversionOutputEventType type)
        {
            return type switch
            {
                ConversionOutputEventType.Information => LogLevel.Information,
                ConversionOutputEventType.Error => LogLevel.Information,
                ConversionOutputEventType.OverallProgress => LogLevel.Information,
                ConversionOutputEventType.OverallStep => LogLevel.Information,
                ConversionOutputEventType.InnerStep => LogLevel.Debug,
                ConversionOutputEventType.InnerStepProgress => LogLevel.Debug,
                _ => LogLevel.None
            };
        }
    }
}