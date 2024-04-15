using System;
using System.Linq;

namespace WkHtmlToPdf.Wrapper.AspNetCore.Logging
{
    public readonly struct ConversionOutputEvent
    {
        public string Message { get; }
        public ConversionOutputEventType EventType { get; }

        private ConversionOutputEvent(string message, ConversionOutputEventType eventType)
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

        internal ProgressStep GetOverallStep()
        {
            if (EventType == ConversionOutputEventType.OverallStep)
            {
                return ProgressStep.ParseOverallProgressStep(Message);
            }
            return default;
        }

        internal int GetOverallProgress()
        {
            if (EventType == ConversionOutputEventType.OverallProgress)
            {
                var percentage = Message.Split(']').LastOrDefault();
                if (percentage != default)
                {
                    int.TryParse(percentage.Replace("%", string.Empty), out var progress);
                    return progress;
                }
            }

            return -1;
        }

        internal Tuple<int, int> GetStepProgress()
        {
            if (EventType == ConversionOutputEventType.InnerStepProgress)
            {
            }

            return Tuple.Create(-1, -1);
        }
    }
}
