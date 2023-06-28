using System;
using System.Linq;

namespace WkHtmlToPdf.Wrapper
{
    public readonly struct OutputEvent
    {
        public string Message { get; }
        public OutputEventType EventType { get; }

        private OutputEvent(string message, OutputEventType eventType)
        {
            Message = message;
            EventType = eventType;
        }

        internal static OutputEvent Parse(string message)
        {
            message = message.Trim();
            if (message.StartsWith("Error", StringComparison.OrdinalIgnoreCase))
            {
                return new OutputEvent(message, OutputEventType.Error);
            }

            if (message.StartsWith("["))
            {
                var type = message.EndsWith("%") ? OutputEventType.OverallProgress : OutputEventType.InnerStepProgress;
                return new OutputEvent(message, type);
            }

            if (message.EndsWith(")"))
            {
                return new OutputEvent(message, OutputEventType.OverallStep);
            }

            return new OutputEvent(message, OutputEventType.Information);
        }

        internal ProgressStep GetOverallStep()
        {
            if (EventType == OutputEventType.OverallStep)
            {
                return ProgressStep.ParseOverallProgressStep(Message);
            }
            return default;
        }

        internal int GetOverallProgress()
        {
            if(EventType == OutputEventType.OverallProgress)
            {
                var percentage = Message.Split(']').LastOrDefault();
                if(percentage != default)
                {
                    int.TryParse(percentage.Replace("%", string.Empty), out var progress);
                    return progress;
                }
            }

            return -1;
        }

        internal Tuple<int, int> GetStepProgress()
        {
            if (EventType == OutputEventType.InnerStepProgress)
            {
            }

            return Tuple.Create(-1, -1);
        }
    }
}
