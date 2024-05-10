using WkHtmlTo.Wrapper.Logging;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void Log<T>(this ILogger<T> logger, ConversionOutputEventArgs args)
        {
            var ev = args.Event;
            var logLevel = ev.EventType.AsLogLevel();
            logger.Log(logLevel, "[{OccurredAt}][{Type}]: {Message}", args.OccurredAt, ev.EventType, ev.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
