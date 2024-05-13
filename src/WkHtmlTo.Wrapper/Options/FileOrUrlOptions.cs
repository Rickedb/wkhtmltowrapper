namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// File or url conversion options for wkhtmltopdf
    /// </summary>
    public class FileOrUrlOptions : PdfOptions, IFileOrUrlOptions
    {
        /// <summary>
        /// Path to the HTML file to be converted or the url that will be downloaded to convert to PDF
        /// </summary>
        public string HtmlFilePathOrUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="FileOrUrlOptions"/> class with the html file path or url
        /// </summary>
        /// <param name="filePathOrUrl">HTML file path to be converted or the url that will be downloaded to convert to PDF</param>
        public FileOrUrlOptions(string filePathOrUrl) : this(filePathOrUrl, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="FileOrUrlOptions"/> class with the html file path or url and the path of the output file
        /// </summary>
        /// <param name="filePathOrUrl">HTML file path to be converted or the url that will be downloaded to convert to PDF</param>
        /// <param name="outputPath">File path where the converted PDF file will be saved</param>
        public FileOrUrlOptions(string filePathOrUrl, string outputPath) : base(outputPath)
        {
            HtmlFilePathOrUrl = filePathOrUrl;
        }
    }
}
