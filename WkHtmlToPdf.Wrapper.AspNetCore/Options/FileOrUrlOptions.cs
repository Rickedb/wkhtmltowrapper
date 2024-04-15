namespace WkHtmlToPdf.Wrapper.AspNetCore.Options
{
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
