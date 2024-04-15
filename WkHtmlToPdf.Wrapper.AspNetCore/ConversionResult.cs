using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WkHtmlToPdf.Wrapper.AspNetCore.Logging;

namespace WkHtmlToPdf.Wrapper.AspNetCore
{
    public abstract class ConversionResult
    {
        private readonly List<ConversionOutputEvent> _events;
        private readonly Stopwatch _stopwatch;

        public IEnumerable<ConversionOutputEvent> Events => _events;
        public IEnumerable<string> Logs => _events.Select(x => x.Message);
        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;
        public bool HasErrors => _events.Any(x => x.EventType == ConversionOutputEventType.Error);
        public abstract long TotalBytes { get; }
        public abstract bool Success { get; }

        public abstract byte[] GetBytes();
        public abstract Stream GetStream();

        internal virtual void SetResult(object result)
        {
            _stopwatch.Stop();
        }


        internal ConversionResult()
        {
            _stopwatch = Stopwatch.StartNew();
            _events = new List<ConversionOutputEvent>();
        }

        internal void AddEvent(ConversionOutputEvent ev)
            => _events.Add(ev);
    }
}
