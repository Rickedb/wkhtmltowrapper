using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WkHtmlToPdf.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper
{
    public class WkHtmlToPdfDriver : WkHtmlDriver
    {
        /// <summary>
        /// wkhtmltopdf only has a .exe extension in Windows.
        /// </summary>
        internal override string WkHtmlExe => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "wkhtmltopdf";

        public WkHtmlToPdfDriver(string wkhtmlPath) : base(wkhtmlPath)
        {
        }

        public Task<ConversionResult> ConvertAsync(string switches, HtmlOptions conversionOptions)
            => ConvertAsync(switches, html, wkhtmlExe);

        public Task<ConversionResult> ConvertAsync(PdfOptions options, HtmlOptions conversionOptions)
        {
            var switches = options.ToSwitchCommand();
            return ConvertHtmlAsync(switches, html);
        }

        public Task<ConversionResult> ConvertAsync(string switches, FileOrUrlOptions conversionOptions)
            => ConvertAsync(switches, html, wkhtmlExe);

        public Task<ConversionResult> ConvertAsync(PdfOptions options, FileOrUrlOptions conversionOptions)
        {
            var switches = options.ToSwitchCommand();
            return ConvertHtmlAsync(switches, html);
        }


        /// <summary>
        /// Converts given HTML string to PDF.
        /// </summary>
        /// <param name="wkhtmltopdfPath">Path to wkthmltopdf.</param>
        /// <param name="switches">Switches that will be passed to wkhtmltopdf binary.</param>
        /// <param name="html">String containing HTML code that should be converted to PDF.</param>
        /// <returns>PDF as byte array.</returns>
        public ConversionResult ConvertFromHtml(string switches, string html)
        {
            return ConvertAsync(switches, html, wkhtmlExe).Result; //TODO: Remove .Result
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
            => ConvertAsync(switches, html, wkhtmlExe);

        public Task<ConversionResult> ConvertHtmlAsync(PdfOptions options, string html)
        {
            var switches = options.ToSwitchCommand();
            return ConvertHtmlAsync(switches, html);
        }

        public Task<ConversionResult> ConvertHtmlAsync(string switches, string html, string outputPath)
           => ConvertAsync(switches, html, wkhtmlExe);

        public Task<ConversionResult> ConvertHtmlAsync(PdfOptions options, string html, string outputPath)
        {
            var switches = options.ToSwitchCommand();
            return ConvertHtmlAsync(switches, html);
        }

        public Task<ConversionResult> ConvertFromFileAsync(string switches, string htmlPath)
        {
            return ConvertAsync(switches, string.Empty, wkhtmlExe);
        }

        public Task<ConversionResult> ConvertFromFileAsync(PdfOptions options, string htmlPath)
        {
            var switches = options.ToSwitchCommand();
            return ConvertFromUriAsync(switches, "");
        }

        public Task<ConversionResult> ConvertFromFileAsync(string switches, string htmlPath, string outputPath)
        {
            return ConvertAsync(switches, string.Empty, wkhtmlExe);
        }

        public Task<ConversionResult> ConvertFromFileAsync(PdfOptions options, string htmlPath, string outputPath)
        {
            var switches = options.ToSwitchCommand();
            return ConvertFromUriAsync(switches, "");
        }

        public Task<ConversionResult> ConvertFromUriAsync(string switches, string uri)
        {
            switches = $"{switches} {uri}";
            return ConvertAsync(switches, string.Empty, wkhtmlExe);
        }

        public Task<ConversionResult> ConvertFromUriAsync(PdfOptions options, string uri)
        {
            var switches = options.ToSwitchCommand();
            return ConvertFromUriAsync(switches, uri);
        }


        public Task<ConversionResult> ConvertFromUriAsync(string switches, string uri, string outputPath)
        {
            switches = $"{switches} {uri}";
            return ConvertAsync(switches, string.Empty, wkhtmlExe);
        }

        public Task<ConversionResult> ConvertFromUriAsync(PdfOptions options, string uri, string outputPath)
        {
            var switches = options.ToSwitchCommand();
            return ConvertFromUriAsync(switches, uri);
        }
    }
}

