namespace WkHtmlToPdf.Wrapper.Options
{
    public class HeaderOptions
    {
        /// <summary>
        /// Centered header text
        /// <para><em>AKA: <c>--header-center</c></em></para>
        /// </summary>
        [OptionFlag("--header-center")]
        public string CenterText { get; set; }

        /// <summary>
        /// Set header font name
        /// <para><em>AKA: <c>--header-font-name</c></em></para>
        /// </summary>
        [OptionFlag("--header-font-name")]
        public string FontName { get; set; } = "Arial";

        /// <summary>
        /// Set header font size
        /// <para><em>AKA: <c>--header-font-size</c></em></para>
        /// </summary>
        [OptionFlag("--header-font-size")]
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// Adds a html header
        /// <para><em>AKA: <c>--header-html</c></em></para>
        /// </summary>
        [OptionFlag("--header-html")]
        public string Html { get; set; }

        /// <summary>
        /// Left aligned header text
        /// <para><em>AKA: <c>--header-left</c></em></para>
        /// </summary>
        [OptionFlag("--header-left")]
        public string LeftText { get; set; }

        /// <summary>
        /// Display line above the header
        /// <para><em>AKA: <c>--header-line</c> and <c>--no-header-line</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--header-line", "--no-header-line")]
        public bool DisplaySeparatorLine { get; set; }

        /// <summary>
        /// Right aligned header text
        /// <para><em>AKA: <c>--header-right</c></em></para>
        /// </summary>
        [OptionFlag("--header-right")]
        public string RightText { get; set; }

        /// <summary>
        /// Spacing between header and content in mm
        /// <para><em>AKA: <c>--header-spacing</c></em></para>
        /// </summary>
        [OptionFlag("--header-spacing")]
        public double Spacing { get; set; } = 0;
        //Replace values
    }
}
