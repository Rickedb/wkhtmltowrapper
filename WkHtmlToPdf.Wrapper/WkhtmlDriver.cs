using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WkHtmlToPdf.Wrapper
{
    public abstract class WkHtmlDriver
    {
        private readonly string _wkhtmlPath;

        internal abstract string WkHtmlExe { get; }

        public event EventHandler<OutputEvent> OutputEvent;

        public WkHtmlDriver(string wkhtmlPath)
        {
            _wkhtmlPath = wkhtmlPath;
        }

        /// <summary>
        /// Converts given URL or HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmlPath">Path to wkthmltopdf\wkthmltoimage.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <param name="wkhtmlExe"></param>
        /// <returns>PDF as byte array.</returns>
        protected async Task<ConversionResult> ConvertHtmlAsync(string switches, string html, string outputPath)
        {
            // switches:
            //     " -"  - switch output to stdout
            //     "- -" - switch input to stdin and output to stdout
            //     "- <path>" - stdin input and output to path
            //     "<url/file-path> -" output to stdout
            //     "<url/file-path> <path>" - just paths
            //switches += " -";

            // generate PDF from given HTML string, not from URL
            if (!string.IsNullOrEmpty(html))
            {
                switches += "\"C:\\GitHub\\wkhtmltowrapper\\WkHtmlToPdf.Wrapper.Tests\\Html\\SimpleHtml.html\" -";
            }

            var proc = CreateWkHtmlProcess(switches);
            proc.Start();

            // generate PDF from given HTML string, not from URL
            if (!string.IsNullOrEmpty(html))
            {
                using (var sIn = proc.StandardInput)
                {
                    await sIn.WriteLineAsync(html);
                }
            }

            var result = new FileConversionResult();
            var logsTask = Task.Run(async () =>
            {
                string log = string.Empty;
                while (!string.IsNullOrEmpty(log = await proc.StandardError.ReadLineAsync()))
                {
                    var ev = Wrapper.OutputEvent.Parse(log);
                    result.AddEvent(ev);
                    OutputEvent?.Invoke(this, ev);
                }
            });

            var ms = new MemoryStream();
            using (var sOut = proc.StandardOutput.BaseStream)
            {
                byte[] buffer = new byte[4096];
                int read;

                while ((read = await sOut.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await ms.WriteAsync(buffer, 0, read);
                }
            }

            //result.SetResult(ms);
            if (proc.ExitCode > 0)
            {
                
            }

            proc.WaitForExit();
            logsTask.Dispose();
            return result;
        }

        private Process CreateWkHtmlProcess(string arguments)
        {
            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(_wkhtmlPath, WkHtmlExe),
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    WorkingDirectory = _wkhtmlPath,
                    CreateNoWindow = true
                }
            };
        }
    }
}