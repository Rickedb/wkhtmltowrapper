using System.Collections.Generic;

namespace Rotativa.AspNetCore.Options
{
    public class CookiesOptions
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
        [OptionFlag("--cookie-jar")]
        public string JarPath { get; set; }
    }
}
