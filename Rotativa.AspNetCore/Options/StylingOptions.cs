namespace Rotativa.AspNetCore.Options
{
    public class StylingOptions
    {
        /// <summary>
        /// Change the dpi explicitly (this has no effect on X11 based systems)
        /// <para><em>AKA: <c>-d</c> or </c><c>--dpi</c></em></para>
        /// </summary>
        [OptionFlag("-d")]
        public int Dpi { get; set; } = 96;

        /// <summary>
        /// PDF will be generated in grayscale
        /// <para><em>AKA: <c>-g</c> or </c><c>--grayscale</c></em></para>
        /// </summary>
        [OptionFlag("-g")]
        public bool Grayscale { get; set; }

        /// <summary>
        /// When embedding images scale them down to this dpi
        /// <para><em>AKA: <c>--image-dpi</c></em></para>
        /// </summary>
        [OptionFlag("--image-dpi")]
        public int ImageDpi { get; set; } = 600;

        /// <summary>
        /// When jpeg compressing images use this quality
        /// <para><em>AKA: <c>--image-quality</c></em></para>
        /// </summary>
        [OptionFlag("--image-quality")]
        public int ImageQuality { get; set; } = 94;

        /// <summary>
        /// Page height
        /// <para><em>AKA: <c>--page-height</c></em></para>
        /// </summary>
        [OptionFlag("--page-height")]
        public double PageHeight { get; set; }

        /// <summary>
        /// Set paper size to: A4, Letter, etc.
        /// <para><em>AKA: <c>-s</c> or </c><c>--page-size</c></em></para>
        /// </summary>
        [OptionFlag("-s")]
        public PageSize PageSize { get; set; } = PageSize.A4;

        /// <summary>
        /// Page width
        /// <para><em>AKA: <c>--page-width</c></em></para>
        /// </summary>
        [OptionFlag("--page-width")]
        public double PageWidth { get; set; }

        /// <summary>
        /// Generates lower quality pdf/ps. Useful to shrink the result document space
        /// <para><em>AKA: <c>-l</c> or </c><c>--lowquality</c></em></para>
        /// </summary>
        [OptionFlag("-l")]
        public bool LowQuality { get; set; }

        /// <summary>
        /// PDF margin definition
        /// </summary>
        public Margin Margin { get; set; } = new Margin();

        /// <summary>
        /// Set the starting page number
        /// <para><em>AKA: <c>--page-offset</c></em></para>
        /// </summary>
        [OptionFlag("--page-offset")]
        public int PageOffset { get; set; } = 0;

        /// <summary>
        /// Minimum font size
        /// <para><em>AKA: <c>--minimum-font-size</c></em></para>
        /// </summary>
        [OptionFlag("--minimum-font-size")]
        public int? MinimumFontSize { get; set; }

        /// <summary>
        /// Do print background
        /// para><em>AKA: <c>--background</c> and <c>--no-background</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--background", "--no-background")]
        public bool Background { get; set; } = true;

        /// <summary>
        /// Use this SVG file when rendering checked checkboxes
        /// <para><em>AKA: <c>--checkbox-checked-svg</c></em></para>
        /// </summary>
        [OptionFlag("--checkbox-checked-svg")]
        public string CheckedCheckboxSVG { get; set; }

        /// <summary>
        ///  Use this SVG file when rendering unchecked checkboxes
        ///  <para><em>AKA: <c>--checkbox-svg</c></em></para>
        /// </summary>
        [OptionFlag("--checkbox-svg")]
        public string UncheckedCheckboxSVG { get; set; }

        /// <summary>
        /// Use this SVG file when rendering checked radiobuttons
        /// <para><em>AKA: <c>--radiobutton-checked-svg</c></em></para>
        /// </summary>
        [OptionFlag("--radiobutton-checked-svg")]
        public string CheckedRadioButtonSVG { get; set; }

        /// <summary>
        /// Use this SVG file when rendering unchecked radiobuttons
        /// <para><em>AKA: <c>--radiobutton-svg</c></em></para>
        /// </summary>
        [OptionFlag("--radiobutton-svg")]
        public string UncheckedRadioButtonSVG { get; set; }

        /// <summary>
        /// Use this zoom factor
        /// <para><em>AKA: <c>--zoom</c></em></para>
        /// </summary>
        [OptionFlag("--zoom")]
        public double ZoomFactor { get; set; } = 1;
    }
}
