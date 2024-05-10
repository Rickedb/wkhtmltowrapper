using System;
using System.Collections.Generic;
using System.Diagnostics;
using WkHtmlTo.Wrapper.Logging;

namespace WkHtmlTo.Wrapper
{
    /// <summary>
    /// Represents errors that occur during <c>wkhtmlto</c> executable operation
    /// </summary>
    public class WkHtmlToException : Exception
    {
        private readonly Process _process;

        /// <summary>
        /// Location of the <c>wkhtmlto</c> executable
        /// </summary>
        public string ExecutablePath => _process.StartInfo.FileName;

        /// <summary>
        /// Plain text arguments that were passed to the executable
        /// </summary>
        public string Arguments => _process.StartInfo.Arguments;

        /// <summary>
        /// Executable out-of-process exit code
        /// </summary>
        public int ExitCode => _process.ExitCode;

        /// <summary>
        /// List of output events that occurred while trying to convert to pdf
        /// </summary>
        public IEnumerable<ConversionOutputEvent> Events { get; }

        internal WkHtmlToException(Process process, IEnumerable<ConversionOutputEvent> events) : base("An error has occurred inside wktohtml process, please check additional output logs if necessary")
        {
            _process = process;
            Events = events;
        }
    }
}
