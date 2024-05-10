using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// Represents all pdf margin options that can be set to wkhtmlto
    /// </summary>
    public class Margin : Options, IOptions
    {
        /// <summary>
        /// Page bottom margin in mm.
        /// </summary>
        [OptionFlag("-B")]
        public double? Bottom;

        /// <summary>
        /// Page left margin in mm.
        /// </summary>
        [OptionFlag("-L")]
        public double? Left;

        /// <summary>
        /// Page right margin in mm.
        /// </summary>
        [OptionFlag("-R")]
        public double? Right;

        /// <summary>
        /// Page top margin in mm.
        /// </summary>
        [OptionFlag("-T")]
        public double? Top;

        /// <summary>
        /// Initializes a new instance of <see cref="Margin"/> options class
        /// </summary>
        public Margin()
        {

        }

        /// <summary>
        /// Returns a new <see cref="Margin"/> option with the all specified side values
        /// </summary>
        /// <param name="top">The double value that should be applied to the top side</param>
        /// <param name="right">The double value that should be applied to the right side</param>
        /// <param name="bottom">The double value that should be applied to the bottom side</param>
        /// <param name="left"> The double value that should be applied to the left side</param>
        public static Margin Only(double? top = null, double? right = null, double? bottom = null, double? left = null)
        {
            return new Margin()
            {
                Top = top,
                Right = right,
                Bottom = bottom,
                Left = left
            };
        }

        /// <summary>
        /// Returns a new <see cref="Margin"/> option with the both specified side direction values
        /// </summary>
        /// <param name="horizontal">The double value that should be applied to right and left sides</param>
        /// <param name="horizontal">The double value that should be applied to top and bottom sides</param>
        /// <returns></returns>
        public static Margin Symmetric(double? horizontal = null, double? vertical = null)
        {
            return new Margin()
            {
                Top = vertical,
                Right = horizontal,
                Bottom = vertical,
                Left = horizontal
            };
        }
        /// <summary>
        /// Returns a new <see cref="Margin"/> option with the value that should be applied to all four sides
        /// </summary>
        /// <param name="value">The double value that should be applied all sides</param>
        public static Margin All(double value)
        {
            return new Margin()
            {
                Top = value,
                Right = value,
                Bottom = value,
                Left = value
            };
        }
    }
}