using WkHtmlTo.Wrapper.Logging;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Extension helper methods for logging wkhtmlto events
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Logs a <see cref="ConversionOutputEventArgs"/> event to the logger in a default format.
        /// <para>Default format: "<c>[{OccurredAt}][{EventType}]: {Message}</c>"</para>
        /// </summary>
        /// <typeparam name="T">Generic logger type</typeparam>
        /// <param name="logger"></param>
        /// <param name="args"></param>
        public static void Log<T>(this ILogger<T> logger, ConversionOutputEventArgs args)
        {
            var ev = args.Event;
            var logLevel = ev.EventType.AsLogLevel();
            logger.Log(logLevel, "[{OccurredAt}][{EventType}]: {Message}", args.OccurredAt, ev.EventType, ev.Message);
        }

        /// <summary>
        /// Correlates and convert the <see cref="ConversionOutputEventType"/> to <see cref="LogLevel"/>
        /// </summary>
        /// <param name="type">The <see cref="ConversionOutputEventType"/> argument</param>
        public static LogLevel AsLogLevel(this ConversionOutputEventType type)
        {
            switch(type)
            {
                case ConversionOutputEventType.Information: 
                case ConversionOutputEventType.OverallStep: 
                case ConversionOutputEventType.OverallProgress: 
                    return LogLevel.Information;
                case ConversionOutputEventType.InnerStep: 
                case ConversionOutputEventType.InnerStepProgress: 
                    return LogLevel.Debug;
                case ConversionOutputEventType.Error: 
                    return LogLevel.Error;
                default: 
                    return LogLevel.None;
            };
        }
    }
}
