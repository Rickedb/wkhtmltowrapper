using WkHtmlTo.Wrapper.Logging;
using WkHtmlTo.Wrapper.Options;

namespace Microsoft.Extensions.Logging
{
    internal static class LoggingExtensions
    {
        internal static LogLevel ToLogLevel(this PromptLogLevel promptLogLevel)
        {
            return promptLogLevel switch
            {
                PromptLogLevel.Error => LogLevel.Error,
                PromptLogLevel.Warn => LogLevel.Warning,
                PromptLogLevel.Info => LogLevel.Information,
                _ => LogLevel.None,
            };
        }

        internal static LogLevel ToLogLevel(this ConversionOutputEventType type)
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