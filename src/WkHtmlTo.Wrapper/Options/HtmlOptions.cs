namespace WkHtmlTo.Wrapper.Options
{
    public class HtmlOptions : PdfOptions, IHtmlOptions
    {
        public string Html { get; set; }

        public HtmlOptions(string html) : this(html, null)
        {

        }

        public HtmlOptions(string html, string outputPath) : base(outputPath) 
        {
            Html = html;
        }
    }
}
