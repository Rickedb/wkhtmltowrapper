using System;

namespace WkHtmlTo.Wrapper.Logging
{
    public readonly struct ConversionOutputEventArgs
    {
        public DateTime OccurredAt { get; }
        public ConversionOutputEvent Event { get; }

        internal ConversionOutputEventArgs(ConversionOutputEvent ev)
        {
            OccurredAt = DateTime.Now;
            Event = ev;
        }
    }
}
