using System.Collections.Generic;

namespace WkHtmlToPdf.Wrapper.Options
{
    public abstract class PdfOptions : Options, IOptions
    {
        public string OutputPath { get; set; }

        /// <summary>
        /// Collate when printing multiple copies
        /// <para><em>AKA: <c>--collate</c> and <c>--no-collate</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--collate", "--no-collate")]
        public bool Collate { get; set; } = true;
        
        /// <summary>
        /// Number of copies to print into the pdf file (default 1)
        /// <para><em>AKA: <c>--copies</c></em></para>
        /// </summary>
        [OptionFlag("--copies")]
        public int Copies { get; set; } = 1;

        /// <summary>
        /// Set log level to: none, error, warn or info
        /// <para><em>AKA: <c>--log-level</c></em></para>
        /// </summary>
        [OptionFlag("--log-level")]
        public PromptLogLevel LogLevel { get; set; } = PromptLogLevel.Info;

        /// <summary>
        /// Set orientation to Landscape or Portrait
        /// <para><em>AKA: <c>--O</c> or <c>--orientation</c></em></para>
        /// </summary>
        [OptionFlag("--orientation")]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary>
        /// Do not use lossless compression on pdf objects
        /// <para><em>AKA: <c>--no-pdf-compression</c></em></para>
        /// </summary>
        [OptionFlag("--no-pdf-compression")]
        public bool NoPdfCompression { get; set; }

        /// <summary>
        /// The title of the generated pdf file (The title of the first document is used if not specified)
        /// <para><em>AKA: <c>--title</c></em></para>
        /// </summary>
        [OptionFlag("--title")]
        public string Title { get; set; }

        /// <summary>
        /// Use the X server (some plugins and other stuff might not work without X11)
        /// <para><em>AKA: <c>--use-xserver</c></em></para> 
        /// </summary>
        [OptionFlag("--use-xserver")]
        public bool UseXServer { get; set; }

        /// <summary>
        /// Add a default header, with the name of the page to the left, and the page number to
        /// the right, this is short for:
        /// <para>--header-left='[webpage]'</para>
        /// <para>--header-right='[page]/[toPage]' --top 2cm</para>
        /// <para>--header-line</para>
        /// <para><em>AKA: <c>--default-header</c></em></para> 
        /// </summary>
        [OptionFlag("--default-header")]
        public bool DefaultHeader { get; set; }

        /// <summary>
        /// Replace [name] with value in header and footer
        /// <para><em>AKA: <c>--replace</c></em></para> 
        /// </summary>
        [OptionFlag("--replace")]
        public Dictionary<string, string> ReplaceHeaderAndFooterValues { get; set; }

        /// <summary>
        /// Any extra option that you want to add
        /// </summary>
        public string CustomExtraOptions { get; set; }

        //public StylingOptions Styling { get; set; } = new StylingOptions();
        public ProxyOptions Proxy { get; set; } = new ProxyOptions();
        public PageOptions Page { get; set; } = new PageOptions();
        //public OutlineOptions Outline { get; set; } = new OutlineOptions();
        //public TableOfContentsOptions TableOfContents { get; set; } = new TableOfContentsOptions();
        public CookiesOptions CookiesOptions { get; set; } = new CookiesOptions();
        //public JavascriptOptions JavascriptOptions { get; set; } = new JavascriptOptions();
        //public HttpOptions HttpOptions { get; set; } = new HttpOptions();
        //public HeaderOptions HeaderOptions { get; set; } = new HeaderOptions();
        //public FooterOptions FooterOptions { get; set; } = new FooterOptions();

        public override string ToSwitchCommand()
        {
            var command = base.ToSwitchCommand();
            if(!string.IsNullOrWhiteSpace(CustomExtraOptions))
            {
                return string.Concat(command, " ", CustomExtraOptions);
            }
            return command;

            //return " --load-error-handling ignore ";
            //return " --load-media-error-handling abort ";
            //var command = base.ToSwitchCommand();
            //return $"{command} {CustomExtraOptions}".Trim();
        }
    }
}
