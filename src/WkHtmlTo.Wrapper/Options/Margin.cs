using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
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

        public Margin()
        {
        }

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