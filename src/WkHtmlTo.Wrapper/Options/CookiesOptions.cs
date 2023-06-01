using System.Collections.Generic;
using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
    public class CookiesOptions : Options, IOptions
    {
        /// <summary>
        /// Set an additional cookie (repeatable), value should be url encoded.
        /// <para><em>AKA: <c>--cookie</c></em></para>
        /// </summary>
        [OptionFlag("--cookie")]
        public Dictionary<string, string> Cookies { get; set; }

        /// <summary>
        /// Read and write cookies from and to the supplied cookie jar file
        /// <para><em>AKA: <c>--cookie-jar</c></em></para>
        /// </summary>
        [PathOptionFlag("--cookie-jar")]
        public string JarPath { get; set; }
    }
}
