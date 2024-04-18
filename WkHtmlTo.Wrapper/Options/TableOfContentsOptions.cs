namespace WkHtmlTo.Wrapper.Options
{
    public class TableOfContentsOptions : Options, IOptions
    {
        [OptionFlag("--toc")]
        public bool Enabled  => true;

        [OptionFlag("--disable-dotted-lines")]
        public bool DisableDottedLines { get; set; }
        [OptionFlag("--toc-header-text")]
        public string HeaderText { get; set; }
        [OptionFlag("--toc-level-indentation")]
        public string IdentationLevelWidth { get; set; } = "1em";
        [OptionFlag("--disable-toc-links")]
        public bool DisableLinks { get; set; }
        [OptionFlag("--toc-text-size-shrink")]
        public double TextSizeShrink { get; set; } = .8d;
        [OptionFlag("--xsl-style-sheet")]
        public string XslStyleSheetPath { get; set; }
    }
}
