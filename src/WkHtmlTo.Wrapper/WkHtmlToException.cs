using System;
using System.Collections.Generic;
using System.Diagnostics;
using WkHtmlTo.Wrapper.Logging;

namespace WkHtmlTo.Wrapper
{
    public class WkHtmlToException : Exception
    {
        private readonly Process _process;

        public string ExecutablePath => _process.StartInfo.FileName;
        public string Arguments => _process.StartInfo.Arguments;
        public int ExitCode => _process.ExitCode;
        public IEnumerable<ConversionOutputEvent> Events { get; }

        internal WkHtmlToException(Process process, IEnumerable<ConversionOutputEvent> events) : base("An error has occurred inside wktohtml process, please check additional output logs if necessary")
        {
            _process = process;
            Events = events;
        }
    }
}
