namespace WkHtmlToPdf.Wrapper.AspNetCore.Options
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
}
