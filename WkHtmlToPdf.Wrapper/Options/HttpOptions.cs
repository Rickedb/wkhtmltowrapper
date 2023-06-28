using System.Collections.Generic;

namespace WkHtmlToPdf.Wrapper.Options
{
    public class HttpOptions : Options, IOptions
    {
        /// <summary>
        /// Add HTTP headers specified by <c>--custom-header</c> for each resource request
        /// <para><em>AKA: <c>--custom-header-propagation</c> and <c>--no-custom-header-propagation</c></em></para>
        /// </summary>
        [ToggleOptionFlag("--custom-header-propagation", "--no-custom-header-propagation")]
        public bool CustomHeaderPropagation { get; set; } = false;

        /// <summary>
        /// Set an additional HTTP header
        /// <para><em>AKA: <c>--custom-header</c></em></para>
        /// </summary>
        [OptionFlag("--custom-header")]
        public Dictionary<string, string> CustomHeaders { get; set; }

        /// <summary>
        /// HTTP Authentication username
        /// <para><em>AKA: <c>--username</c></em></para>
        /// </summary>
        [OptionFlag("--username")]
        public string HttpAuthenticationUsername { get; set; }

        /// <summary>
        /// HTTP Authentication password
        /// <para><em>AKA: <c>--password</c></em></para>
        /// </summary>
        [OptionFlag("--password")]
        public string HttpAuthenticationPassword { get; set; }
    }
}
