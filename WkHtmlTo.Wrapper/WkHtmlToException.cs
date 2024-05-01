using System;
using System.Diagnostics;

namespace WkHtmlTo.Wrapper
{
    public class WkHtmlToException : Exception
    {
        private readonly Process _process;

        public string ExecutablePath => _process.StartInfo.FileName;
        public string Arguments => _process.StartInfo.Arguments;
        public int ExitCode => _process.ExitCode;

        internal WkHtmlToException(Process process) : base("An error has occurred inside wktohtml process, please check additional output logs if necessary")
        {
            _process = process;
        }
    }
}
