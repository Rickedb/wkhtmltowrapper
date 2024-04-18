namespace WkHtmlTo.Wrapper.Options
{
    public class FooterOptions : Options, IOptions
    {
        /// <summary>
        /// Centered footer text
        /// <para><em>AKA: <c>--footer-center</c></em></para>
        /// </summary>
        [OptionFlag("--footer-center")]
        public string CenterText { get; set; }

        /// <summary>
        /// Set footer font name
        /// <para><em>AKA: <c>--footer-font-name</c></em></para>
        /// </summary>
        [OptionFlag("--footer-font-name")]
        public string FontName { get; set; } = "Arial";

        /// <summary>
        /// Set footer font size
        /// <para><em>AKA: <c>--footer-font-size</c></em></para>
        /// </summary>
        [OptionFlag("--footer-font-size")]
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// Adds a html footer
        /// <para><em>AKA: <c>--footer-html</c></em></para>
        /// </summary>
        [OptionFlag("--footer-html")]
        public string HtmlUrl { get; set; }

        /// <summary>
        /// Left aligned footer text
        /// <para><em>AKA: <c>--footer-left</c></em></para>
        /// </summary>
        [OptionFlag("--footer-left")]
        public string LeftText { get; set; }

        /// <summary>
        /// Display line above the footer
        /// <para><em>AKA: <c>--footer-line</c> and <c>--no-footer-line</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--footer-line", "--no-footer-line")]
        public bool DisplaySeparatorLine { get; set; } = false;

        /// <summary>
        /// Right aligned footer text
        /// <para><em>AKA: <c>--footer-right</c></em></para>
        /// </summary>
        [OptionFlag("--footer-right")]
        public string RightText { get; set; }

        /// <summary>
        /// Spacing between footer and content in mm
        /// <para><em>AKA: <c>--footer-spacing</c></em></para>
        /// </summary>
        [OptionFlag("--footer-spacing")]
        public double Spacing { get; set; } = 1;
    }
}
