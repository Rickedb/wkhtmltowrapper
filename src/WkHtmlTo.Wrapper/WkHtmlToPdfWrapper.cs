using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.Logging;
using WkHtmlTo.Wrapper.Options;

[assembly: InternalsVisibleTo("WkHtmlToPdf.Wrapper.Tests")]
namespace WkHtmlTo.Wrapper
{
    /// <summary>
    /// Wraps and provides access to <c>wkhtmltopdf</c> executable
    /// </summary>
    public class WkHtmlToPdfWrapper
    {
        private static readonly string _wkHtmlExe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "wkhtmltopdf";
        private readonly string _wkHtmlPath;

        /// <summary>
        /// Occurs when a message is printed out in stdout at <c>wkhtmltopdf</c> out-of-process execution
        /// </summary>
        public event EventHandler<ConversionOutputEventArgs> OutputEvent;

        /// <summary>
        /// Initializes a new instance of <see cref="WkHtmlToPdfWrapper"/> without specifying the executable path
        /// </summary>
        public WkHtmlToPdfWrapper() : this(string.Empty)
        {
           
        }

        /// <summary>
        ///  Initializes a new instance of <see cref="WkHtmlToPdfWrapper"/> specifying the executable path
        /// </summary>
        /// <param name="wkHtmlExecutablePath">Folder path where your whkhtmltopdf executable is located</param>
        public WkHtmlToPdfWrapper(string wkHtmlExecutablePath)
        {
            _wkHtmlPath = wkHtmlExecutablePath;
        }

        /// <summary>
        /// Asynchronously converts a raw html to a pdf by executing whkhtmltopdf executable outside of main process and 
        /// monitors cancellation requests
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous conversion operation</returns>
        /// <exception cref="WkHtmlToException"></exception>
        public Task<ConversionResult> ConvertAsync(IHtmlOptions options, CancellationToken cancellationToken = default)
            => ConvertAsync((IPdfOptions)options, cancellationToken);

        /// <summary>
        /// Asynchronously converts an html from a file or a given url to a pdf by executing whkhtmltopdf executable 
        /// outside of main process and monitors cancellation requests
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous conversion operation</returns>
        /// <exception cref="WkHtmlToException"></exception>
        public Task<ConversionResult> ConvertAsync(IFileOrUrlOptions options, CancellationToken cancellationToken = default)
            => ConvertAsync((IPdfOptions)options, cancellationToken);

        private async Task<ConversionResult> ConvertAsync(IPdfOptions options, CancellationToken cancellationToken)
        {
            var args = new StringBuilder(options.ToSwitchCommand());
            if (options is IFileOrUrlOptions fileOrUrlOptions)
            {
                args.Append($" \"{fileOrUrlOptions.HtmlFilePathOrUrl}\"");
            }
            else
            {
                args.Append(" -");
            }

            ConversionResult result;
            var outputPath = options.OutputPath;
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                result = new StreamConversionResult();
                args.Append(" -");
            }
            else
            {
                result = new FileConversionResult();
                outputPath = PathInfo.GetAbsolutePath(outputPath);
                args.Append($" \"{outputPath}\"");
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(_wkHtmlPath, _wkHtmlExe),
                Arguments = args.ToString(),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                WorkingDirectory = _wkHtmlPath,
                CreateNoWindow = true
            };
            
            using (var proc = Process.Start(startInfo))
            {
                if (options is IHtmlOptions htmlOptions)
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
                        OutputEvent?.Invoke(this, new ConversionOutputEventArgs(ev));
                    }
                }, cancellationToken);

                if (result is StreamConversionResult)
                {
                    var ms = new MemoryStream();
                    using (var sOut = proc.StandardOutput.BaseStream)
                    {
                        var buffer = new byte[2048];
                        int read;

                        while ((read = await sOut.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
                        {
                            await ms.WriteAsync(buffer, 0, read, cancellationToken);
                        }
                    }
                    result.SetResult(ms);
                    await proc.CompatibleWaitForExitAsync(cancellationToken);
                }
                else
                {
                    await proc.CompatibleWaitForExitAsync(cancellationToken);
                    result.SetResult(outputPath);
                }

                if (proc.ExitCode > 0)
                {
                    throw new WkHtmlToException(proc, result.Events);
                }
            }

            return result;
        }
    }
}

