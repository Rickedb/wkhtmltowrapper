using System;

namespace WkHtmlTo.Wrapper.Logging
{
    /// <summary>
    /// Wkhtmlto executable stdout event
    /// </summary>
    public class ConversionOutputEventArgs : EventArgs
    {
        /// <summary>
        /// The timestamp when the stdout event was captured
        /// </summary>
        public DateTime OccurredAt { get; }

        /// <summary>
        /// The captured stdout parsed event
        /// </summary>
        public ConversionOutputEvent Event { get; }

        internal ConversionOutputEventArgs(ConversionOutputEvent ev)
        {
            OccurredAt = DateTime.Now;
            Event = ev;
        }
    }
}
