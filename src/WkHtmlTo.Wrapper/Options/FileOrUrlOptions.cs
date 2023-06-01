namespace WkHtmlTo.Wrapper.Options
{
    public class FileOrUrlOptions : PdfOptions, IFileOrUrlOptions
    {
        public string HtmlFilePathOrUrl { get; set; }

        public FileOrUrlOptions(string filePathOrUrl) : this(filePathOrUrl, null)
        {

        }

        public FileOrUrlOptions(string filePathOrUrl, string outputPath) : base(outputPath)
        {
            HtmlFilePathOrUrl = filePathOrUrl;
        }
    }
}
