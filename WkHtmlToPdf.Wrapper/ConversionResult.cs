using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace WkHtmlToPdf.Wrapper
{
    public abstract class ConversionResult
    {
        private readonly List<OutputEvent> _events;
        private readonly Stopwatch _stopwatch;


        public IEnumerable<OutputEvent> Events => _events;
        public IEnumerable<string> Logs => _events.Select(x => x.Message);
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        public bool HasErrors => _events.Any(x => x.EventType == OutputEventType.Error);
        public abstract long TotalBytes { get; }
        public abstract bool Success { get; }

        public abstract byte[] GetBytes();
        public abstract Stream GetStream();


        internal ConversionResult()
        {
            _stopwatch = Stopwatch.StartNew();
            _events = new List<OutputEvent>();
        }

        internal void AddEvent(OutputEvent ev)
            => _events.Add(ev);
    }
}
