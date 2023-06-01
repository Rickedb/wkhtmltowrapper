using System;

namespace WkHtmlTo.Wrapper.Logging
{
    public class ConversionOutputEvent
    {
        public string Message { get; }
        public ConversionOutputEventType EventType { get; }

        internal ConversionOutputEvent(string message, ConversionOutputEventType eventType)
        {
            Message = message;
            EventType = eventType;
        }

        internal static ConversionOutputEvent Parse(string message)
        {
            message = message.Trim();
            if (message.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
            {
                return new ConversionOutputEvent(message, ConversionOutputEventType.Error);
            }

            if (message.StartsWith("["))
            {
                var type = message.EndsWith("%") ? ConversionOutputEventType.OverallProgress : ConversionOutputEventType.InnerStepProgress;
                return new ConversionOutputEvent(message, type);
            }

            if (message.EndsWith(")"))
            {
                return new ConversionOutputEvent(message, ConversionOutputEventType.OverallStep);
            }

            return new ConversionOutputEvent(message, ConversionOutputEventType.Information);
        }
    }
}
