﻿using System.Collections.Generic;
using WkHtmlTo.Wrapper.Flags;

namespace WkHtmlTo.Wrapper.Options
{
    /// <summary>
    /// Represents all proxy options that can be set to wkhtmlto
    /// </summary>
    public class ProxyOptions : Options, IOptions
    {
        /// <summary>
        /// Use a proxy
        /// <para><em>AKA: <c>-p</c> or </c><c>--proxy</c></em></para>
        /// </summary>
        [OptionFlag("--proxy")]
        public string Url { get; set; }

        /// <summary>
        /// Use the proxy for resolving hostnames
        /// <para><em>AKA: <c>--proxy-hostname-lookup</c></em></para>
        /// </summary>
        [OptionFlag("--proxy-hostname-lookup")]
        public bool HostnameLookup { get; set; }

        /// <summary>
        /// Bypass proxy for host
        /// <para><em>AKA: <c>--bypass-proxy-for</c></em></para>
        /// </summary>
        [OptionFlag("--bypass-proxy-for")]
        public List<string> BypassFor { get; set; }
    }
}
