namespace WkHtmlTo.Wrapper.Options
{
    public class OutlineOptions : Options, IOptions
    {
        /// <summary>
        /// Dump the default TOC xsl style sheet to stdout
        /// <para><em>AKA: <c>--dump-default-toc-xsl</c></em></para> 
        /// </summary>
        [OptionFlag("--dump-default-toc-xsl")]
        public bool DumpDefaultTableOfContentsXsl { get; set; }

        /// <summary>
        /// Dump the outline to a file
        /// <para><em>AKA: <c>--dump-outline</c></em></para> 
        /// </summary>
        [OptionFlag("--dump-outline")]
        public string DumpOutlinePath { get; set; }

        /// <summary>
        /// Put an outline into the pdf
        /// <para><em>AKA: <c>--outline</c> and <c>--no-outline</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--outline", "--no-outline")]
        public bool Outline { get; set; } = true;

        /// <summary>
        /// Set the depth of the outline
        /// <para><em>AKA: <c>--outline-depth</c></em></para> 
        /// </summary>
        [OptionFlag("--outline-depth")]
        public int OutlineDepth { get; set; } = 4;
    }
}
