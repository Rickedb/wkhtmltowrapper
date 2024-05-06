using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WkHtmlTo.Wrapper.Logging;
using WkHtmlTo.Wrapper.Options;

namespace WkHtmlTo.Wrapper
{
    public class WkHtmlToPdfWrapper
    {
        private static readonly string _wkHtmlExe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "wkhtmltopdf";
        private readonly string _wkHtmlPath;

        public event EventHandler<ConversionOutputEvent> OutputEvent;

        public WkHtmlToPdfWrapper() : this(string.Empty)
        {
        }

        public WkHtmlToPdfWrapper(string wkHtmlExecutablePath)
        {
            _wkHtmlPath = wkHtmlExecutablePath;
        }

        public Task<ConversionResult> ConvertAsync(IHtmlOptions options, CancellationToken cancellationToken = default)
            => ConvertAsync((IPdfOptions)options, cancellationToken);

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

            var arguments = args.ToString();
            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(_wkHtmlPath, _wkHtmlExe),
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                WorkingDirectory = _wkHtmlPath,
                CreateNoWindow = true
            });

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
                    OutputEvent?.Invoke(this, ev);
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

            return result;
        }
    }
}

