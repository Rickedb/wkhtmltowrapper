using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper
{
    public class WkHtmlToPdfWrapper
    {
        private static readonly string _wkHtmlExe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "wkhtmltopdf";
        private readonly string _wkhtmlPath;

        public event EventHandler<ConversionOutputEvent> OutputEvent;

        public WkHtmlToPdfWrapper()
        {

        }

        public WkHtmlToPdfWrapper(string wkhtmlExecutablePath)
        {
            _wkhtmlPath = wkhtmlExecutablePath;
        }

        public Task<ConversionResult> GenerateAsync(HtmlOptions options, CancellationToken cancellationToken = default)
            => ConvertAsync(options, cancellationToken);

        public Task<ConversionResult> GenerateAsync(FileOrUrlOptions options, CancellationToken cancellationToken = default)
            => ConvertAsync(options, cancellationToken);


        /// <summary>
        /// Converts given HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmltopdfPath">Path to wkthmltopdf.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public ConversionResult ConvertFromHtml(string switches, string html)
        {
            throw new NotImplementedException();
            //return ConvertAsync(switches, html, wkhtmlExe).Result; //TODO: Remove .Result
        }

        /// <summary>
        /// Converts given HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmltopdfPath">Path to wkthmltopdf.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public ConversionResult ConvertFromHtml(PdfOptions options, string html)
        {
            var switches = options.ToSwitchCommand();
            return ConvertFromHtml(switches, html);
        }

        public Task<ConversionResult> ConvertHtmlAsync(string switches, string html)
        {
            throw new NotImplementedException();
            //ConvertAsync(switches, html, wkhtmlExe);
        }

        public Task<ConversionResult> GenerateAsync(PdfOptions options, string html)
        {
            var switches = options.ToSwitchCommand();
            return ConvertHtmlAsync(switches, html);
        }

        private async Task<ConversionResult> ConvertAsync(PdfOptions options, CancellationToken cancellationToken)
        {
            // switches:
            //     " -"  - switch output to stdout
            //     "- -" - switch input to stdin and output to stdout
            //     "- <path>" - stdin input and output to path
            //     "<url/file-path> -" output to stdout
            //     "<url/file-path> <path>" - just paths
            //switches += " -";
            var args = new StringBuilder(options.ToSwitchCommand());
            if(options is FileOrUrlOptions fileOrUrlOptions)
            {
                args.Append($" \"{fileOrUrlOptions.FilePathOrUrl}\"");
            }
            else
            {
                args.Append(" -");
            }

            ConversionResult result;
            if (string.IsNullOrWhiteSpace(options.OutputPath))
            {
                result = new StreamConversionResult();
                args.Append(" -");
            }
            else
            {
                result = new FileConversionResult();
                args.Append($" \"{options.OutputPath}\"");
            }

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(_wkhtmlPath, _wkHtmlExe),
                Arguments = args.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                WorkingDirectory = _wkhtmlPath,
                CreateNoWindow = true
            });

            if (options is HtmlOptions htmlOptions)
            {
                using (var stdIn = proc.StandardInput)
                {
                    await stdIn.WriteLineAsync(htmlOptions.Html);
                }
            }

            _ = Task.Run(async () =>
            {
                string log = string.Empty;
                while (!string.IsNullOrEmpty(log = await proc.StandardError.ReadLineAsync()))
                {
                    var ev = ConversionOutputEvent.Parse(log);
                    result.AddEvent(ev);
                    OutputEvent?.Invoke(this, ev);
                }
            }, cancellationToken);

            if (result is StreamConversionResult)
            {
                var ms = new MemoryStream();
                using (var sOut = proc.StandardOutput.BaseStream)
                {
                    byte[] buffer = new byte[4096];
                    int read;

                    while ((read = await sOut.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                    {
                        await ms.WriteAsync(buffer, 0, read, cancellationToken);
                    }
                }
                result.SetResult(ms);
            }
            else
            {
                result.SetResult(options.OutputPath);
            }

            await proc.CompatibleWaitForExitAsync(cancellationToken);
            if (proc.ExitCode > 0)
            {

            }

            return result;
        }
    }
}

