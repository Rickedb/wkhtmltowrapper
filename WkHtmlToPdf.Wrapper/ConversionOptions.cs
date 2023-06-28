namespace WkHtmlToPdf.Wrapper
{
    public struct HtmlOptions
    {
        public string Html { get; set; }
        public string OutputPath { get; set; }
    }

    public struct FileOrUrlOptions
    {
        public string InputHtmlPathOrUrl { get; set; }
        public string OutputPath { get; set; }

        public FileOrUrlOptions(string inputHtmlPathOrUrl)
        {
            InputHtmlPathOrUrl = inputHtmlPathOrUrl;
            OutputPath = null;
        }

        public FileOrUrlOptions(string inputHtmlPathOrUrl, string outputPath)
        {
            InputHtmlPathOrUrl = inputHtmlPathOrUrl;
            OutputPath = null;
        }
    }
}
