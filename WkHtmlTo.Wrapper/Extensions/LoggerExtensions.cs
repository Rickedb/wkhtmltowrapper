using Microsoft.Extensions.Logging;
using WkHtmlTo.Wrapper.Logging;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void Log<T>(this ILogger<T> logger, ConversionOutputEvent ev)
        {
            var logLevel = ev.EventType.AsLogLevel();
            logger.Log(logLevel, "[{Type}]: {Message}", ev.EventType, ev.Message);
        }

        public static LogLevel AsLogLevel(this ConversionOutputEventType type)
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
