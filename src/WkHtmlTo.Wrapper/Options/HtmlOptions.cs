namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// Plain html text conversion options for wkhtmltopdf
    /// </summary>
    public class HtmlOptions : PdfOptions, IHtmlOptions
    {
        /// <summary>
        /// Plain html text that will be converted to PDF
        /// </summary>
        public string Html { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="HtmlOptions"/> class with a plain html text
        /// </summary>
        /// <param name="html">HTML content for conversion</param>
        public HtmlOptions(string html) : this(html, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="HtmlOptions"/> class with a plain html text and the path of the output file
        /// </summary>
        /// <param name="html">HTML content for conversion</param>
        /// <param name="outputPath">File path where the converted PDF file will be saved</param>
        public HtmlOptions(string html, string outputPath) : base(outputPath) 
        {
            Html = html;
        }
    }
}
