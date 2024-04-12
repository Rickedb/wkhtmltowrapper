using WkHtmlToPdf.Wrapper.Options;

namespace WkHtmlToPdf.Wrapper
{
    public class HtmlOptions : PdfOptions
    {
        public string Html { get; set; }

        public HtmlOptions(string html) : this(html, null)
        {
            
        }

        public HtmlOptions(string html, string outputPath)
        {
            Html = html;
            OutputPath = outputPath;
        }
    }

    public class FileOrUrlOptions : PdfOptions
    {
        public string FilePathOrUrl { get; set; }

        public FileOrUrlOptions(string filePathOrUrl) : this(filePathOrUrl, null)
        {
            
        }

        public FileOrUrlOptions(string filePathOrUrl, string outputPath)
        {
            FilePathOrUrl = filePathOrUrl;
            OutputPath = outputPath;
        }
    }
}
