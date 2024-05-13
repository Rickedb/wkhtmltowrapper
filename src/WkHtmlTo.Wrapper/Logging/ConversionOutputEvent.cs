using System;

namespace WkHtmlTo.Wrapper.Logging
{
    /// <summary>
    /// Represents a stdout output from wkhtmlto executable
    /// </summary>
    public sealed class ConversionOutputEvent
    {
        /// <summary>
        /// The outputted message from stdout
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The type of the outputted message
        /// </summary>
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
